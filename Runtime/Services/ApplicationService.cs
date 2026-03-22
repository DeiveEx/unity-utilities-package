using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DeiveEx.Utilities
{
    public class ApplicationService
    {
        /// <summary>
        /// In the player, quits and  closes the app. In the Editor, exits play mode.
        /// </summary>
        public void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}