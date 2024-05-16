#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
    public static partial class AsyncTriggerExtensions
    {
        // Util.
        static T GetOrAddComponent<T>(GameObject gameObject)
            where T : Component
        {
            if (!gameObject.TryGetComponent<T>(out var component))
                component = gameObject.AddComponent<T>();
            return component;
        }

        // Special for single operation.

        /// <summary>This function is called when the MonoBehaviour will be destroyed.</summary>
        public static UniTask OnDestroyAsync(this GameObject gameObject)
        {
            return gameObject.GetAsyncDestroyTrigger().OnDestroyAsync();
        }

        /// <summary>This function is called when the MonoBehaviour will be destroyed.</summary>
        public static UniTask OnDestroyAsync(this Component component)
        {
            return component.GetAsyncDestroyTrigger().OnDestroyAsync();
        }

        public static UniTask StartAsync(this GameObject gameObject)
        {
            return gameObject.GetAsyncStartTrigger().StartAsync();
        }

        public static UniTask StartAsync(this Component component)
        {
            return component.GetAsyncStartTrigger().StartAsync();
        }
    }
}

