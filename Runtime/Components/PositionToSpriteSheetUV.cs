using System;
using UnityEngine;

namespace DeiveEx.Utilities
{
    [ExecuteAlways]

    public class PositionToSpriteSheetUV : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Transform _sourcePosition;
        [SerializeField] private SpriteSheetUVController _targetUV;
        [SerializeField] private VectorAxis _rowValue = VectorAxis.X;
        [SerializeField] private float _rowMultiplier = 1;
        [SerializeField] private VectorAxis _columnValue = VectorAxis.Y;
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

            int rowIndex = Mathf.RoundToInt(GetCorrectAxis(vector, _rowValue) * _rowMultiplier);
            int columnIndex = Mathf.RoundToInt(GetCorrectAxis(vector, _columnValue) * _columnMultiplier);

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
