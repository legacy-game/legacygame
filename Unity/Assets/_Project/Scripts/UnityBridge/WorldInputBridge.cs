using System;
using Legacy.Commands;
using Legacy.Save;
using Legacy.World;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace Legacy.UnityBridge
{
    public sealed class WorldInputBridge : MonoBehaviour
    {
        [SerializeField] private string _playerCitizenId = "citizen_noaharan";
        [SerializeField] private string _buildingId = "building_linden_cafe";
        [SerializeField] private string _plotId = "plot_linden_14";
        [SerializeField] private string _placeId = "place_linden_cafe_interior";
        [SerializeField] private string _transferOwnerCitizenId = "citizen_rowan";
        [SerializeField] private string _goalCitizenId = "citizen_old_mr_pell";
        [SerializeField] private string _goalTargetPlaceId = "place_pell_pharmacy_interior";
        [SerializeField] private string _debugTerritoryId = "territory_aldwich_south_grassland";
        [SerializeField] private string _debugSaveSlot = "world-memory-smoke";
        [SerializeField] private PropertyInfoPanel _infoPanel;

        private bool _isSavingOrLoading;

        public WorldCommandResult LastResult { get; private set; }

        public void SetInfoPanel(PropertyInfoPanel infoPanel)
        {
            _infoPanel = infoPanel;
        }

        private void Update()
        {
            if (WorldBootstrap.Runtime == null) {
                return;
            }

            Keyboard keyboard = Keyboard.current;
            if (keyboard == null) {
                return;
            }

            if (keyboard.f1Key.wasPressedThisFrame) {
                WorldDebugPanel.Toggle();
            }

            if (!WorldDebugPanel.IsVisible) {
                return;
            }

            if (keyboard.iKey.wasPressedThisFrame) {
                ExecuteAndShow(new InspectBuildingCommand(
                    new WorldEntityId(_buildingId),
                    new WorldEntityId(_playerCitizenId)));
            }

            if (keyboard.pKey.wasPressedThisFrame) {
                ExecuteAndShow(new InspectPlotCommand(
                    new WorldEntityId(_plotId),
                    new WorldEntityId(_playerCitizenId)));
            }

            if (keyboard.vKey.wasPressedThisFrame) {
                ExecuteAndShow(new InspectPlaceCommand(
                    new WorldEntityId(_placeId),
                    new WorldEntityId(_playerCitizenId)));
            }

            if (keyboard.tKey.wasPressedThisFrame) {
                ExecuteAndShow(new AdvanceTimeCommand(10));
            }

            if (keyboard.oKey.wasPressedThisFrame) {
                ExecuteAndShow(new TransferBuildingOwnershipCommand(
                    new WorldEntityId(_buildingId),
                    new WorldEntityId(_transferOwnerCitizenId),
                    new WorldEntityId(_playerCitizenId)));
            }

            if (keyboard.gKey.wasPressedThisFrame) {
                AddPellGoal();
            }

            if (keyboard.yKey.wasPressedThisFrame) {
                ExecuteAndShow(new InspectTerritoryCommand(
                    new WorldEntityId(_debugTerritoryId),
                    new WorldEntityId(_playerCitizenId)));
            }

            if (keyboard.uKey.wasPressedThisFrame) {
                ExecuteAndShow(new ClaimTerritoryCommand(
                    new WorldEntityId(_debugTerritoryId),
                    new WorldEntityId(_playerCitizenId),
                    new WorldEntityId(_playerCitizenId)));
            }

            if (keyboard.bKey.wasPressedThisFrame) {
                ExecuteAndShow(new DoWorldActionCommand(
                    new WorldEntityId("citizen_rowan"),
                    WorldActionKind.ServeCustomer,
                    new WorldEntityId("place_linden_cafe_interior")));
            }

            if (keyboard.nKey.wasPressedThisFrame) {
                ExecuteAndShow(new DoWorldActionCommand(
                    new WorldEntityId(_playerCitizenId),
                    WorldActionKind.PatrolDistrict,
                    new WorldEntityId("place_linden_cafe_interior")));
            }

            if (keyboard.sKey.wasPressedThisFrame) {
                SaveWorldAsync();
            }

            if (keyboard.lKey.wasPressedThisFrame) {
                LoadWorldAsync();
            }

            if (keyboard.digit1Key.wasPressedThisFrame) {
                SwitchScene(WorldSceneIds.ExteriorSceneId, WorldSceneIds.ExteriorUnitySceneName);
            }

            if (keyboard.digit2Key.wasPressedThisFrame) {
                SwitchScene(WorldSceneIds.CafeInteriorSceneId, WorldSceneIds.CafeInteriorUnitySceneName);
            }

            if (keyboard.digit3Key.wasPressedThisFrame) {
                SwitchScene(WorldSceneIds.PharmacyInteriorSceneId, WorldSceneIds.PharmacyInteriorUnitySceneName);
            }
        }

        private void ExecuteAndShow(IWorldCommand command)
        {
            LastResult = WorldBootstrap.Runtime.Execute(command);
            ShowInfo(LastResult.Message);
        }

        private void AddPellGoal()
        {
            WorldState state = WorldBootstrap.Runtime.State;
            var targetPlaceId = new WorldEntityId(_goalTargetPlaceId);
            GridCoord targetCoord = new GridCoord(2, 2);
            if (state.TryGetBuilding(new WorldEntityId("building_pell_pharmacy"), out BuildingState pharmacy)) {
                targetCoord = pharmacy.EntranceCoord;
            }

            var goal = new CitizenGoalDefinition(
                CitizenGoalKind.TravelToPlace,
                targetPlaceId,
                targetCoord,
                CitizenActivityState.Visiting,
                100,
                "Go to Pell Pharmacy");
            var goalId = new WorldEntityId($"goal_pell_pharmacy_{state.CitizenGoals.Count + 1:000}");

            ExecuteAndShow(new AddCitizenGoalCommand(
                goalId,
                new WorldEntityId(_goalCitizenId),
                goal,
                state.CurrentTime.AddMinutes(180),
                new WorldEntityId(_playerCitizenId)));
        }

        private async void SaveWorldAsync()
        {
            if (_isSavingOrLoading || WorldBootstrap.SaveManager == null) {
                return;
            }

            _isSavingOrLoading = true;
            try {
                SaveResult result = await WorldBootstrap.SaveManager.SaveAsync(_debugSaveSlot, WorldBootstrap.Runtime.State);
                ShowInfo(result == SaveResult.Ok ? $"Saved slot: {_debugSaveSlot}" : "Save failed.");
            } catch (Exception ex) {
                Debug.LogException(ex);
                ShowInfo("Save failed. See Console.");
            } finally {
                _isSavingOrLoading = false;
            }
        }

        private async void LoadWorldAsync()
        {
            if (_isSavingOrLoading || WorldBootstrap.SaveManager == null) {
                return;
            }

            _isSavingOrLoading = true;
            try {
                WorldState loadedState = await WorldBootstrap.SaveManager.LoadAsync(_debugSaveSlot);
                WorldBootstrap.Runtime.ReplaceState(loadedState);
                ShowInfo($"Loaded slot: {_debugSaveSlot}");
                LoadUnitySceneForWorldScene(loadedState.CurrentSceneId);
            } catch (Exception ex) {
                Debug.LogException(ex);
                ShowInfo("Load failed. Save first with S.");
            } finally {
                _isSavingOrLoading = false;
            }
        }

        private void ShowInfo(string message)
        {
            if (_infoPanel != null) {
                _infoPanel.Show(message);
            }
        }

        private void SwitchScene(string sceneId, string unitySceneName)
        {
            LastResult = WorldBootstrap.Runtime.Execute(new SwitchSceneCommand(
                new WorldEntityId(sceneId),
                new WorldEntityId(_playerCitizenId)));

            if (LastResult.Succeeded) {
                LoadUnityScene(unitySceneName);
            }
        }

        private void LoadUnitySceneForWorldScene(WorldEntityId sceneId)
        {
            if (sceneId.Value == WorldSceneIds.CafeInteriorSceneId) {
                LoadUnityScene(WorldSceneIds.CafeInteriorUnitySceneName);
                return;
            }

            if (sceneId.Value == WorldSceneIds.PharmacyInteriorSceneId) {
                LoadUnityScene(WorldSceneIds.PharmacyInteriorUnitySceneName);
                return;
            }

            LoadUnityScene(WorldSceneIds.ExteriorUnitySceneName);
        }

        private void LoadUnityScene(string unitySceneName)
        {
#if UNITY_EDITOR
            string path = GetUnityScenePath(unitySceneName);

            if (Application.isPlaying) {
                EditorSceneManager.LoadSceneInPlayMode(path, new LoadSceneParameters(LoadSceneMode.Single));
            } else {
                EditorSceneManager.OpenScene(path);
            }
#else
            SceneManager.LoadScene(unitySceneName);
#endif
        }

        private static string GetUnityScenePath(string unitySceneName)
        {
            if (unitySceneName == WorldSceneIds.CafeInteriorUnitySceneName) {
                return WorldSceneIds.CafeInteriorUnityScenePath;
            }

            if (unitySceneName == WorldSceneIds.PharmacyInteriorUnitySceneName) {
                return WorldSceneIds.PharmacyInteriorUnityScenePath;
            }

            return WorldSceneIds.ExteriorUnityScenePath;
        }
    }
}
