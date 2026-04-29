# v13 — CancellationToken propagation across all async surfaces

## Why this branch exists

During the v12 migration we threaded `CancellationToken` through `IPublisher.PublishAsync` and `ICronusStartup.BootstrapAsync`. Mid-flight we expanded scope to thread CT through every async dispatch interface — handlers, sagas, projections, application services. That expansion turned out to be much larger than initially scoped: an exhaustive sweep across all 12 framework repos surfaced ~50+ async signatures still missing CT, with significant downstream impact on every Cronus consumer app.

Rather than holding v12.0.0 hostage to that work, the CT-everywhere expansion was split off into v13. v12 ships with the publisher/startup CT work only; v13 picks up where this branch leaves off.

## Branch base

This branch is rebased on top of the current `preview` HEAD (which contains the revert of the CT-handler work). Resuming v13 just means continuing on top of this branch after v12 has fully shipped to master.

## Status of v13 work

### ✅ Already done on this branch

Commit subject: `feat: Threads CancellationToken through all async dispatch surfaces`. Adds CT to:

**Interfaces (8):**
- `ICommandHandler<T>.HandleAsync`
- `IEventHandler<T>.HandleAsync`
- `IPublicEventHandler<T>.HandleAsync`
- `ISignalHandle<T>.HandleAsync`
- `ISagaTimeoutHandler<T>.HandleAsync`
- `IProjectionDefinition.ApplyAsync`
- `IAmEventSourcedProjection.ReplayEventAsync`
- `IAmEventSourcedProjection.OnReplayCompletedAsync`

**Abstract / virtual on base classes (3):**
- `Saga.RequestTimeoutAsync<T>`
- `ProjectionDefinition.OnReplayCompletedAsync`
- `AggregateRootApplicationService<AR>.UpdateAsync`

All 11 surfaces have full XML doc summaries and `<param>` lines. Existing tests (84/84) pass against the new contract because the CT param is defaulted.

### ⏳ Still pending in DomainModeling for v13

**`IAggregateRepository` (2 methods):**
- `Task SaveAsync<AR>(AR aggregateRoot)` → add `CancellationToken cancellationToken = default`
- `Task<ReadResult<AR>> LoadAsync<AR>(AggregateRootId id)` → add CT

The interface currently has empty `<param>` placeholders that should be filled in while the file is being touched.

**`IProjectionReader` (4 methods):**
- `Task<ReadResult<T>> GetAsync<T>(IBlobId projectionId)` → add CT
- `Task<ReadResult<T>> GetAsOfAsync<T>(IBlobId projectionId, DateTimeOffset timestamp)` → add CT
- `Task<ReadResult<IProjectionDefinition>> GetAsync(IBlobId projectionId, Type projectionType)` → add CT
- `Task<ReadResult<IProjectionDefinition>> GetAsOfAsync(IBlobId projectionId, Type projectionType, DateTimeOffset timestamp)` → add CT

After both interfaces gain CT, also thread the token inside `AggregateRootApplicationService.UpdateAsync` body (currently calls `repository.LoadAsync(id)` and `repository.SaveAsync(result.Data)` without passing CT — the implementation noted this and left it for when the repo signatures could accept a token).

## Companion work in `Elders/Cronus` core repo

Sister branch: `v13-cancellation-tokens-cascade`.

That branch contains the cascade through Cronus core to match the new CT-aware DomainModeling contract — every `HandleAsync` implementation in core (and test fixtures) gets the CT param, plus new CT-aware signatures on Cronus core's own internal interfaces (`IConsumer`, `IEventStore`, `IMigrationCustomLogic`, `ICronusMigrator`, `IAggregateCommitInterceptor`, `IAggregateCommitHandle`, `IEventStoreIndex`, `CronusJob` abstracts, `WorkflowBase` abstracts, `RetryPolicy.ExecuteActionAsync`, `PlayerOperator` callback delegates, `IIndexStore`, `IMessageCounter`).

## Still pending in Cronus core for v13 (NOT yet on the cascade branch)

Investigation across `src/Elders.Cronus/**` surfaced these additional interfaces / abstracts that also need CT:

- `IAggregateRootAtomicAction.ExecuteAsync`
- `ILock.IsLockedAsync` / `LockAsync` / `UnlockAsync`
- `IAutoUpdate.ApplyAsync`
- `ICronusJobRunner.CancelAsync` × 3 + `CancelAllAsync`
- `IDangerZone.WipeDataAsync` + `DangerZoneExecutor` impl
- `ICronusHost.StartAsync` / `StopAsync` + `CronusHost` impl
- `IRpcHost.StartAsync` / `StopAsync`
- `ISubscriber.ProcessAsync` + `SubscriberBase` impl
- `MigrationRunnerBase.RunAsync` (abstract) + 4 derived overrides (`CopyEventStore`, `DeleteEventStoreEvents`, `ValidateEventStore`)
- `IProjectionStore.EnumerateProjectionsAsync` / `SaveAsync`
- `IInitializableProjectionStore.InitializeAsync`
- `IProjectionStoreStorageManager.CreateProjectionsStorageAsync`
- `IProjectionWriter.SaveAsync` × 2 + impls in `ProjectionRepository` and `ProjectionRepositoryWithFallback`
- `ProgressTracker.InitializeAsync`

## Still pending in satellites for v13

| Repo | Surfaces |
|---|---|
| `Cronus.Transport.RabbitMQ` | `IRequestHandler<TReq, TRes>.HandleAsync` (1) |
| `Cronus.Persistence.Cassandra` | `ICronusMessageStore.AppendAsync` + `LoadMessagesAsync`; cascade impls of `IIndexStore` / `IMessageCounter` / `IEventStore` |
| `Cronus.Projections.Cassandra` | `IProjectionPartionsStore.AppendAsync` + `GetPartitionsAsync`; `IProjectionStoreSchemaNew.CreateProjectionStorageNewAsync`; `IProjectionStore` impls |
| `Cronus.AtomicAction.Redis` | `IRevisionStore.PrepareRevisionAsync` + `SaveRevisionAsync`; `IAtomicAction` / `ILock` impls |
| `Cronus.Api`, `Cronus.AspNetCore`, `Cronus.Monitor`, `Cronus.Cluster.Consul`, `Cronus.Serialization.NewtonsoftJson`, `Elders.Cronus.SourceGeneration.AutoUpdater` | none |

## Migration burden for downstream Cronus consumers (v12 → v13)

When v13 ships, consumer apps will need a one-time pass to add `CancellationToken cancellationToken = default` to:

1. **Every command/event/public-event/signal handler** — `HandleAsync` (high count)
2. **Every saga timeout handler + `Saga.RequestTimeoutAsync` calls** (low count)
3. **Every projection** — `ApplyAsync`, `ReplayEventAsync`, `OnReplayCompletedAsync` (medium count)
4. **Every `AggregateRootApplicationService<AR>` derivation** — `UpdateAsync` (low count)
5. **Custom `IAggregateRepository` test fakes/mocks** (low count, test-only)
6. **Custom `IPublisher` implementations** — already done at v12, no change here
7. **Custom `CronusJob` / `WorkflowBase` derivations** — abstract methods got CT (low count)
8. **Apps with custom RPC request handlers** (`Cronus.Transport.RabbitMQ`) — `IRequestHandler.HandleAsync` (low count)

Default-value parameter (`= default`) means existing call sites compile when no token is passed — **only the implementer signatures must update**.

## Resuming the v13 migration

Pre-requisites:
1. v12.0.0 has shipped to NuGet (DomainModeling, Cronus core, all satellites — through Phase 12 of the v12 plan)
2. master is up to date

Suggested approach when resuming:
1. Branch from latest `master` of Cronus.DomainModeling
2. Cherry-pick or merge `v13-cancellation-tokens` into the new branch
3. Work the "Still pending in DomainModeling" list above
4. Ship `13.0.0-preview.1` with `major:` prefix
5. Repeat per repo: branch from master → merge `v13-cancellation-tokens-cascade` (for Cronus core) → work the pending list → ship

The order below mirrors the v12 plan's repo dependency chain:
1. `Cronus.DomainModeling` → ship v13
2. `Cronus` → bump dep to v13, complete cascade, ship
3. `Cronus.Transport.RabbitMQ`
4. `Cronus.Persistence.Cassandra`
5. `Cronus.Projections.Cassandra`
6. `Cronus.AtomicAction.Redis`
7. Satellites with no CT scope of their own — bump dep, rebuild, ship

## Why we split the work

- v12 was already a major release. Bundling another ~50 surface changes inflated scope and risk.
- The handler-CT migration has different downstream impact than the publisher migration: it touches every domain handler in every consumer app, whereas the publisher migration mostly touches infrastructure code.
- The user prefers shipping smaller, well-scoped breaking changes rather than one mega-release.
- Decoupling lets v12 ship soon; v13 can land later when consumers have fully migrated to v12.
