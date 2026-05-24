using System;
using UnityEngine;

namespace Legacy.UnityBridge
{
    public sealed class InteractableView : MonoBehaviour
    {
        private Func<string> _promptProvider;
        private Action _action;
        private int _priority;
        private float _radius = 1.5f;

        public string Prompt => _promptProvider != null ? _promptProvider() : string.Empty;
        public int Priority => _priority;
        public float Radius => _radius;

        public void Configure(Func<string> promptProvider, Action action, int priority = 0, float radius = 1.5f)
        {
            _promptProvider = promptProvider;
            _action = action;
            _priority = priority;
            _radius = radius;
        }

        public void Interact()
        {
            _action?.Invoke();
        }
    }
}
