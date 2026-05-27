using System;
using System.Collections.Generic;
using Legacy.World;

namespace Legacy.Commands
{
    public enum SerializedWorldCommandKind
    {
        Unknown = 0,
        AdvanceTime,
        DoWorldAction,
        StartJobTask,
        SubmitMiniGameResult,
        CompleteJobTask,
        EndShift,
        SwitchScene
    }

    [Serializable]
    public sealed class WorldCommandArgumentDto
    {
        public string key;
        public string value;

        public WorldCommandArgumentDto()
        {
        }

        public WorldCommandArgumentDto(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
    }

    [Serializable]
    public sealed class WorldCommandEnvelopeDto
    {
        public const int CurrentProtocolVersion = 1;

        public int protocolVersion = CurrentProtocolVersion;
        public string commandId;
        public string clientId;
        public string actorId;
        public string kind;
        public int expectedStateRevision = -1;
        public long clientSequence;
        public List<WorldCommandArgumentDto> arguments = new();

        public void AddArgument(string key, string value)
        {
            arguments.Add(new WorldCommandArgumentDto(key, value));
        }
    }

    public readonly struct WorldCommandValidationResult
    {
        public bool IsValid { get; }
        public string Error { get; }

        private WorldCommandValidationResult(bool isValid, string error)
        {
            IsValid = isValid;
            Error = error ?? string.Empty;
        }

        public static WorldCommandValidationResult Valid()
        {
            return new WorldCommandValidationResult(true, string.Empty);
        }

        public static WorldCommandValidationResult Invalid(string error)
        {
            return new WorldCommandValidationResult(false, error);
        }
    }

    public static class WorldCommandEnvelopeValidator
    {
        public static WorldCommandValidationResult Validate(WorldCommandEnvelopeDto envelope, int currentStateRevision)
        {
            if (envelope == null) {
                return WorldCommandValidationResult.Invalid("Command envelope is missing.");
            }

            if (envelope.protocolVersion != WorldCommandEnvelopeDto.CurrentProtocolVersion) {
                return WorldCommandValidationResult.Invalid($"Unsupported command protocol version: {envelope.protocolVersion}.");
            }

            if (string.IsNullOrWhiteSpace(envelope.commandId)) {
                return WorldCommandValidationResult.Invalid("Command id is required.");
            }

            if (string.IsNullOrWhiteSpace(envelope.clientId)) {
                return WorldCommandValidationResult.Invalid("Client id is required.");
            }

            if (string.IsNullOrWhiteSpace(envelope.kind)) {
                return WorldCommandValidationResult.Invalid("Command kind is required.");
            }

            if (!Enum.TryParse(envelope.kind, true, out SerializedWorldCommandKind kind) ||
                kind == SerializedWorldCommandKind.Unknown) {
                return WorldCommandValidationResult.Invalid($"Unsupported command kind: {envelope.kind}.");
            }

            if (envelope.expectedStateRevision >= 0 && envelope.expectedStateRevision != currentStateRevision) {
                return WorldCommandValidationResult.Invalid($"Client state revision {envelope.expectedStateRevision} is stale; server is at {currentStateRevision}.");
            }

            var seenKeys = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < envelope.arguments.Count; i++) {
                WorldCommandArgumentDto argument = envelope.arguments[i];
                if (argument == null || string.IsNullOrWhiteSpace(argument.key)) {
                    return WorldCommandValidationResult.Invalid("Command argument keys must not be empty.");
                }

                if (!seenKeys.Add(argument.key)) {
                    return WorldCommandValidationResult.Invalid($"Duplicate command argument: {argument.key}.");
                }
            }

            return WorldCommandValidationResult.Valid();
        }
    }

    public static class WorldCommandEnvelopeMapper
    {
        public static bool TryCreateCommand(
            WorldCommandEnvelopeDto envelope,
            int currentStateRevision,
            out IWorldCommand command,
            out WorldCommandValidationResult validation)
        {
            command = null;
            validation = WorldCommandEnvelopeValidator.Validate(envelope, currentStateRevision);
            if (!validation.IsValid) {
                return false;
            }

            var arguments = new WorldCommandArguments(envelope);
            var actorId = OptionalId(envelope.actorId);
            var kind = (SerializedWorldCommandKind)Enum.Parse(typeof(SerializedWorldCommandKind), envelope.kind, true);
            switch (kind) {
                case SerializedWorldCommandKind.AdvanceTime:
                    if (!arguments.TryGetPositiveInt("minutes", out int minutes, out validation)) {
                        return false;
                    }

                    command = new AdvanceTimeCommand(minutes);
                    return true;

                case SerializedWorldCommandKind.DoWorldAction:
                    if (!RequireActor(actorId, out validation) ||
                        !arguments.TryGetEnum("action", out WorldActionKind action, out validation) ||
                        !arguments.TryGetId("targetPlaceId", out WorldEntityId targetPlaceId, out validation)) {
                        return false;
                    }

                    command = new DoWorldActionCommand(actorId, action, targetPlaceId, arguments.GetOptionalId("targetEntityId"));
                    return true;

                case SerializedWorldCommandKind.StartJobTask:
                    if (!arguments.TryGetId("taskId", out WorldEntityId startTaskId, out validation) ||
                        !arguments.TryGetId("shiftId", out WorldEntityId shiftId, out validation)) {
                        return false;
                    }

                    command = new StartJobTaskCommand(startTaskId, shiftId, arguments.GetOptionalId("workerCitizenId", actorId));
                    return true;

                case SerializedWorldCommandKind.SubmitMiniGameResult:
                    if (!arguments.TryGetId("taskId", out WorldEntityId submitTaskId, out validation) ||
                        !arguments.TryGetInt("score", out int score, out validation) ||
                        !arguments.TryGetPositiveInt("maxScore", out int maxScore, out validation) ||
                        !arguments.TryGetPositiveInt("durationSeconds", out int durationSeconds, out validation) ||
                        !arguments.TryGetInt("mistakes", out int mistakes, out validation)) {
                        return false;
                    }

                    command = new SubmitMiniGameResultCommand(submitTaskId, arguments.GetOptionalId("workerCitizenId", actorId), score, maxScore, durationSeconds, mistakes);
                    return true;

                case SerializedWorldCommandKind.CompleteJobTask:
                    if (!arguments.TryGetId("taskId", out WorldEntityId completeTaskId, out validation)) {
                        return false;
                    }

                    command = new CompleteJobTaskCommand(completeTaskId, arguments.GetOptionalId("workerCitizenId", actorId));
                    return true;

                case SerializedWorldCommandKind.EndShift:
                    if (!arguments.TryGetId("shiftId", out WorldEntityId endShiftId, out validation)) {
                        return false;
                    }

                    command = new EndShiftCommand(endShiftId, arguments.GetOptionalId("workerCitizenId", actorId));
                    return true;

                case SerializedWorldCommandKind.SwitchScene:
                    if (!arguments.TryGetId("sceneId", out WorldEntityId sceneId, out validation)) {
                        return false;
                    }

                    command = new SwitchSceneCommand(sceneId, arguments.GetOptionalId("actorId", actorId));
                    return true;

                default:
                    validation = WorldCommandValidationResult.Invalid($"Unsupported command kind: {envelope.kind}.");
                    return false;
            }
        }

        private static bool RequireActor(WorldEntityId actorId, out WorldCommandValidationResult validation)
        {
            if (string.IsNullOrEmpty(actorId.Value)) {
                validation = WorldCommandValidationResult.Invalid("Actor id is required for this command.");
                return false;
            }

            validation = WorldCommandValidationResult.Valid();
            return true;
        }

        private static WorldEntityId OptionalId(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? default
                : new WorldEntityId(value);
        }

        private sealed class WorldCommandArguments
        {
            private readonly WorldCommandEnvelopeDto _envelope;

            public WorldCommandArguments(WorldCommandEnvelopeDto envelope)
            {
                _envelope = envelope;
            }

            public bool TryGetId(string key, out WorldEntityId value, out WorldCommandValidationResult validation)
            {
                if (!TryGetString(key, out string raw, out validation)) {
                    value = default;
                    return false;
                }

                value = new WorldEntityId(raw);
                return true;
            }

            public WorldEntityId GetOptionalId(string key)
            {
                return TryFind(key, out string raw) && !string.IsNullOrWhiteSpace(raw)
                    ? new WorldEntityId(raw)
                    : default;
            }

            public WorldEntityId GetOptionalId(string key, WorldEntityId fallback)
            {
                WorldEntityId value = GetOptionalId(key);
                return string.IsNullOrEmpty(value.Value) ? fallback : value;
            }

            public bool TryGetPositiveInt(string key, out int value, out WorldCommandValidationResult validation)
            {
                if (!TryGetInt(key, out value, out validation)) {
                    return false;
                }

                if (value <= 0) {
                    validation = WorldCommandValidationResult.Invalid($"Command argument {key} must be positive.");
                    return false;
                }

                return true;
            }

            public bool TryGetInt(string key, out int value, out WorldCommandValidationResult validation)
            {
                if (!TryGetString(key, out string raw, out validation)) {
                    value = 0;
                    return false;
                }

                if (!int.TryParse(raw, out value)) {
                    validation = WorldCommandValidationResult.Invalid($"Command argument {key} must be an integer.");
                    return false;
                }

                validation = WorldCommandValidationResult.Valid();
                return true;
            }

            public bool TryGetEnum<TEnum>(string key, out TEnum value, out WorldCommandValidationResult validation)
                where TEnum : struct
            {
                if (!TryGetString(key, out string raw, out validation)) {
                    value = default;
                    return false;
                }

                if (!Enum.TryParse(raw, true, out value)) {
                    validation = WorldCommandValidationResult.Invalid($"Command argument {key} has unsupported value: {raw}.");
                    return false;
                }

                validation = WorldCommandValidationResult.Valid();
                return true;
            }

            private bool TryGetString(string key, out string value, out WorldCommandValidationResult validation)
            {
                if (!TryFind(key, out value) || string.IsNullOrWhiteSpace(value)) {
                    validation = WorldCommandValidationResult.Invalid($"Command argument {key} is required.");
                    return false;
                }

                validation = WorldCommandValidationResult.Valid();
                return true;
            }

            private bool TryFind(string key, out string value)
            {
                for (int i = 0; i < _envelope.arguments.Count; i++) {
                    WorldCommandArgumentDto argument = _envelope.arguments[i];
                    if (argument != null && string.Equals(argument.key, key, StringComparison.Ordinal)) {
                        value = argument.value;
                        return true;
                    }
                }

                value = null;
                return false;
            }
        }
    }
}
