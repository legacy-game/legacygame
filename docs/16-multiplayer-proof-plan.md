# Multiplayer Proof Plan

This is the next step after the local command/state boundary is stable. Do not start here until the local café loop still works through `GameRuntime.Execute(...)`.

## Goal

Prove that the café can run as an authoritative multiplayer simulation with two clients.

This is not the MMO. This is only the first proof that the architecture can support server-owned world state.

## Test Scenario

```text
host starts café region
client 1 joins
client 2 joins
both clients see the same time, same NPCs, same active order, same till
only one client can successfully prepare/serve an order
server owns WorldState
clients render snapshots
```

## Architecture

```text
client input
-> command object
-> transport
-> server GameRuntime
-> WorldState mutation
-> state snapshot/event
-> client presentation update
```

## First Commands To Network

- `MoveCharacterCommand`
- `TalkToNpcCommand`
- `PrepareOrderCommand`
- `ServeOrderCommand`

## First Server Rules

- server owns `WorldState`
- clients never mutate world state directly
- clients submit commands
- server validates commands
- server broadcasts state changes
- local single-player should keep using the same command path

## First Technical Version

Use an in-editor or local-host setup first.

Phase order:

1. keep current local command path working
2. add an `IWorldTransport` interface
3. create a fake local transport that just calls the same runtime
4. create a second Unity player view using the fake transport
5. only then test a real networking package

## Current Foundation

The first server-authoritative prep layer is local-only and transport-neutral:

- `WorldCommandEnvelopeDto` is the serialized client input shape. It carries a protocol version, command id, client id, actor id, expected state revision, client sequence, command kind, and string key/value arguments.
- `WorldCommandEnvelopeMapper` is the only envelope-to-command materializer. It whitelists supported command kinds instead of accepting arbitrary C# type names.
- `WorldCommandEnvelopeValidator` performs deterministic validation before runtime execution, including protocol checks, duplicate argument checks, and stale state revision rejection.
- `WorldCommandResultDto`, `WorldEventDto`, and `WorldSnapshotDto` are the broadcast shapes clients can render without owning `WorldState`.
- `LocalAuthoritativeWorldHost` and `LocalWorldClient` prove the host/client split without a networking package. The host owns `WorldRuntime`; clients submit envelopes and receive result plus snapshot DTOs.

The scaffold currently supports the commands most useful for the local café proof: `AdvanceTime`, `DoWorldAction`, `StartJobTask`, `SubmitMiniGameResult`, `CompleteJobTask`, `EndShift`, and `SwitchScene`.

Next transport step: add a small `IWorldTransport` facade over the local host/client API, then use it from UI code without changing command semantics.

## Local Transport Proof

The current proof now includes an in-memory transport layer:

- `IWorldTransport` is the local transport contract for connecting clients, submitting serialized command envelopes, and reading the latest host snapshot.
- `InMemoryWorldTransport` owns a `LocalAuthoritativeWorldHost`; it does not create sockets, accounts, lobbies, or production networking concerns.
- `IWorldTransportClient` is the client-side contract; `InMemoryWorldTransportClient` creates command envelopes from its last known server revision and receives every server result broadcast, including snapshots and events for commands submitted by other clients.
- Two-client tests cover initial snapshot agreement, host command submission broadcast, event delivery, and a contested job task where the first completion wins and the stale second completion is rejected.

This keeps the server-authoritative boundary transport-neutral while proving that client views can converge from host-owned snapshots/results.

## Success Criteria

- two clients can move in one café scene
- both see the same clock/till/order state
- Holland/Sasha orders are server-owned
- if both players try to serve, only the valid first command succeeds
- save/load serializes the server `WorldState`

## Do Not Build Yet

- accounts
- login
- dedicated server deployment
- database
- regions/shards
- voice chat
- anti-cheat
- authoritative physics

Those come after this proof.
