#if R3_OBSERVABLES
using R3;
using UnityEngine.UIElements;

namespace DeiveEx.Utilities
{
    public static class UIToolkitR3ObservableExtensions
    {
        public static Observable<Unit> OnClickAsObservable(this Button button)
        {
            return Observable.FromEvent(
                h => button.clicked += h,
                h => button.clicked -= h);
        }

        public static Observable<ChangeEvent<T>> OnValueChangedAsObservable<T>(this INotifyValueChanged<T> element)
        {
            return Observable.FromEvent<EventCallback<ChangeEvent<T>>, ChangeEvent<T>>(
                h => evt => h(evt),
                h => element.RegisterValueChangedCallback(h),
                h => element.UnregisterValueChangedCallback(h));
        }
    }
}
#endif
