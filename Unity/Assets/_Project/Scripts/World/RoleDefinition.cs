using System;
using System.Collections.Generic;

namespace Legacy.World
{
    public sealed class RoleDefinition
    {
        private readonly List<WorldActionKind> _authorizedActions;

        public string Id { get; }
        public string DisplayName { get; }
        public bool IsPublicService { get; }
        public IReadOnlyList<WorldActionKind> AuthorizedActions => _authorizedActions;

        public RoleDefinition(string id, string displayName, bool isPublicService, IEnumerable<WorldActionKind> authorizedActions)
        {
            if (string.IsNullOrWhiteSpace(id)) {
                throw new ArgumentException("Role id must not be empty.", nameof(id));
            }

            Id = id;
            DisplayName = string.IsNullOrWhiteSpace(displayName) ? id : displayName;
            IsPublicService = isPublicService;
            _authorizedActions = new List<WorldActionKind>(authorizedActions ?? Array.Empty<WorldActionKind>());
        }

        public bool Allows(WorldActionKind action)
        {
            return _authorizedActions.Contains(action);
        }
    }
}
