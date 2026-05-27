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

        [Test]
        public void AdvanceTime_HollandArrival_QueuesCafeVisitTask()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());

            WorldCommandResult result = runtime.Execute(new AdvanceTimeCommand(100));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(runtime.State.VisitsById.Count, Is.GreaterThanOrEqualTo(1));
            Assert.That(runtime.State.TryGetNextQueuedTask(new WorldEntityId("workplace_linden_cafe"), WorldActionKind.ServeCustomer, out JobTaskState task), Is.True);
            Assert.That(task.TargetEntityId, Is.EqualTo(new WorldEntityId("citizen_mr_holland")));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.VisitArrived).Count, Is.GreaterThanOrEqualTo(1));
        }

        [Test]
        public void AdvanceTime_SashaArrival_QueuesPharmacyVisitTask()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var pharmacyId = new WorldEntityId("workplace_pell_pharmacy");

            WorldCommandResult result = runtime.Execute(new AdvanceTimeCommand(135));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(runtime.State.TryGetNextQueuedTask(pharmacyId, WorldActionKind.StockShelves, out JobTaskState task), Is.True);
            Assert.That(task.TargetEntityId, Is.EqualTo(new WorldEntityId("citizen_sasha")));
            Assert.That(runtime.State.TryGetVisitForTask(task.Id, out VisitState visit), Is.True);
            Assert.That(visit.Status, Is.EqualTo(VisitStatus.WaitingForTask));
        }

        [Test]
        public void AdvanceTime_ReprocessingVisitWindow_DoesNotDuplicateVisitTask()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());

            runtime.Execute(new AdvanceTimeCommand(100));
            int visitsAfterArrival = runtime.State.VisitsById.Count;
            int tasksAfterArrival = runtime.State.JobTasksById.Count;
            runtime.Execute(new AdvanceTimeCommand(1));

            Assert.That(runtime.State.VisitsById.Count, Is.EqualTo(visitsAfterArrival));
            Assert.That(runtime.State.JobTasksById.Count, Is.EqualTo(tasksAfterArrival));
        }

        [Test]
        public void VisitLinkedTaskCompletion_MarksVisitServedAndMorningProgress()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var workerId = new WorldEntityId("citizen_noaharan");
            var postingId = new WorldEntityId("posting_cafe_worker_001");
            var applicationId = new WorldEntityId("application_noah_cafe");
            var contractId = new WorldEntityId("contract_noah_cafe");
            var shiftId = new WorldEntityId("shift_noah_cafe");
            var workplaceId = new WorldEntityId("workplace_linden_cafe");

            runtime.Execute(new ApplyForJobCommand(applicationId, postingId, workerId));
            runtime.Execute(new OfferJobCommand(applicationId, new WorldEntityId("citizen_noaharan")));
            runtime.Execute(new AcceptJobOfferCommand(contractId, applicationId, workerId));
            runtime.Execute(new StartShiftCommand(shiftId, contractId, 120));
            runtime.Execute(new AdvanceTimeCommand(100));

            Assert.That(runtime.State.TryGetNextQueuedTask(workplaceId, WorldActionKind.ServeCustomer, out JobTaskState task), Is.True);
            TakeQueuedCafeOrder(runtime, task.Id, workerId);
            runtime.Execute(new StartJobTaskCommand(task.Id, shiftId, workerId));
            runtime.Execute(new SubmitMiniGameResultCommand(task.Id, workerId, 90, 100, 30, 0));
            WorldCommandResult result = runtime.Execute(new CompleteJobTaskCommand(task.Id, workerId));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(runtime.State.TryGetVisitForTask(task.Id, out VisitState visit), Is.True);
            Assert.That(visit.Status, Is.EqualTo(VisitStatus.WaitingForTask));
            Assert.That(visit.CafeStage, Is.EqualTo(CafeVisitStage.Receive));
            Assert.That(runtime.State.Morning.TasksCompleted, Is.EqualTo(1));
            Assert.That(runtime.State.Morning.MoneyEarnedCents, Is.EqualTo(125));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.VisitCompleted).Count, Is.EqualTo(0));
        }

        [Test]
        public void StartJobTask_RemovesTaskFromQueuedWork()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var workerId = new WorldEntityId("citizen_noaharan");
            var applicationId = new WorldEntityId("application_noah_cafe");
            var contractId = new WorldEntityId("contract_noah_cafe");
            var shiftId = new WorldEntityId("shift_noah_cafe");
            var workplaceId = new WorldEntityId("workplace_linden_cafe");

            runtime.Execute(new ApplyForJobCommand(applicationId, new WorldEntityId("posting_cafe_worker_001"), workerId));
            runtime.Execute(new OfferJobCommand(applicationId, new WorldEntityId("citizen_noaharan")));
            runtime.Execute(new AcceptJobOfferCommand(contractId, applicationId, workerId));
            runtime.Execute(new StartShiftCommand(shiftId, contractId, 120));
            runtime.Execute(new AdvanceTimeCommand(100));
            runtime.State.TryGetNextQueuedTask(workplaceId, WorldActionKind.ServeCustomer, out JobTaskState task);
            TakeQueuedCafeOrder(runtime, task.Id, workerId);

            WorldCommandResult result = runtime.Execute(new StartJobTaskCommand(task.Id, shiftId, workerId));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(runtime.State.WorkplacesById[workplaceId].QueuedTaskIds, Has.No.Member(task.Id));
            Assert.That(runtime.State.TryGetNextQueuedTask(workplaceId, WorldActionKind.ServeCustomer, out JobTaskState _), Is.False);
        }

        [Test]
        public void CompleteTask_SecondAttemptDoesNotPayOrWriteHistoryAgain()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var workerId = new WorldEntityId("citizen_noaharan");
            var applicationId = new WorldEntityId("application_noah_cafe");
            var contractId = new WorldEntityId("contract_noah_cafe");
            var shiftId = new WorldEntityId("shift_noah_cafe");
            var taskId = new WorldEntityId("task_noah_cafe_serve");
            var workplaceId = new WorldEntityId("workplace_linden_cafe");
            var workerAccountId = new WorldEntityId("account_noaharan_cash");

            runtime.Execute(new ApplyForJobCommand(applicationId, new WorldEntityId("posting_cafe_worker_001"), workerId));
            runtime.Execute(new OfferJobCommand(applicationId, new WorldEntityId("citizen_noaharan")));
            runtime.Execute(new AcceptJobOfferCommand(contractId, applicationId, workerId));
            runtime.Execute(new StartShiftCommand(shiftId, contractId, 120));
            runtime.Execute(new CreateJobTaskCommand(taskId, JobTaskCatalog.ServeCafeCustomer, workplaceId));
            runtime.Execute(new StartJobTaskCommand(taskId, shiftId, workerId));
            runtime.Execute(new SubmitMiniGameResultCommand(taskId, workerId, 80, 100, 40, 1));
            Assert.That(runtime.Execute(new CompleteJobTaskCommand(taskId, workerId)).Succeeded, Is.True);
            int workerAfterFirstCompletion = runtime.State.MoneyAccountsById[workerAccountId].BalanceCents;
            int transactionsAfterFirstCompletion = runtime.State.Transactions.Count;

            WorldCommandResult second = runtime.Execute(new CompleteJobTaskCommand(taskId, workerId));

            Assert.That(second.Succeeded, Is.False);
            Assert.That(runtime.State.MoneyAccountsById[workerAccountId].BalanceCents, Is.EqualTo(workerAfterFirstCompletion));
            Assert.That(runtime.State.Transactions.Count, Is.EqualTo(transactionsAfterFirstCompletion));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.JobTaskCompleted).Count, Is.EqualTo(1));
            Assert.That(runtime.State.ShiftsById[shiftId].EarnedCents, Is.EqualTo(125));
        }

        [Test]
        public void CompleteTask_MissingInventoryDoesNotPayOrMutate()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var workerId = new WorldEntityId("citizen_noaharan");
            var applicationId = new WorldEntityId("application_noah_cafe");
            var contractId = new WorldEntityId("contract_noah_cafe");
            var shiftId = new WorldEntityId("shift_noah_cafe");
            var taskId = new WorldEntityId("task_noah_cafe_serve");
            var workplaceId = new WorldEntityId("workplace_linden_cafe");
            var workerAccountId = new WorldEntityId("account_noaharan_cash");
            var businessAccountId = new WorldEntityId("account_linden_cafe");

            runtime.Execute(new ApplyForJobCommand(applicationId, new WorldEntityId("posting_cafe_worker_001"), workerId));
            runtime.Execute(new OfferJobCommand(applicationId, new WorldEntityId("citizen_noaharan")));
            runtime.Execute(new AcceptJobOfferCommand(contractId, applicationId, workerId));
            runtime.Execute(new StartShiftCommand(shiftId, contractId, 120));
            runtime.Execute(new CreateJobTaskCommand(taskId, JobTaskCatalog.ServeCafeCustomer, workplaceId));
            runtime.State.WorkplaceInventoriesById[workplaceId].TryRemove("coffee_beans", 25);
            runtime.Execute(new StartJobTaskCommand(taskId, shiftId, workerId));
            runtime.Execute(new SubmitMiniGameResultCommand(taskId, workerId, 80, 100, 40, 1));

            int workerStart = runtime.State.MoneyAccountsById[workerAccountId].BalanceCents;
            int businessStart = runtime.State.MoneyAccountsById[businessAccountId].BalanceCents;
            WorldCommandResult result = runtime.Execute(new CompleteJobTaskCommand(taskId, workerId));

            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.Message, Does.Contain("coffee_beans"));
            Assert.That(runtime.State.MoneyAccountsById[workerAccountId].BalanceCents, Is.EqualTo(workerStart));
            Assert.That(runtime.State.MoneyAccountsById[businessAccountId].BalanceCents, Is.EqualTo(businessStart));
            Assert.That(runtime.State.Transactions.Count, Is.EqualTo(0));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.PaymentRecorded).Count, Is.EqualTo(0));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.JobTaskCompleted).Count, Is.EqualTo(0));
            Assert.That(runtime.State.WorkplaceInventoriesById[workplaceId].CountOf("prepared_coffee"), Is.EqualTo(0));
            Assert.That(runtime.State.JobTasksById[taskId].Status, Is.EqualTo(JobTaskStatus.ResultSubmitted));
        }

        [Test]
        public void EndShift_FailsWhileTaskIsActive()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var workerId = new WorldEntityId("citizen_noaharan");
            var applicationId = new WorldEntityId("application_noah_cafe");
            var contractId = new WorldEntityId("contract_noah_cafe");
            var shiftId = new WorldEntityId("shift_noah_cafe");
            var taskId = new WorldEntityId("task_noah_cafe_serve");
            var workplaceId = new WorldEntityId("workplace_linden_cafe");

            runtime.Execute(new ApplyForJobCommand(applicationId, new WorldEntityId("posting_cafe_worker_001"), workerId));
            runtime.Execute(new OfferJobCommand(applicationId, new WorldEntityId("citizen_noaharan")));
            runtime.Execute(new AcceptJobOfferCommand(contractId, applicationId, workerId));
            runtime.Execute(new StartShiftCommand(shiftId, contractId, 120));
            runtime.Execute(new CreateJobTaskCommand(taskId, JobTaskCatalog.ServeCafeCustomer, workplaceId));
            runtime.Execute(new StartJobTaskCommand(taskId, shiftId, workerId));

            WorldCommandResult result = runtime.Execute(new EndShiftCommand(shiftId, workerId));

            Assert.That(result.Succeeded, Is.False);
            Assert.That(runtime.State.ShiftsById[shiftId].Status, Is.EqualTo(ShiftStatus.Active));
            Assert.That(runtime.State.ShiftSummaries.Count, Is.EqualTo(0));
        }

        [Test]
        public void EndMorning_ClearsUnservedVisitsAndQueuedTasks()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var workplaceId = new WorldEntityId("workplace_linden_cafe");

            runtime.Execute(new AdvanceTimeCommand(100));
            Assert.That(runtime.State.TryGetNextQueuedTask(workplaceId, WorldActionKind.ServeCustomer, out JobTaskState task), Is.True);

            WorldCommandResult result = runtime.Execute(new EndMorningCommand(new WorldEntityId("citizen_noaharan")));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(runtime.State.Morning.Status, Is.EqualTo(MorningStatus.Complete));
            Assert.That(runtime.State.JobTasksById[task.Id].Status, Is.EqualTo(JobTaskStatus.Cancelled));
            Assert.That(runtime.State.TryGetVisitForTask(task.Id, out VisitState visit), Is.True);
            Assert.That(visit.Status, Is.EqualTo(VisitStatus.Left));
            Assert.That(runtime.State.TryGetNextQueuedTask(workplaceId, WorldActionKind.ServeCustomer, out JobTaskState _), Is.False);
        }

        [Test]
        public void EndShiftAndMorning_WriteSummariesAndPersist()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var workerId = new WorldEntityId("citizen_noaharan");
            var postingId = new WorldEntityId("posting_cafe_worker_001");
            var applicationId = new WorldEntityId("application_noah_cafe");
            var contractId = new WorldEntityId("contract_noah_cafe");
            var shiftId = new WorldEntityId("shift_noah_cafe");
            var workplaceId = new WorldEntityId("workplace_linden_cafe");

            runtime.Execute(new ApplyForJobCommand(applicationId, postingId, workerId));
            runtime.Execute(new OfferJobCommand(applicationId, new WorldEntityId("citizen_noaharan")));
            runtime.Execute(new AcceptJobOfferCommand(contractId, applicationId, workerId));
            runtime.Execute(new StartShiftCommand(shiftId, contractId, 120));
            runtime.Execute(new AdvanceTimeCommand(100));
            runtime.State.TryGetNextQueuedTask(workplaceId, WorldActionKind.ServeCustomer, out JobTaskState task);
            TakeQueuedCafeOrder(runtime, task.Id, workerId);
            runtime.Execute(new StartJobTaskCommand(task.Id, shiftId, workerId));
            runtime.Execute(new SubmitMiniGameResultCommand(task.Id, workerId, 80, 100, 40, 1));
            runtime.Execute(new CompleteJobTaskCommand(task.Id, workerId));
            runtime.Execute(new EndShiftCommand(shiftId, workerId));
            runtime.Execute(new EndMorningCommand(workerId));

            WorldState loaded = WorldSaveMapper.ToRuntime(WorldSaveMapper.ToSaveData(runtime.State));

            Assert.That(loaded.ShiftSummaries.Count, Is.EqualTo(1));
            Assert.That(loaded.ShiftSummaries[0].TasksCompleted, Is.EqualTo(1));
            Assert.That(loaded.Morning.Status, Is.EqualTo(MorningStatus.Complete));
            Assert.That(loaded.Morning.TasksCompleted, Is.EqualTo(1));
            Assert.That(loaded.VisitsById.Count, Is.GreaterThanOrEqualTo(1));
            Assert.That(loaded.GetHistoryByKind(HistoryEventKind.MorningCompleted).Count, Is.EqualTo(1));
        }

        private static void TakeQueuedCafeOrder(WorldRuntime runtime, WorldEntityId taskId, WorldEntityId workerId)
        {
            Assert.That(runtime.State.TryGetVisitForTask(taskId, out VisitState visit), Is.True);
            Assert.That(runtime.Execute(new TakeCafeOrderCommand(visit.Id, workerId, CafeRecipeCatalog.HouseCoffee)).Succeeded, Is.True);
        }
    }
}
