using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace DeiveEx.Utilities
{
    [ExecuteAlways]

    public class PositionToSpriteSheetUV : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Transform _sourcePosition;
        [SerializeField] private SpriteSheetUVController _targetUV;
        
        [SerializeField] private bool _overrideRow = false;
#if ODIN_INSPECTOR
        [ShowIf("_overrideRow")]
#endif
        [SerializeField] private int _rowIndex = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Row Settings", VisibleIf = "@!_overrideRow")]
#endif
        [SerializeField] private VectorAxis _rowValue = VectorAxis.X;
#if ODIN_INSPECTOR
        [BoxGroup("Row Settings")]
#endif
        [SerializeField] private float _rowMultiplier = 1;
        
        [SerializeField] private bool _overrideColumn = false;
#if ODIN_INSPECTOR
        [ShowIf("_overrideColumn")]
#endif
        [SerializeField] private int _columnIndex = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Column Settings", VisibleIf = "@!_overrideColumn")]
#endif
        [SerializeField] private VectorAxis _columnValue = VectorAxis.Y;
#if ODIN_INSPECTOR
        [BoxGroup("Column Settings")]
#endif
        [SerializeField] private float _columnMultiplier = 1;

        private bool _isSourcePositionNull;
        private bool _isTargetUVNull;

        #endregion

        #region Unity Events

        private void OnValidate()
        {
            CheckValidity();
        }

        private void Awake()
        {
            CheckValidity();
        }

        private void Update()
        {
            if (_isSourcePositionNull || _isTargetUVNull)
                return;

            UpdateUVs();
        }

        #endregion

        #region Private Methods

        private void CheckValidity()
        {
            _isSourcePositionNull = _sourcePosition == null;
            _isTargetUVNull = _targetUV == null;
        }

        private void UpdateUVs()
        {
            Vector3 vector = _sourcePosition.localPosition;

            int rowIndex = _rowIndex;
            int columnIndex = _columnIndex;

            if (_overrideRow)
                rowIndex = Mathf.RoundToInt(GetCorrectAxis(vector, _rowValue) * _rowMultiplier);

            if (_overrideColumn)
                columnIndex = Mathf.RoundToInt(GetCorrectAxis(vector, _columnValue) * _columnMultiplier);

            _targetUV.SetSpriteIndex(rowIndex, columnIndex);
        }

        private float GetCorrectAxis(Vector3 source, VectorAxis axis)
        {
            switch (axis)
            {
                case VectorAxis.X:
                    return source.x;
                case VectorAxis.Y:
                    return source.y;
                case VectorAxis.Z:
                    return source.z;
                default:
                    throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
            }
        }

        #endregion
    }
}
