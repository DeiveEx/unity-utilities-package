using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace DeiveEx.Utilities
{
    [ExecuteAlways]

    public class SpriteSheetUVController : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Renderer _targetRenderer;
        [Min(0)] [SerializeField] private int _materialIndex;
        [Min(1)] [SerializeField] private int _rows = 1;
        [Min(1)] [SerializeField] private int _columns = 1;
#if ODIN_INSPECTOR
        [Title("Debug")]
        [PropertyRange(0, "@_rows - 1")]
#endif
        [SerializeField]
        private int _rowIndex;
#if ODIN_INSPECTOR
        [PropertyRange(0, "@_columns - 1")]
#endif
        [SerializeField]
        private int _columIndex;

        #endregion

        #region Unity Events

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_targetRenderer == null)
                return;

            SetSpriteIndex(_rowIndex, _columIndex);
        }
#endif

        #endregion

        #region Public Methods

        public void SetSpriteIndex(int spriteRow, int spriteColumn)
        {
            _rowIndex = spriteRow;
            _columIndex = spriteColumn;

            var offset = new Vector2()
            {
                x = _columIndex * (1f / _columns),
                y = _rowIndex * (1f / _rows),
            };

            var scale = new Vector2()
            {
                x = 1f / _columns,
                y = 1f / _rows,
            };

#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                //If we try to change the material before the scene is ready, it'll think we're trying to edit the prefab
                if (_targetRenderer.gameObject.scene.IsValid())
                    _targetRenderer.materials = ChangeMaterial(_targetRenderer.materials, offset, scale);
            }
            else
            {
                _targetRenderer.sharedMaterials = ChangeMaterial(_targetRenderer.sharedMaterials, offset, scale);
            }
#else
        _targetRenderer.materials = ChangeMaterial(_targetRenderer.materials, offset, scale);
#endif
        }

        public void SetSpriteRowIndex(int rowIndex)
        {
            SetSpriteIndex(rowIndex, _columIndex);
        }

        public void SetSpriteColumnIndex(int columnIndex)
        {
            SetSpriteIndex(_rowIndex, columnIndex);
        }

        private Material[] ChangeMaterial(Material[] materials, Vector2 offset, Vector2 scale)
        {
            materials[_materialIndex].mainTextureOffset = offset;
            materials[_materialIndex].mainTextureScale = scale;

            return materials;
        }

        #endregion
    }
}
