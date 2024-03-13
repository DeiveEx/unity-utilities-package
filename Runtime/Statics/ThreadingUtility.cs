using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DeiveEx.Utilities
{
#if UNITY_EDITOR
	[InitializeOnLoad] //Initializes this class when Unity does a Domain Reload
#endif
	public static class ThreadingUtility
	{
		static readonly CancellationTokenSource quitSource;

		public static CancellationToken QuitToken { get; }
		public static SynchronizationContext UnityContext { get; private set; }

		static ThreadingUtility()
		{
			quitSource = new CancellationTokenSource();
			QuitToken = quitSource.Token;
		}

#if UNITY_EDITOR
		[InitializeOnLoadMethod] //Automatically calls this method when Unity does a Domain Reload during edit mode
#endif
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] //Automatically calls this methods during runtime on the specified load type
		static void MainThreadInitialize()
		{
			//Whenever Unity quits or exits play mode, we call "Cancel" on the Cancellation token so Tasks can cancel themselves
			UnityContext = SynchronizationContext.Current;
			Application.quitting += quitSource.Cancel;
#if UNITY_EDITOR
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
		}

#if UNITY_EDITOR
		static void OnPlayModeStateChanged(PlayModeStateChange state)
		{
			if (state == PlayModeStateChange.ExitingPlayMode)
				quitSource.Cancel();
		}
#endif
		
		public static async Task Delay(int milliseconds, bool ignoreTimeScale = false, CancellationTokenSource cancellationTokenSource = null)
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
	}
}