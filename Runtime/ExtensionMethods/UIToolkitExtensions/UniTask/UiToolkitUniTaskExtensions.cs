#if UNITASK
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;

namespace DeiveEx.Utilities
{
    public static class UiToolkitUniTaskExtensions
    {
        public static async UniTask WaitForEventCallback<T>(this VisualElement element, CancellationToken cancellationToken = default)
            where T : EventBase<T>, new()
        {
            // WaitWhile/WaitUntil checks the condition once per frame, so callbacks that needs to be executed on the
            // same frame can't use that. To convert a callback to a UniTask that can be completed on the same frame,
            // we need to use a UniTaskCompletionSource
            var completionSource = new UniTaskCompletionSource();
            
            //Wait for the callback or cancellation (will throw an exception if canceled)
            try
            {
                element.RegisterCallback<T>(IsCompleted);
                await completionSource.Task.AttachExternalCancellation(cancellationToken);
            }
            finally
            {
                element?.UnregisterCallback<T>(IsCompleted);
            }
            
            return;
            
            void IsCompleted(T _) => completionSource.TrySetResult();
        }
    }
}
#endif
