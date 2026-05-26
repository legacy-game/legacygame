using Legacy.Commands;
using Legacy.History;
using Legacy.Save;
using Legacy.World;
using NUnit.Framework;

namespace Legacy.Tests.EditMode
{
    public sealed class JobSystemTests
    {
        [Test]
        public void JobLifecycle_PostingApplicationContractShiftAndTask_CompletesThroughSharedState()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var workerId = new WorldEntityId("citizen_noaharan");
            var employerId = new WorldEntityId("citizen_noaharan");
            var postingId = new WorldEntityId("posting_cafe_worker_001");
            var applicationId = new WorldEntityId("application_noah_cafe");
            var contractId = new WorldEntityId("contract_noah_cafe");
            var shiftId = new WorldEntityId("shift_noah_cafe");
            var taskId = new WorldEntityId("task_noah_cafe_serve");
            var workplaceId = new WorldEntityId("workplace_linden_cafe");

            Assert.That(runtime.Execute(new ApplyForJobCommand(applicationId, postingId, workerId)).Succeeded, Is.True);
            Assert.That(runtime.Execute(new OfferJobCommand(applicationId, employerId)).Succeeded, Is.True);
            Assert.That(runtime.Execute(new AcceptJobOfferCommand(contractId, applicationId, workerId)).Succeeded, Is.True);
            Assert.That(runtime.Execute(new StartShiftCommand(shiftId, contractId, 120)).Succeeded, Is.True);
            Assert.That(runtime.Execute(new CreateJobTaskCommand(taskId, JobTaskCatalog.ServeCafeCustomer, workplaceId, new WorldEntityId("citizen_mr_holland"))).Succeeded, Is.True);
            Assert.That(runtime.Execute(new StartJobTaskCommand(taskId, shiftId, workerId)).Succeeded, Is.True);
            Assert.That(runtime.Execute(new SubmitMiniGameResultCommand(taskId, workerId, 90, 100, 45, 1)).Succeeded, Is.True);

            int workerStart = runtime.State.MoneyAccountsById[new WorldEntityId("account_noaharan_cash")].BalanceCents;
            int beansStart = runtime.State.WorkplaceInventoriesById[workplaceId].CountOf("coffee_beans");
            WorldCommandResult completed = runtime.Execute(new CompleteJobTaskCommand(taskId, workerId));

            Assert.That(completed.Succeeded, Is.True);
            Assert.That(runtime.State.JobTasksById[taskId].Status, Is.EqualTo(JobTaskStatus.Completed));
            Assert.That(runtime.State.MoneyAccountsById[new WorldEntityId("account_noaharan_cash")].BalanceCents, Is.EqualTo(workerStart + 125));
            Assert.That(runtime.State.WorkplaceInventoriesById[workplaceId].CountOf("coffee_beans"), Is.EqualTo(beansStart - 1));
            Assert.That(runtime.State.WorkplaceInventoriesById[workplaceId].CountOf("prepared_coffee"), Is.EqualTo(1));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.JobTaskCompleted).Count, Is.EqualTo(1));
            Assert.That(runtime.State.GetOrCreateSkill(workerId, SkillKind.Barista).Experience, Is.EqualTo(90));
        }

        [Test]
        public void StartShift_FailsWithoutActiveContract()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());

            WorldCommandResult result = runtime.Execute(new StartShiftCommand(
                new WorldEntityId("shift_missing_contract"),
                new WorldEntityId("contract_missing"),
                60));

            Assert.That(result.Succeeded, Is.False);
        }

        [Test]
        public void UnauthorizedWorker_CannotStartAnotherWorkersTask()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var workerId = new WorldEntityId("citizen_noaharan");
            var otherWorkerId = new WorldEntityId("citizen_rowan");
            var postingId = new WorldEntityId("posting_cafe_worker_001");
            var applicationId = new WorldEntityId("application_noah_cafe");
            var contractId = new WorldEntityId("contract_noah_cafe");
            var shiftId = new WorldEntityId("shift_noah_cafe");
            var taskId = new WorldEntityId("task_noah_cafe_serve");
            var workplaceId = new WorldEntityId("workplace_linden_cafe");

            runtime.Execute(new ApplyForJobCommand(applicationId, postingId, workerId));
            runtime.Execute(new OfferJobCommand(applicationId, new WorldEntityId("citizen_noaharan")));
            runtime.Execute(new AcceptJobOfferCommand(contractId, applicationId, workerId));
            runtime.Execute(new StartShiftCommand(shiftId, contractId, 120));
            runtime.Execute(new CreateJobTaskCommand(taskId, JobTaskCatalog.ServeCafeCustomer, workplaceId));

            WorldCommandResult result = runtime.Execute(new StartJobTaskCommand(taskId, shiftId, otherWorkerId));

            Assert.That(result.Succeeded, Is.False);
        }

        [Test]
        public void JobSystemRoundTrip_PreservesContractsShiftsTasksInventorySkillsAndHistory()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var workerId = new WorldEntityId("citizen_noaharan");
            var postingId = new WorldEntityId("posting_cafe_worker_001");
            var applicationId = new WorldEntityId("application_noah_cafe");
            var contractId = new WorldEntityId("contract_noah_cafe");
            var shiftId = new WorldEntityId("shift_noah_cafe");
            var taskId = new WorldEntityId("task_noah_cafe_serve");
            var workplaceId = new WorldEntityId("workplace_linden_cafe");

            runtime.Execute(new ApplyForJobCommand(applicationId, postingId, workerId));
            runtime.Execute(new OfferJobCommand(applicationId, new WorldEntityId("citizen_noaharan")));
            runtime.Execute(new AcceptJobOfferCommand(contractId, applicationId, workerId));
            runtime.Execute(new StartShiftCommand(shiftId, contractId, 120));
            runtime.Execute(new CreateJobTaskCommand(taskId, JobTaskCatalog.ServeCafeCustomer, workplaceId));
            runtime.Execute(new StartJobTaskCommand(taskId, shiftId, workerId));
            runtime.Execute(new SubmitMiniGameResultCommand(taskId, workerId, 80, 100, 50, 2));
            runtime.Execute(new CompleteJobTaskCommand(taskId, workerId));

            WorldState loaded = WorldSaveMapper.ToRuntime(WorldSaveMapper.ToSaveData(runtime.State));

            Assert.That(loaded.EmploymentContractsById.ContainsKey(contractId), Is.True);
            Assert.That(loaded.ShiftsById.ContainsKey(shiftId), Is.True);
            Assert.That(loaded.JobTasksById[taskId].Status, Is.EqualTo(JobTaskStatus.Completed));
            Assert.That(loaded.WorkplaceInventoriesById[workplaceId].CountOf("prepared_coffee"), Is.EqualTo(1));
            Assert.That(loaded.GetOrCreateSkill(workerId, SkillKind.Barista).Experience, Is.EqualTo(80));
            Assert.That(loaded.GetHistoryByKind(HistoryEventKind.JobTaskCompleted).Count, Is.EqualTo(1));
        }

        [Test]
        public void NpcBackfill_UsesEmploymentContractPath()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());

            WorldCommandResult result = runtime.Execute(new NpcBackfillJobCommand(
                new WorldEntityId("contract_npc_pharmacy"),
                new WorldEntityId("posting_pharmacy_clerk_001")));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(runtime.State.EmploymentContractsById.ContainsKey(new WorldEntityId("contract_npc_pharmacy")), Is.True);
        }
    }
}
