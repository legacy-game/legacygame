using System;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class LocalAuthoritativeWorldHost
    {
        private readonly WorldRuntime _runtime;
        private int _stateRevision;
        private long _serverSequence;

        public WorldRuntime Runtime => _runtime;
        public WorldState State => _runtime.State;
        public int StateRevision => _stateRevision;
        public long ServerSequence => _serverSequence;

        public LocalAuthoritativeWorldHost(WorldState initialState)
            : this(new WorldRuntime(initialState))
        {
        }

        public LocalAuthoritativeWorldHost(WorldRuntime runtime)
        {
            _runtime = runtime ?? throw new ArgumentNullException(nameof(runtime));
        }

        public WorldSnapshotDto CreateSnapshot()
        {
            return WorldCommandSnapshotMapper.ToSnapshotDto(_runtime.State, _stateRevision);
        }

        public WorldCommandResultDto Submit(WorldCommandEnvelopeDto envelope)
        {
            _serverSequence++;
            if (!WorldCommandEnvelopeMapper.TryCreateCommand(envelope, _stateRevision, out IWorldCommand command, out WorldCommandValidationResult validation)) {
                return WorldCommandSnapshotMapper.ToResultDto(
                    envelope,
                    WorldCommandResult.Failure(validation.Error),
                    _runtime.State,
                    _stateRevision,
                    _serverSequence);
            }

            WorldCommandResult result = _runtime.Execute(command);
            if (result.Succeeded) {
                _stateRevision++;
            }

            return WorldCommandSnapshotMapper.ToResultDto(envelope, result, _runtime.State, _stateRevision, _serverSequence);
        }
    }

    public sealed class LocalWorldClient
    {
        private readonly LocalAuthoritativeWorldHost _host;
        private long _nextClientSequence = 1;

        public string ClientId { get; }
        public int LastKnownStateRevision { get; private set; }
        public WorldSnapshotDto LastSnapshot { get; private set; }
        public WorldCommandResultDto LastResult { get; private set; }

        public LocalWorldClient(string clientId, LocalAuthoritativeWorldHost host)
        {
            if (string.IsNullOrWhiteSpace(clientId)) {
                throw new ArgumentException("Client id is required.", nameof(clientId));
            }

            ClientId = clientId;
            _host = host ?? throw new ArgumentNullException(nameof(host));
            LastSnapshot = _host.CreateSnapshot();
            LastKnownStateRevision = LastSnapshot.stateRevision;
        }

        public WorldCommandEnvelopeDto CreateEnvelope(SerializedWorldCommandKind kind, string actorId = "")
        {
            return new WorldCommandEnvelopeDto {
                commandId = $"{ClientId}_{_nextClientSequence:000000}",
                clientId = ClientId,
                actorId = actorId ?? string.Empty,
                kind = kind.ToString(),
                expectedStateRevision = LastKnownStateRevision,
                clientSequence = _nextClientSequence++
            };
        }

        public WorldCommandResultDto Submit(WorldCommandEnvelopeDto envelope)
        {
            if (envelope == null) {
                throw new ArgumentNullException(nameof(envelope));
            }

            if (string.IsNullOrWhiteSpace(envelope.clientId)) {
                envelope.clientId = ClientId;
            }

            if (envelope.expectedStateRevision < 0) {
                envelope.expectedStateRevision = LastKnownStateRevision;
            }

            LastResult = _host.Submit(envelope);
            LastSnapshot = LastResult.snapshot;
            LastKnownStateRevision = LastResult.stateRevision;
            return LastResult;
        }
    }
}
