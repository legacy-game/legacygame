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
                    scheduleStage = citizen.ScheduleStage
                });
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
                    citizen.scheduleStage));
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
    }
}
