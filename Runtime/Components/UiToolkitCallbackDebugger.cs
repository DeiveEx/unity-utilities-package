using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace DeiveEx.Utilities
{
    public class UiToolkitCallbackDebugger : MonoBehaviour
    {
        [Flags]
        private enum CallbackFlags
        {
            None = 0,
            Everything = int.MaxValue,
            OnPointerEnter = 1,
            OnPointerLeave = 2,
            OnPointerOver = 4,
            OnPointerOut = 8,
            OnPointerMove = 16,
        }

        [SerializeField] private UIDocument _uiDocument;
        [SerializeField] private CallbackFlags _callbackFlags = (CallbackFlags) int.MaxValue;
        [SerializeField] private TrickleDown _trickleDown;

        private void OnEnable()
        {
            RegisterUnregisterCallbacksRecursive(_uiDocument.rootVisualElement, true, _callbackFlags, _trickleDown);
        }

        private void OnDisable()
        {
            RegisterUnregisterCallbacksRecursive(_uiDocument.rootVisualElement, false, _callbackFlags, _trickleDown);
        }

        private void RegisterUnregisterCallbacksRecursive(VisualElement target, bool isRegister, CallbackFlags flags, TrickleDown trickleDown)
        {
            if (flags.HasFlag(CallbackFlags.OnPointerEnter))
                RegisterUnregisterCallback<PointerEnterEvent>(target, LogEvent, isRegister, trickleDown);

            if (flags.HasFlag(CallbackFlags.OnPointerLeave))
                RegisterUnregisterCallback<PointerLeaveEvent>(target, LogEvent, isRegister, trickleDown);

            if (flags.HasFlag(CallbackFlags.OnPointerOver))
                RegisterUnregisterCallback<PointerOverEvent>(target, LogEvent, isRegister, trickleDown);

            if (flags.HasFlag(CallbackFlags.OnPointerOut))
                RegisterUnregisterCallback<PointerOutEvent>(target, LogEvent, isRegister, trickleDown);

            if (flags.HasFlag(CallbackFlags.OnPointerMove))
                RegisterUnregisterCallback<PointerMoveEvent>(target, LogEvent, isRegister, trickleDown);

            foreach (var child in target.Children())
                RegisterUnregisterCallbacksRecursive(child, isRegister, flags, trickleDown);
        }

        private void LogEvent<T>(T evt)
            where T : EventBase<T>, new()
        {
            var element = evt.currentTarget as VisualElement;
            Debug.Log($"{typeof(T).Name} triggered on {element?.name} (Phase: {evt.propagationPhase})");
        }

        private void RegisterUnregisterCallback<T>(VisualElement target, EventCallback<T> callback, bool isRegister, TrickleDown trickleDown)
            where T : EventBase<T>, new()
        {
            if (isRegister)
                target.RegisterCallback(callback, trickleDown);
            else
                target.UnregisterCallback(callback, trickleDown);
        }

        private void OnValidate()
        {
            if (!Application.isPlaying || _uiDocument == null || !_uiDocument.gameObject.activeInHierarchy || _uiDocument.rootVisualElement == null)
                return;

            RegisterUnregisterCallbacksRecursive(_uiDocument.rootVisualElement, false, CallbackFlags.Everything, TrickleDown.TrickleDown);
            RegisterUnregisterCallbacksRecursive(_uiDocument.rootVisualElement, true, _callbackFlags, _trickleDown);
        }
    }

}