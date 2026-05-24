using System;
using System.Collections.Generic;

namespace Legacy.World
{
    public sealed class RoutineDefinition
    {
        private readonly List<RoutineStepDefinition> _steps;

        public string Id { get; }
        public string DisplayName { get; }
        public IReadOnlyList<RoutineStepDefinition> Steps => _steps;

        public RoutineDefinition(string id, string displayName, IEnumerable<RoutineStepDefinition> steps)
        {
            if (string.IsNullOrWhiteSpace(id)) {
                throw new ArgumentException("Routine id must not be empty.", nameof(id));
            }

            Id = id;
            DisplayName = string.IsNullOrWhiteSpace(displayName) ? id : displayName;
            _steps = new List<RoutineStepDefinition>(steps ?? Array.Empty<RoutineStepDefinition>());
        }
    }
}
