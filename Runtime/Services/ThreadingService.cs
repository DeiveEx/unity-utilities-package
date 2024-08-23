using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DeiveEx.Utilities
{
	public class ThreadingService
	{
		private static bool _initialized;
		private static CancellationTokenSource _quitSource;
		private static SynchronizationContext _unityContext;

		public CancellationToken QuitToken => _quitSource.Token;
		public SynchronizationContext UnityContext => _unityContext;

#if UNITY_EDITOR
		[InitializeOnLoadMethod] //Automatically calls this method when Unity does a Domain Reload during edit mode
#endif
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] //Automatically calls this methods during runtime on the specified load type
		static void MainThreadInitialize()
		{
			//Whenever Unity quits or exits play mode, we call "Cancel" on the Cancellation token so Tasks can cancel themselves
			_unityContext = SynchronizationContext.Current;
			_quitSource = new CancellationTokenSource();
			
			Application.quitting += _quitSource.Cancel;
#if UNITY_EDITOR
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
		}

#if UNITY_EDITOR
		static void OnPlayModeStateChanged(PlayModeStateChange state)
		{
			if (state == PlayModeStateChange.ExitingPlayMode)
				_quitSource.Cancel();
		}
#endif
		
		public async Task DelayMilliseconds(int milliseconds, bool ignoreTimeScale = false, CancellationTokenSource cancellationTokenSource = null)
		{
			float elapsedTime = 0;
			float seconds = milliseconds / 1000f;

			while (elapsedTime < seconds)
			{
				elapsedTime += ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
				await Task.Yield();

				if (cancellationTokenSource != null &&
				    cancellationTokenSource.IsCancellationRequested)
				{
					return;
				}
			}
		}

		public async Task DelaySeconds(float seconds, bool ignoreTimeScale = false, CancellationTokenSource cancellationTokenSource = null)
		{
			await DelayMilliseconds((int) (seconds * 1000), ignoreTimeScale, cancellationTokenSource);
		}

		public async Task WaitWhile(Func<bool> condition)
		{
			while (condition())
			{
				await Task.Yield();

				if (_quitSource.IsCancellationRequested)
					return;
			}
		}
	}		
}