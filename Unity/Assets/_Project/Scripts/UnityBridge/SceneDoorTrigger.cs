using Legacy.Commands;
using Legacy.World;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace Legacy.UnityBridge
{
    [RequireComponent(typeof(BoxCollider2D))]
    public sealed class SceneDoorTrigger : MonoBehaviour
    {
        [SerializeField] private string _targetWorldSceneId;
        [SerializeField] private string _targetUnitySceneName;
        [SerializeField] private string _targetUnityScenePath;
        [SerializeField] private string _actorCitizenId = "citizen_noaharan";
        [SerializeField] private string _prompt = "Press E";
        [SerializeField] private PropertyInfoPanel _infoPanel;

        public void Configure(
            string targetWorldSceneId,
            string targetUnitySceneName,
            string targetUnityScenePath,
            string prompt,
            PropertyInfoPanel infoPanel)
        {
            _targetWorldSceneId = targetWorldSceneId;
            _targetUnitySceneName = targetUnitySceneName;
            _targetUnityScenePath = targetUnityScenePath;
            _prompt = prompt;
            _infoPanel = infoPanel;
        }

        public string GetPrompt()
        {
            return _prompt;
        }

        public void Interact()
        {
            if (WorldBootstrap.Runtime == null) {
                return;
            }

            WorldCommandResult result = WorldBootstrap.Runtime.Execute(new SwitchSceneCommand(
                new WorldEntityId(_targetWorldSceneId),
                new WorldEntityId(_actorCitizenId)));
            if (!result.Succeeded) {
                ShowMessage(result.Message);
                return;
            }

            LoadTargetScene();
        }

        private void Reset()
        {
            BoxCollider2D trigger = GetComponent<BoxCollider2D>();
            trigger.isTrigger = true;
        }

        private void Awake()
        {
            BoxCollider2D trigger = GetComponent<BoxCollider2D>();
            if (trigger != null) {
                trigger.isTrigger = true;
            }

            InteractableView interactable = GetComponent<InteractableView>();
            if (interactable == null) {
                interactable = gameObject.AddComponent<InteractableView>();
            }

            interactable.Configure(GetPrompt, Interact, 100, 2.2f);
        }

        private void ShowMessage(string message)
        {
            if (_infoPanel != null) {
                _infoPanel.Show(message);
            }
        }

        private void LoadTargetScene()
        {
#if UNITY_EDITOR
            if (Application.isPlaying) {
                EditorSceneManager.LoadSceneInPlayMode(_targetUnityScenePath, new LoadSceneParameters(LoadSceneMode.Single));
                return;
            }
#endif
            SceneManager.LoadScene(_targetUnitySceneName);
        }
    }
}
