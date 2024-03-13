using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DeiveEx.Utilities
{
    [RequireComponent(typeof(RectTransform))]
    public class ForceUILayoutUpdate : MonoBehaviour
    {
        #region Fields

        [SerializeField] private int _framesToWait = 1;

        #endregion

        #region Unity Events

        private void OnEnable()
        {
            StartCoroutine(ForceLayoutUpdateRoutine());
        }

        #endregion

        #region Private Methods

        private IEnumerator ForceLayoutUpdateRoutine()
        {
            for (int i = 0; i < _framesToWait; i++)
            {
                yield return null;
            }
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }

        #endregion
    }
}
