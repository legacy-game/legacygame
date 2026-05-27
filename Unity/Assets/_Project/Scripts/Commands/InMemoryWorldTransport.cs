using System;
using System.Collections.Generic;
using Legacy.World;

namespace Legacy.Commands
{
    public interface IWorldTransport
    {
        int StateRevision { get; }
        long ServerSequence { get; }
        WorldSnapshotDto LatestSnapshot { get; }
        IReadOnlyList<IWorldTransportClient> Clients { get; }

        IWorldTransportClient Connect(string clientId);
        WorldCommandResultDto Submit(WorldCommandEnvelopeDto envelope);
    }

    public interface IWorldTransportClient
    {
        string ClientId { get; }
        int LastKnownStateRevision { get; }
        WorldSnapshotDto LastSnapshot { get; }
        WorldCommandResultDto LastResult { get; }
        IReadOnlyList<WorldCommandResultDto> ReceivedResults { get; }
        IReadOnlyList<WorldSnapshotDto> ReceivedSnapshots { get; }
        IReadOnlyList<WorldEventDto> ReceivedEvents { get; }

        WorldCommandEnvelopeDto CreateEnvelope(SerializedWorldCommandKind kind, string actorId = "");
        WorldCommandResultDto Submit(WorldCommandEnvelopeDto envelope);
    }

    public sealed class InMemoryWorldTransport : IWorldTransport
    {
        private readonly LocalAuthoritativeWorldHost _host;
        private readonly List<IWorldTransportClient> _clients = new();
        private readonly List<InMemoryWorldTransportClient> _localClients = new();
        private readonly Dictionary<string, InMemoryWorldTransportClient> _clientsById = new(StringComparer.Ordinal);

        public int StateRevision => _host.StateRevision;
        public long ServerSequence => _host.ServerSequence;
        public WorldSnapshotDto LatestSnapshot => _host.CreateSnapshot();
        public IReadOnlyList<IWorldTransportClient> Clients => _clients;

        public InMemoryWorldTransport(WorldState initialState)
            : this(new LocalAuthoritativeWorldHost(initialState))
        {
        }

        public InMemoryWorldTransport(LocalAuthoritativeWorldHost host)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
        }

        public IWorldTransportClient Connect(string clientId)
        {
            if (string.IsNullOrWhiteSpace(clientId)) {
                throw new ArgumentException("Client id is required.", nameof(clientId));
            }

            if (_clientsById.ContainsKey(clientId)) {
                throw new InvalidOperationException($"Client {clientId} is already connected.");
            }

            var client = new InMemoryWorldTransportClient(clientId, this, _host.CreateSnapshot());
            _clients.Add(client);
            _localClients.Add(client);
            _clientsById.Add(clientId, client);
            return client;
        }

        public WorldCommandResultDto Submit(WorldCommandEnvelopeDto envelope)
        {
            if (envelope == null) {
                throw new ArgumentNullException(nameof(envelope));
            }

            WorldCommandResultDto result = _host.Submit(envelope);
            Broadcast(result);
            return result;
        }

        private void Broadcast(WorldCommandResultDto result)
        {
            for (int i = 0; i < _localClients.Count; i++) {
                _localClients[i].Receive(result);
            }
        }
    }

    public sealed class InMemoryWorldTransportClient : IWorldTransportClient
    {
        private readonly InMemoryWorldTransport _transport;
        private readonly List<WorldCommandResultDto> _receivedResults = new();
        private readonly List<WorldSnapshotDto> _receivedSnapshots = new();
        private readonly List<WorldEventDto> _receivedEvents = new();
        private long _nextClientSequence = 1;

        public string ClientId { get; }
        public int LastKnownStateRevision { get; private set; }
        public WorldSnapshotDto LastSnapshot { get; private set; }
        public WorldCommandResultDto LastResult { get; private set; }
        public IReadOnlyList<WorldCommandResultDto> ReceivedResults => _receivedResults;
        public IReadOnlyList<WorldSnapshotDto> ReceivedSnapshots => _receivedSnapshots;
        public IReadOnlyList<WorldEventDto> ReceivedEvents => _receivedEvents;

        internal InMemoryWorldTransportClient(string clientId, InMemoryWorldTransport transport, WorldSnapshotDto initialSnapshot)
        {
            ClientId = clientId;
            _transport = transport ?? throw new ArgumentNullException(nameof(transport));
            LastSnapshot = initialSnapshot ?? throw new ArgumentNullException(nameof(initialSnapshot));
            LastKnownStateRevision = initialSnapshot.stateRevision;
            _receivedSnapshots.Add(initialSnapshot);
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

            return _transport.Submit(envelope);
        }

        internal void Receive(WorldCommandResultDto result)
        {
            LastResult = result ?? throw new ArgumentNullException(nameof(result));
            LastKnownStateRevision = result.stateRevision;
            _receivedResults.Add(result);

            if (result.snapshot != null) {
                LastSnapshot = result.snapshot;
                _receivedSnapshots.Add(result.snapshot);
            }

            for (int i = 0; i < result.events.Count; i++) {
                _receivedEvents.Add(result.events[i]);
            }
        }
    }
}
