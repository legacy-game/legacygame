using Legacy.Commands;
using Legacy.World;
using NUnit.Framework;
using UnityEngine;

namespace Legacy.Tests.EditMode
{
    public sealed class MultiplayerPrepTests
    {
        [Test]
        public void EnvelopeValidator_RejectsStaleRevisionBeforeCommandCreation()
        {
            WorldCommandEnvelopeDto envelope = CreateAdvanceTimeEnvelope("client_a", 0, 10);

            WorldCommandValidationResult result = WorldCommandEnvelopeValidator.Validate(envelope, 2);

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Error, Does.Contain("stale"));
        }

        [Test]
        public void LocalHost_AppliesWhitelistedEnvelopeAndReturnsSerializableSnapshot()
        {
            var host = new LocalAuthoritativeWorldHost(WorldFactory.CreateVeyneSeedWorld());
            var client = new LocalWorldClient("client_a", host);
            WorldCommandEnvelopeDto envelope = client.CreateEnvelope(SerializedWorldCommandKind.AdvanceTime);
            envelope.AddArgument("minutes", "10");

            WorldCommandResultDto result = client.Submit(envelope);
            string json = JsonUtility.ToJson(result);

            Assert.That(result.succeeded, Is.True);
            Assert.That(result.stateRevision, Is.EqualTo(1));
            Assert.That(result.snapshot.currentTime.hour, Is.EqualTo(6));
            Assert.That(result.snapshot.currentTime.minute, Is.EqualTo(40));
            Assert.That(result.snapshot.citizens.Count, Is.GreaterThan(0));
            Assert.That(result.events.Count, Is.GreaterThanOrEqualTo(1));
            Assert.That(json, Does.Contain("client_a_000001"));
        }

        [Test]
        public void LocalHost_RejectsStaleClientWithoutAdvancingRevision()
        {
            var host = new LocalAuthoritativeWorldHost(WorldFactory.CreateVeyneSeedWorld());
            WorldCommandEnvelopeDto first = CreateAdvanceTimeEnvelope("client_a", 0, 10);
            WorldCommandEnvelopeDto stale = CreateAdvanceTimeEnvelope("client_b", 0, 10);

            WorldCommandResultDto firstResult = host.Submit(first);
            WorldCommandResultDto staleResult = host.Submit(stale);

            Assert.That(firstResult.succeeded, Is.True);
            Assert.That(staleResult.succeeded, Is.False);
            Assert.That(staleResult.message, Does.Contain("stale"));
            Assert.That(host.StateRevision, Is.EqualTo(1));
            Assert.That(host.State.CurrentTime.Time.Hour, Is.EqualTo(6));
            Assert.That(host.State.CurrentTime.Time.Minute, Is.EqualTo(40));
        }

        [Test]
        public void LocalHost_OnlyFirstClientCanCompleteContendedTask()
        {
            WorldRuntime setupRuntime = CreateReadyToCompleteTaskRuntime();
            var host = new LocalAuthoritativeWorldHost(setupRuntime);
            var firstClient = new LocalWorldClient("client_a", host);
            var secondClient = new LocalWorldClient("client_b", host);
            WorldCommandEnvelopeDto firstComplete = firstClient.CreateEnvelope(SerializedWorldCommandKind.CompleteJobTask, "citizen_noaharan");
            WorldCommandEnvelopeDto secondComplete = secondClient.CreateEnvelope(SerializedWorldCommandKind.CompleteJobTask, "citizen_noaharan");
            firstComplete.AddArgument("taskId", "task_noah_cafe_mp");
            secondComplete.AddArgument("taskId", "task_noah_cafe_mp");

            WorldCommandResultDto firstResult = firstClient.Submit(firstComplete);
            WorldCommandResultDto secondResult = secondClient.Submit(secondComplete);

            Assert.That(firstResult.succeeded, Is.True);
            Assert.That(secondResult.succeeded, Is.False);
            Assert.That(secondResult.message, Does.Contain("stale"));
            Assert.That(host.State.JobTasksById[new WorldEntityId("task_noah_cafe_mp")].Status, Is.EqualTo(JobTaskStatus.Completed));
            Assert.That(host.StateRevision, Is.EqualTo(1));
        }

        private static WorldCommandEnvelopeDto CreateAdvanceTimeEnvelope(string clientId, int expectedRevision, int minutes)
        {
            var envelope = new WorldCommandEnvelopeDto {
                commandId = $"{clientId}_advance",
                clientId = clientId,
                kind = SerializedWorldCommandKind.AdvanceTime.ToString(),
                expectedStateRevision = expectedRevision
            };
            envelope.AddArgument("minutes", minutes.ToString());
            return envelope;
        }

        private static WorldRuntime CreateReadyToCompleteTaskRuntime()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var workerId = new WorldEntityId("citizen_noaharan");
            var applicationId = new WorldEntityId("application_noah_cafe_mp");
            var contractId = new WorldEntityId("contract_noah_cafe_mp");
            var shiftId = new WorldEntityId("shift_noah_cafe_mp");
            var taskId = new WorldEntityId("task_noah_cafe_mp");
            var workplaceId = new WorldEntityId("workplace_linden_cafe");

            runtime.Execute(new ApplyForJobCommand(applicationId, new WorldEntityId("posting_cafe_worker_001"), workerId));
            runtime.Execute(new OfferJobCommand(applicationId, new WorldEntityId("citizen_noaharan")));
            runtime.Execute(new AcceptJobOfferCommand(contractId, applicationId, workerId));
            runtime.Execute(new StartShiftCommand(shiftId, contractId, 120));
            runtime.Execute(new CreateJobTaskCommand(taskId, JobTaskCatalog.ServeCafeCustomer, workplaceId));
            runtime.Execute(new StartJobTaskCommand(taskId, shiftId, workerId));
            runtime.Execute(new SubmitMiniGameResultCommand(taskId, workerId, 80, 100, 50, 2));
            return runtime;
        }
    }
}
