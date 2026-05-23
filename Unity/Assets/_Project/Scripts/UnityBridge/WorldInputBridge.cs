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
        [SerializeField] private string _transferOwnerCitizenId = "citizen_rowan";
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

            if (keyboard.iKey.wasPressedThisFrame) {
                LastResult = WorldBootstrap.Runtime.Execute(new InspectBuildingCommand(
                    new WorldEntityId(_buildingId),
                    new WorldEntityId(_playerCitizenId)));
            }

            if (keyboard.tKey.wasPressedThisFrame) {
                LastResult = WorldBootstrap.Runtime.Execute(new AdvanceTimeCommand(10));
            }

            if (keyboard.oKey.wasPressedThisFrame) {
                LastResult = WorldBootstrap.Runtime.Execute(new TransferBuildingOwnershipCommand(
                    new WorldEntityId(_buildingId),
                    new WorldEntityId(_transferOwnerCitizenId),
                    new WorldEntityId(_playerCitizenId)));
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

            LoadUnityScene(WorldSceneIds.ExteriorUnitySceneName);
        }

        private void LoadUnityScene(string unitySceneName)
        {
#if UNITY_EDITOR
            string path = unitySceneName == WorldSceneIds.CafeInteriorUnitySceneName
                ? WorldSceneIds.CafeInteriorUnityScenePath
                : WorldSceneIds.ExteriorUnityScenePath;

            if (Application.isPlaying) {
                EditorSceneManager.LoadSceneInPlayMode(path, new LoadSceneParameters(LoadSceneMode.Single));
            } else {
                EditorSceneManager.OpenScene(path);
            }
#else
            SceneManager.LoadScene(unitySceneName);
#endif
        }
    }
}
