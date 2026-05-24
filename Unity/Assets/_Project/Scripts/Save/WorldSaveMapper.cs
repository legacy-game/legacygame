using System;
using Legacy.History;
using Legacy.Time;
using Legacy.World;

namespace Legacy.Save
{
    public static class WorldSaveMapper
    {
        public static WorldSaveData ToSaveData(WorldState state)
        {
            var data = new WorldSaveData {
                schemaVersion = state.SchemaVersion,
                worldSeed = state.WorldSeed,
                currentTime = ToSaveData(state.CurrentTime),
                currentSceneId = state.CurrentSceneId.Value
            };

            foreach (RegionState region in state.RegionsById.Values) {
                data.regions.Add(new RegionSaveData {
                    id = region.Id.Value,
                    displayName = region.DisplayName,
                    bounds = ToSaveData(region.Bounds)
                });
            }

            foreach (WorldSceneState scene in state.ScenesById.Values) {
                data.scenes.Add(new WorldSceneSaveData {
                    id = scene.Id.Value,
                    regionId = scene.RegionId.Value,
                    displayName = scene.DisplayName
                });
            }

            foreach (PlaceState place in state.PlacesById.Values) {
                data.places.Add(new PlaceSaveData {
                    id = place.Id.Value,
                    regionId = place.RegionId.Value,
                    sceneId = place.SceneId.Value,
                    displayName = place.DisplayName,
                    kind = place.Kind.ToString()
                });
            }

            foreach (TerritoryChunkState territory in state.TerritoryChunksById.Values) {
                data.territoryChunks.Add(ToSaveData(territory));
            }

            foreach (CitizenState citizen in state.CitizensById.Values) {
                data.citizens.Add(new CitizenSaveData {
                    id = citizen.Id.Value,
                    displayName = citizen.DisplayName,
                    homeBuildingId = citizen.HomeBuildingId.Value,
                    workplaceBuildingId = citizen.WorkplaceBuildingId.Value,
                    currentRegionId = citizen.CurrentRegionId.Value,
                    currentSceneId = citizen.CurrentSceneId.Value,
                    currentPlaceId = citizen.CurrentPlaceId.Value,
                    currentCoord = ToSaveData(citizen.CurrentCoord),
                    activity = citizen.Activity.ToString(),
                    scheduleStage = citizen.ScheduleStage,
                    routineId = citizen.Routine.RoutineId,
                    activeRoutineStepId = citizen.Routine.ActiveStepId,
                    currentIntent = citizen.Routine.CurrentIntent,
                    lastRoutineAbsoluteMinute = citizen.Routine.LastProcessedAbsoluteMinute
                });
            }

            foreach (CitizenRegistrationState registration in state.CitizenRegistrations) {
                data.citizenRegistrations.Add(ToSaveData(registration));
            }

            foreach (CitizenGoalState goal in state.CitizenGoals) {
                data.citizenGoals.Add(ToSaveData(goal));
            }

            foreach (RoleAssignmentState assignment in state.RoleAssignments) {
                data.roleAssignments.Add(ToSaveData(assignment));
            }

            foreach (PlotState plot in state.PlotsById.Values) {
                data.plots.Add(new PlotSaveData {
                    id = plot.Id.Value,
                    regionId = plot.RegionId.Value,
                    displayName = plot.DisplayName,
                    bounds = ToSaveData(plot.Bounds),
                    ownerCitizenId = plot.OwnerCitizenId.Value,
                    accessRule = plot.AccessRule.ToString()
                });
            }

            foreach (BuildingState building in state.BuildingsById.Values) {
                data.buildings.Add(new BuildingSaveData {
                    id = building.Id.Value,
                    regionId = building.RegionId.Value,
                    plotId = building.PlotId.Value,
                    exteriorSceneId = building.ExteriorSceneId.Value,
                    interiorSceneId = building.InteriorSceneId.Value,
                    interiorPlaceId = building.InteriorPlaceId.Value,
                    displayName = building.DisplayName,
                    ownerCitizenId = building.OwnerCitizenId.Value,
                    kind = building.Kind.ToString(),
                    entranceCoord = ToSaveData(building.EntranceCoord)
                });
            }

            foreach (HistoryEvent historyEvent in state.RecentHistory) {
                data.recentHistory.Add(ToSaveData(historyEvent));
            }

            foreach (HistoryEvent historyEvent in state.HistoryArchive.Events) {
                data.archivedHistory.Add(ToSaveData(historyEvent));
            }

            return data;
        }

        public static WorldState ToRuntime(WorldSaveData data)
        {
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
            }

            var archive = new InMemoryHistoryArchive();
            if (data.archivedHistory != null) {
                foreach (HistoryEventSaveData history in data.archivedHistory) {
                    archive.Add(ToRuntime(history));
                }
            }

            var state = new WorldState(data.schemaVersion, data.worldSeed, ToRuntime(data.currentTime), Id(data.currentSceneId), archive);

            foreach (RegionSaveData region in data.regions) {
                state.AddRegion(new RegionState(Id(region.id), region.displayName, ToRuntime(region.bounds)));
            }

            foreach (WorldSceneSaveData scene in data.scenes) {
                state.AddScene(new WorldSceneState(Id(scene.id), Id(scene.regionId), scene.displayName));
            }

            foreach (PlaceSaveData place in data.places) {
                state.AddPlace(new PlaceState(
                    Id(place.id),
                    Id(place.regionId),
                    Id(place.sceneId),
                    place.displayName,
                    Enum.Parse<PlaceKind>(place.kind)));
            }

            if (data.territoryChunks != null) {
                foreach (TerritoryChunkSaveData territory in data.territoryChunks) {
                    state.AddTerritoryChunk(ToRuntime(territory));
                }
            }

            foreach (CitizenSaveData citizen in data.citizens) {
                state.AddCitizen(new CitizenState(
                    Id(citizen.id),
                    citizen.displayName,
                    Id(citizen.homeBuildingId),
                    Id(citizen.workplaceBuildingId),
                    Id(citizen.currentRegionId),
                    Id(citizen.currentSceneId),
                    Id(citizen.currentPlaceId),
                    ToRuntime(citizen.currentCoord),
                    Enum.Parse<CitizenActivityState>(citizen.activity),
                    citizen.scheduleStage,
                    new CitizenRoutineState(
                        citizen.routineId ?? string.Empty,
                        citizen.activeRoutineStepId ?? string.Empty,
                        citizen.currentIntent ?? string.Empty,
                        citizen.lastRoutineAbsoluteMinute == 0 && string.IsNullOrEmpty(citizen.activeRoutineStepId)
                            ? -1
                            : citizen.lastRoutineAbsoluteMinute)));
            }

            if (data.citizenRegistrations != null) {
                foreach (CitizenRegistrationSaveData registration in data.citizenRegistrations) {
                    state.AddCitizenRegistration(ToRuntime(registration));
                }
            }

            if (data.citizenGoals != null) {
                foreach (CitizenGoalSaveData goal in data.citizenGoals) {
                    state.AddCitizenGoal(ToRuntime(goal));
                }
            }

            if (data.roleAssignments != null) {
                foreach (RoleAssignmentSaveData assignment in data.roleAssignments) {
                    state.AddRoleAssignment(ToRuntime(assignment));
                }
            }

            foreach (PlotSaveData plot in data.plots) {
                state.AddPlot(new PlotState(
                    Id(plot.id),
                    Id(plot.regionId),
                    plot.displayName,
                    ToRuntime(plot.bounds),
                    Id(plot.ownerCitizenId),
                    Enum.Parse<AccessRule>(plot.accessRule)));
            }

            foreach (BuildingSaveData building in data.buildings) {
                state.AddBuilding(new BuildingState(
                    Id(building.id),
                    Id(building.regionId),
                    Id(building.plotId),
                    Id(building.exteriorSceneId),
                    Id(building.interiorSceneId),
                    Id(building.interiorPlaceId),
                    building.displayName,
                    Id(building.ownerCitizenId),
                    Enum.Parse<PlaceKind>(building.kind),
                    ToRuntime(building.entranceCoord)));
            }

            if (data.recentHistory != null) {
                foreach (HistoryEventSaveData history in data.recentHistory) {
                    state.AddHistoryEvent(ToRuntime(history));
                }
            }

            return state;
        }

        private static WorldEntityId Id(string value)
        {
            return new WorldEntityId(value);
        }

        private static GridSaveData ToSaveData(GridCoord coord)
        {
            return new GridSaveData { x = coord.X, y = coord.Y };
        }

        private static GridCoord ToRuntime(GridSaveData data)
        {
            return new GridCoord(data.x, data.y);
        }

        private static GridBoundsSaveData ToSaveData(GridBounds bounds)
        {
            return new GridBoundsSaveData {
                min = ToSaveData(bounds.Min),
                width = bounds.Width,
                height = bounds.Height
            };
        }

        private static GridBounds ToRuntime(GridBoundsSaveData data)
        {
            return new GridBounds(ToRuntime(data.min), data.width, data.height);
        }

        private static DateTimeSaveData ToSaveData(GameDateTime dateTime)
        {
            return new DateTimeSaveData {
                year = dateTime.Date.Year,
                month = dateTime.Date.Month,
                day = dateTime.Date.Day,
                absoluteDay = dateTime.Date.AbsoluteDay,
                hour = dateTime.Time.Hour,
                minute = dateTime.Time.Minute
            };
        }

        private static GameDateTime ToRuntime(DateTimeSaveData data)
        {
            return new GameDateTime(
                new GameDate(data.year, data.month, data.day, data.absoluteDay),
                new TimeOfDay(data.hour, data.minute));
        }

        private static HistoryEventSaveData ToSaveData(HistoryEvent historyEvent)
        {
            var historyData = new HistoryEventSaveData {
                id = historyEvent.Id.Value,
                timestamp = ToSaveData(historyEvent.Timestamp),
                kind = historyEvent.Kind.ToString(),
                description = historyEvent.Description
            };

            foreach (WorldEntityId actorId in historyEvent.ActorIds) {
                historyData.actorIds.Add(actorId.Value);
            }

            foreach (WorldEntityId placeId in historyEvent.PlaceIds) {
                historyData.placeIds.Add(placeId.Value);
            }

            return historyData;
        }

        private static CitizenGoalSaveData ToSaveData(CitizenGoalState goal)
        {
            return new CitizenGoalSaveData {
                id = goal.Id.Value,
                citizenId = goal.CitizenId.Value,
                kind = goal.Kind.ToString(),
                targetPlaceId = goal.TargetPlaceId.Value,
                targetCoord = ToSaveData(goal.TargetCoord),
                activity = goal.Activity.ToString(),
                urgency = goal.Urgency,
                reason = goal.Reason,
                createdAt = ToSaveData(goal.CreatedAt),
                expiresAt = ToSaveData(goal.ExpiresAt),
                status = goal.Status.ToString()
            };
        }

        private static CitizenRegistrationSaveData ToSaveData(CitizenRegistrationState registration)
        {
            return new CitizenRegistrationSaveData {
                citizenId = registration.CitizenId.Value,
                registeredAt = ToSaveData(registration.RegisteredAt),
                startingResidencePlaceId = registration.StartingResidencePlaceId.Value
            };
        }

        private static CitizenRegistrationState ToRuntime(CitizenRegistrationSaveData registration)
        {
            return new CitizenRegistrationState(
                Id(registration.citizenId),
                ToRuntime(registration.registeredAt),
                Id(registration.startingResidencePlaceId));
        }

        private static TerritoryChunkSaveData ToSaveData(TerritoryChunkState territory)
        {
            return new TerritoryChunkSaveData {
                id = territory.Id.Value,
                regionId = territory.RegionId.Value,
                chunkX = territory.Coord.X,
                chunkY = territory.Coord.Y,
                displayName = territory.DisplayName,
                biome = territory.Biome.ToString(),
                claimStatus = territory.ClaimStatus.ToString(),
                claimOwnerId = territory.ClaimOwnerId.Value,
                settlementId = territory.SettlementId.Value,
                jurisdictionId = territory.JurisdictionId.Value,
                isBuildable = territory.IsBuildable,
                isDiscovered = territory.IsDiscovered
            };
        }

        private static RoleAssignmentSaveData ToSaveData(RoleAssignmentState assignment)
        {
            return new RoleAssignmentSaveData {
                id = assignment.Id.Value,
                citizenId = assignment.CitizenId.Value,
                roleId = assignment.RoleId,
                workplacePlaceId = assignment.WorkplacePlaceId.Value,
                isActive = assignment.IsActive
            };
        }

        private static RoleAssignmentState ToRuntime(RoleAssignmentSaveData assignment)
        {
            return new RoleAssignmentState(
                Id(assignment.id),
                Id(assignment.citizenId),
                assignment.roleId,
                Id(assignment.workplacePlaceId),
                assignment.isActive);
        }

        private static TerritoryChunkState ToRuntime(TerritoryChunkSaveData territory)
        {
            return new TerritoryChunkState(
                Id(territory.id),
                Id(territory.regionId),
                new GridChunkCoord(territory.chunkX, territory.chunkY),
                territory.displayName,
                Enum.Parse<TerritoryBiome>(territory.biome),
                Enum.Parse<TerritoryClaimStatus>(territory.claimStatus),
                OptionalId(territory.claimOwnerId),
                OptionalId(territory.settlementId),
                OptionalId(territory.jurisdictionId),
                territory.isBuildable,
                territory.isDiscovered);
        }

        private static CitizenGoalState ToRuntime(CitizenGoalSaveData goal)
        {
            var definition = new CitizenGoalDefinition(
                Enum.Parse<CitizenGoalKind>(goal.kind),
                Id(goal.targetPlaceId),
                ToRuntime(goal.targetCoord),
                Enum.Parse<CitizenActivityState>(goal.activity),
                goal.urgency,
                goal.reason);

            return new CitizenGoalState(
                Id(goal.id),
                Id(goal.citizenId),
                definition,
                ToRuntime(goal.createdAt),
                ToRuntime(goal.expiresAt),
                Enum.Parse<CitizenGoalStatus>(goal.status));
        }

        private static HistoryEvent ToRuntime(HistoryEventSaveData history)
        {
            var actorIds = new WorldEntityId[history.actorIds.Count];
            for (int i = 0; i < actorIds.Length; i++) {
                actorIds[i] = Id(history.actorIds[i]);
            }

            var placeIds = new WorldEntityId[history.placeIds.Count];
            for (int i = 0; i < placeIds.Length; i++) {
                placeIds[i] = Id(history.placeIds[i]);
            }

            return new HistoryEvent(
                Id(history.id),
                ToRuntime(history.timestamp),
                Enum.Parse<HistoryEventKind>(history.kind),
                history.description,
                actorIds,
                placeIds);
        }

        private static WorldEntityId OptionalId(string value)
        {
            return string.IsNullOrEmpty(value)
                ? default
                : Id(value);
        }
    }
}
