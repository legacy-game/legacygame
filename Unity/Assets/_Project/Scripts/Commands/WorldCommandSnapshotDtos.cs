using System;
using System.Collections.Generic;
using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    [Serializable]
    public sealed class WorldDateTimeDto
    {
        public int year;
        public int month;
        public int day;
        public int absoluteDay;
        public int hour;
        public int minute;
    }

    [Serializable]
    public sealed class WorldEventDto
    {
        public string id;
        public WorldDateTimeDto timestamp;
        public string kind;
        public string description;
        public List<string> actorIds = new();
        public List<string> placeIds = new();
    }

    [Serializable]
    public sealed class CitizenSnapshotDto
    {
        public string id;
        public string displayName;
        public string currentRegionId;
        public string currentSceneId;
        public string currentPlaceId;
        public int coordX;
        public int coordY;
        public string activity;
        public string currentIntent;
    }

    [Serializable]
    public sealed class WorkplaceSnapshotDto
    {
        public string id;
        public string placeId;
        public string ownerCitizenId;
        public string status;
        public List<string> activeShiftIds = new();
        public List<string> queuedTaskIds = new();
    }

    [Serializable]
    public sealed class JobTaskSnapshotDto
    {
        public string id;
        public string definitionId;
        public string workplaceId;
        public string assignedWorkerId;
        public string shiftId;
        public string targetEntityId;
        public string status;
        public int quality;
    }

    [Serializable]
    public sealed class VisitSnapshotDto
    {
        public string id;
        public string visitorCitizenId;
        public string workplaceId;
        public string linkedTaskId;
        public string status;
        public string cafeStage;
        public string recipeId;
        public int priceCents;
        public int prepQuality;
    }

    [Serializable]
    public sealed class MoneyAccountSnapshotDto
    {
        public string id;
        public string ownerEntityId;
        public string displayName;
        public int balanceCents;
    }

    [Serializable]
    public sealed class WorldSnapshotDto
    {
        public int protocolVersion = WorldCommandEnvelopeDto.CurrentProtocolVersion;
        public int schemaVersion;
        public long worldSeed;
        public int stateRevision;
        public WorldDateTimeDto currentTime;
        public string currentSceneId;
        public List<CitizenSnapshotDto> citizens = new();
        public List<WorkplaceSnapshotDto> workplaces = new();
        public List<JobTaskSnapshotDto> jobTasks = new();
        public List<VisitSnapshotDto> visits = new();
        public List<MoneyAccountSnapshotDto> moneyAccounts = new();
    }

    [Serializable]
    public sealed class WorldCommandResultDto
    {
        public int protocolVersion = WorldCommandEnvelopeDto.CurrentProtocolVersion;
        public string commandId;
        public string clientId;
        public long serverSequence;
        public int stateRevision;
        public bool succeeded;
        public string message;
        public List<string> changedEntityIds = new();
        public List<WorldEventDto> events = new();
        public WorldSnapshotDto snapshot;
    }

    public static class WorldCommandSnapshotMapper
    {
        public static WorldCommandResultDto ToResultDto(
            WorldCommandEnvelopeDto envelope,
            WorldCommandResult result,
            WorldState state,
            int stateRevision,
            long serverSequence)
        {
            var data = new WorldCommandResultDto {
                commandId = envelope?.commandId ?? string.Empty,
                clientId = envelope?.clientId ?? string.Empty,
                serverSequence = serverSequence,
                stateRevision = stateRevision,
                succeeded = result.Succeeded,
                message = result.Message,
                snapshot = ToSnapshotDto(state, stateRevision)
            };

            for (int i = 0; i < result.ChangedEntityIds.Count; i++) {
                data.changedEntityIds.Add(result.ChangedEntityIds[i].Value);
            }

            for (int i = 0; i < result.HistoryEvents.Count; i++) {
                data.events.Add(ToEventDto(result.HistoryEvents[i]));
            }

            data.changedEntityIds.Sort(StringComparer.Ordinal);
            return data;
        }

        public static WorldSnapshotDto ToSnapshotDto(WorldState state, int stateRevision)
        {
            var data = new WorldSnapshotDto {
                schemaVersion = state.SchemaVersion,
                worldSeed = state.WorldSeed,
                stateRevision = stateRevision,
                currentTime = ToDateTimeDto(state.CurrentTime),
                currentSceneId = state.CurrentSceneId.Value
            };

            var citizens = new List<CitizenState>(state.CitizensById.Values);
            citizens.Sort((left, right) => string.CompareOrdinal(left.Id.Value, right.Id.Value));
            for (int i = 0; i < citizens.Count; i++) {
                CitizenState citizen = citizens[i];
                data.citizens.Add(new CitizenSnapshotDto {
                    id = citizen.Id.Value,
                    displayName = citizen.DisplayName,
                    currentRegionId = citizen.CurrentRegionId.Value,
                    currentSceneId = citizen.CurrentSceneId.Value,
                    currentPlaceId = citizen.CurrentPlaceId.Value,
                    coordX = citizen.CurrentCoord.X,
                    coordY = citizen.CurrentCoord.Y,
                    activity = citizen.Activity.ToString(),
                    currentIntent = citizen.Routine.CurrentIntent
                });
            }

            var workplaces = new List<WorkplaceState>(state.WorkplacesById.Values);
            workplaces.Sort((left, right) => string.CompareOrdinal(left.Id.Value, right.Id.Value));
            for (int i = 0; i < workplaces.Count; i++) {
                WorkplaceState workplace = workplaces[i];
                var workplaceData = new WorkplaceSnapshotDto {
                    id = workplace.Id.Value,
                    placeId = workplace.PlaceId.Value,
                    ownerCitizenId = workplace.OwnerCitizenId.Value,
                    status = workplace.Status.ToString()
                };
                AddIds(workplaceData.activeShiftIds, workplace.ActiveShiftIds);
                AddIds(workplaceData.queuedTaskIds, workplace.QueuedTaskIds);
                data.workplaces.Add(workplaceData);
            }

            var tasks = new List<JobTaskState>(state.JobTasksById.Values);
            tasks.Sort((left, right) => string.CompareOrdinal(left.Id.Value, right.Id.Value));
            for (int i = 0; i < tasks.Count; i++) {
                JobTaskState task = tasks[i];
                data.jobTasks.Add(new JobTaskSnapshotDto {
                    id = task.Id.Value,
                    definitionId = task.DefinitionId,
                    workplaceId = task.WorkplaceId.Value,
                    assignedWorkerId = task.AssignedWorkerId.Value,
                    shiftId = task.ShiftId.Value,
                    targetEntityId = task.TargetEntityId.Value,
                    status = task.Status.ToString(),
                    quality = task.Quality
                });
            }

            var visits = new List<VisitState>(state.VisitsById.Values);
            visits.Sort((left, right) => string.CompareOrdinal(left.Id.Value, right.Id.Value));
            for (int i = 0; i < visits.Count; i++) {
                VisitState visit = visits[i];
                data.visits.Add(new VisitSnapshotDto {
                    id = visit.Id.Value,
                    visitorCitizenId = visit.VisitorCitizenId.Value,
                    workplaceId = visit.WorkplaceId.Value,
                    linkedTaskId = visit.LinkedTaskId.Value,
                    status = visit.Status.ToString(),
                    cafeStage = visit.CafeStage.ToString(),
                    recipeId = visit.RecipeId,
                    priceCents = visit.PriceCents,
                    prepQuality = visit.PrepQuality
                });
            }

            var accounts = new List<MoneyAccountState>(state.MoneyAccountsById.Values);
            accounts.Sort((left, right) => string.CompareOrdinal(left.Id.Value, right.Id.Value));
            for (int i = 0; i < accounts.Count; i++) {
                MoneyAccountState account = accounts[i];
                data.moneyAccounts.Add(new MoneyAccountSnapshotDto {
                    id = account.Id.Value,
                    ownerEntityId = account.OwnerEntityId.Value,
                    displayName = account.DisplayName,
                    balanceCents = account.BalanceCents
                });
            }

            return data;
        }

        public static WorldEventDto ToEventDto(HistoryEvent historyEvent)
        {
            var data = new WorldEventDto {
                id = historyEvent.Id.Value,
                timestamp = ToDateTimeDto(historyEvent.Timestamp),
                kind = historyEvent.Kind.ToString(),
                description = historyEvent.Description
            };

            for (int i = 0; i < historyEvent.ActorIds.Count; i++) {
                data.actorIds.Add(historyEvent.ActorIds[i].Value);
            }

            for (int i = 0; i < historyEvent.PlaceIds.Count; i++) {
                data.placeIds.Add(historyEvent.PlaceIds[i].Value);
            }

            data.actorIds.Sort(StringComparer.Ordinal);
            data.placeIds.Sort(StringComparer.Ordinal);
            return data;
        }

        private static WorldDateTimeDto ToDateTimeDto(Legacy.Time.GameDateTime dateTime)
        {
            return new WorldDateTimeDto {
                year = dateTime.Date.Year,
                month = dateTime.Date.Month,
                day = dateTime.Date.Day,
                absoluteDay = dateTime.Date.AbsoluteDay,
                hour = dateTime.Time.Hour,
                minute = dateTime.Time.Minute
            };
        }

        private static void AddIds(List<string> target, IReadOnlyList<WorldEntityId> ids)
        {
            for (int i = 0; i < ids.Count; i++) {
                target.Add(ids[i].Value);
            }

            target.Sort(StringComparer.Ordinal);
        }
    }
}
