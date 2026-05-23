using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class SwitchSceneCommand : IWorldCommand
    {
        private readonly WorldEntityId _sceneId;
        private readonly WorldEntityId _actorId;

        public SwitchSceneCommand(WorldEntityId sceneId, WorldEntityId actorId)
        {
            _sceneId = sceneId;
            _actorId = actorId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetScene(_sceneId, out WorldSceneState scene)) {
                return WorldCommandResult.Failure($"Scene not found: {_sceneId}");
            }

            context.State.SetCurrentScene(_sceneId);

            HistoryEvent historyEvent = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.SceneChanged,
                $"Active scene changed to {scene.DisplayName}.",
                new[] { _actorId },
                new[] { _sceneId });

            return WorldCommandResult
                .Success($"Entered {scene.DisplayName}.")
                .WithChangedEntity(_sceneId)
                .WithHistoryEvent(historyEvent);
        }
    }
}
