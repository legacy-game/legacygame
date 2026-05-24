using System.Text;
using Legacy.History;
using Legacy.World;
using UnityEngine;

namespace Legacy.UnityBridge
{
    public sealed class WorldDebugPanel : MonoBehaviour
    {
        public static bool IsVisible { get; private set; }

        private readonly StringBuilder _builder = new();

        public static void Toggle()
        {
            IsVisible = !IsVisible;
        }

        private void OnGUI()
        {
            if (!IsVisible) {
                return;
            }

            if (WorldBootstrap.Runtime == null) {
                GUILayout.Label("World runtime not initialized.");
                return;
            }

            WorldState state = WorldBootstrap.Runtime.State;
            _builder.Clear();
            _builder.AppendLine($"Time: {state.CurrentTime}");
            _builder.AppendLine($"Regions: {state.RegionsById.Count}");
            _builder.AppendLine($"Citizens: {state.CitizensById.Count}");
            _builder.AppendLine($"Plots: {state.PlotsById.Count}");
            _builder.AppendLine($"Buildings: {state.BuildingsById.Count}");
            _builder.AppendLine($"Buildings in active scene: {state.GetBuildingIdsInScene(state.CurrentSceneId).Count}");
            _builder.AppendLine($"Citizens in active scene: {state.GetCitizenIdsInScene(state.CurrentSceneId).Count}");
            _builder.AppendLine($"Cafe history events: {state.GetHistoryForPlace(new WorldEntityId("building_linden_cafe")).Count}");
            _builder.AppendLine($"Noaharan history events: {state.GetHistoryForActor(new WorldEntityId("citizen_noaharan")).Count}");
            _builder.AppendLine($"Ownership transfers: {state.GetHistoryByKind(HistoryEventKind.BuildingOwnershipTransferred).Count}");
            _builder.AppendLine($"World actions: {state.GetHistoryByKind(HistoryEventKind.WorldActionPerformed).Count}");
            AppendCitizenLocation(state, new WorldEntityId("citizen_old_mr_pell"));
            AppendCitizenGoals(state);
            AppendTerritory(state);
            _builder.AppendLine("Recent history:");

            foreach (HistoryEvent historyEvent in HistoryQuery.Last(state.RecentHistory, 5)) {
                _builder.AppendLine($"- {historyEvent.Timestamp}: {historyEvent.Description}");
            }

            GUILayout.Box(_builder.ToString(), GUILayout.Width(420));
        }

        private void AppendCitizenLocation(WorldState state, WorldEntityId citizenId)
        {
            if (!state.TryGetCitizen(citizenId, out CitizenState citizen)) {
                return;
            }

            string scene = state.TryGetScene(citizen.CurrentSceneId, out WorldSceneState sceneState)
                ? sceneState.DisplayName
                : citizen.CurrentSceneId.ToString();

            string place = state.TryGetPlace(citizen.CurrentPlaceId, out PlaceState placeState)
                ? placeState.DisplayName
                : citizen.CurrentPlaceId.ToString();

            string intent = string.IsNullOrWhiteSpace(citizen.Routine.CurrentIntent)
                ? "No routine intent"
                : citizen.Routine.CurrentIntent;
            _builder.AppendLine($"{citizen.DisplayName}: {citizen.Activity} at {place} ({scene}) - {intent}");
        }

        private void AppendCitizenGoals(WorldState state)
        {
            _builder.AppendLine($"Citizen goals: {state.CitizenGoals.Count}");
            int visibleCount = Mathf.Min(state.CitizenGoals.Count, 4);
            for (int i = 0; i < visibleCount; i++) {
                CitizenGoalState goal = state.CitizenGoals[i];
                string citizenName = state.TryGetCitizen(goal.CitizenId, out CitizenState citizen)
                    ? citizen.DisplayName
                    : goal.CitizenId.ToString();
                string targetName = state.TryGetPlace(goal.TargetPlaceId, out PlaceState place)
                    ? place.DisplayName
                    : goal.TargetPlaceId.ToString();

                _builder.AppendLine($"- {goal.Status}: {citizenName} -> {targetName} ({goal.Reason})");
            }
        }

        private void AppendTerritory(WorldState state)
        {
            int claimed = 0;
            int unclaimed = 0;
            foreach (TerritoryChunkState territory in state.TerritoryChunksById.Values) {
                if (territory.ClaimStatus == TerritoryClaimStatus.Unclaimed) {
                    unclaimed++;
                } else {
                    claimed++;
                }
            }

            _builder.AppendLine($"Territory chunks: {state.TerritoryChunksById.Count} (claimed {claimed}, unclaimed {unclaimed})");
            var grasslandId = new WorldEntityId("territory_aldwich_south_grassland");
            if (state.TryGetTerritoryChunk(grasslandId, out TerritoryChunkState grassland)) {
                _builder.AppendLine($"{grassland.DisplayName}: {grassland.Biome} / {grassland.ClaimStatus} / Buildable: {grassland.IsBuildable}");
            }

            _builder.AppendLine("Y = inspect territory | U = claim territory");
            _builder.AppendLine("B = Rowan serve customer | N = Noaharan patrol fail");
        }
    }
}
