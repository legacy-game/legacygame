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
