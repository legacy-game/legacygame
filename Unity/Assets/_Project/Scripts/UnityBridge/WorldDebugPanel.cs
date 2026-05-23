using System.Text;
using Legacy.History;
using Legacy.World;
using UnityEngine;

namespace Legacy.UnityBridge
{
    public sealed class WorldDebugPanel : MonoBehaviour
    {
        private readonly StringBuilder _builder = new();

        private void OnGUI()
        {
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
            AppendCitizenLocation(state, new WorldEntityId("citizen_old_mr_pell"));
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

            _builder.AppendLine($"{citizen.DisplayName}: {citizen.Activity} at {place} ({scene})");
        }
    }
}
