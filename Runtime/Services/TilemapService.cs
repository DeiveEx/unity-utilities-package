using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DeiveEx.Utilities
{
    public static partial class UtilityServices
    {
        public static class TilemapService
        {
            public static void FillArea(UnityEngine.Tilemaps.Tilemap tilemap, IEnumerable<Vector3Int> positions, TileBase tile)
            {
                foreach (var position in positions)
                {
                    tilemap.SetTile(position, tile);
                }
            }
            
            public static bool IsAreaFree(UnityEngine.Tilemaps.Tilemap tilemap, Vector3Int start, int regionSize)
            {
                for (int x = start.x; x < start.x + regionSize; x++)
                {
                    for (int y = start.y; y < start.y + regionSize; y++)
                    {
                        Vector3Int pos = new Vector3Int(x, y, 0);

                        if (tilemap.HasTile(pos))
                            return false;
                    }
                }
                
                return true;
            }
            
            public static List<Vector3Int> GetSquare(Vector3Int start, int size)
            {
                var positions = new List<Vector3Int>();

                for (int x = start.x; x < start.x + size; x++)
                {
                    for (int y = start.y; y < start.y + size; y++)
                    {
                        Vector3Int pos = new Vector3Int(x, y, 0);
                        positions.Add(pos);
                    }
                }

                return positions;
            }
            
            public static List<Vector3Int> GetCircle(Vector3Int start, int size)
            {
                var center = start + (Vector3Int.one * size / 2);
                center.z = 0;

                var positions = new List<Vector3Int>();
                
                for (int x = start.x; x < start.x + size; x++)
                {
                    for (int y = start.y; y < start.y + size; y++)
                    {
                        Vector3Int pos = new Vector3Int(x, y, 0);
                        float distance = Vector3Int.Distance(pos, (Vector3Int)center);
                            
                        if (distance < size / 2)
                        {
                            positions.Add(pos);
                        }
                    }
                }

                return positions;
            }

            public static List<Vector3Int> GetLine(Vector3Int start, Vector3Int end, int thickness)
            {
                HashSet<Vector3Int> positions = new();

                float distance = Vector3Int.Distance(start, end);
                float steps = distance / (thickness / 2f);
                float stepSize = distance / steps;
                Vector3 direction = (end - start);
                direction = direction.normalized;

                for (float i = 0; i < steps; i++)
                {
                    Vector2 pos = new Vector2() {
                        x = start.x + direction.x * stepSize * i,
                        y = start.y + direction.y * stepSize * i,
                    };

                    var square = GetSquare(new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)), thickness);

                    for (int j = 0; j < square.Count; j++)
                    {
                        positions.Add(square[j]);
                    }
                }

                var finalSquare = GetSquare(new Vector3Int(Mathf.RoundToInt(end.x), Mathf.RoundToInt(end.y)), thickness);
                
                for (int i = 0; i < finalSquare.Count; i++)
                {
                    positions.Add(finalSquare[i]);
                }

                return positions.ToList();
            }
        }
    }
}
