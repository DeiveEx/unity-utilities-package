using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DeiveEx.Utilities
{
    public class UiService
    {
        #region Grid Layout Group
        
        /// <inheritdoc cref="SetupGridNavigation{T}(UnityEngine.UI.GridLayoutGroup,System.Collections.Generic.List{T},UnityEngine.UI.Selectable,UnityEngine.UI.Selectable,UnityEngine.UI.Selectable,UnityEngine.UI.Selectable)"/>
        public static void SetupGridNavigation<T>(
            GridLayoutGroup gridLayout, 
            List<T> collection,
            Selectable onUp = null,
            Selectable onDown = null,
            Selectable onLeft = null,
            Selectable onRight = null
        )
            where T : Selectable
        {
            SetupGridNavigation(gridLayout, collection,
                gridPos => onUp,
                gridPos => onDown,
                gridPos => onLeft,
                gridPos => onRight
                );
        }
        
        /// <summary>
        /// Automatically setups the navigation between items in a grid layout, and allows to set Selectables for
        /// when trying to move outside the boundaries of the grid.
        /// This method assumes the grid starts counting from the top left
        /// </summary>
        /// <param name="gridLayout">The Layout component</param>
        /// <param name="collection">The Selectable collection to configure the navigation for</param>
        /// <param name="onUp">What should be selected when pressing "up" while being on the first row</param>
        /// <param name="onDown">What should be selected when pressing "down" while being on the last row</param>
        /// <param name="onLeft">What should be selected when pressing "left" while being on the first column row</param>
        /// <param name="onRight">What should be selected when pressing "right" while being on the last column</param>
        /// <typeparam name="T">The type of selectable (Selectable, Button, etc.)</typeparam>
        public static void SetupGridNavigation<T>(
            GridLayoutGroup gridLayout, 
            List<T> collection, 
            Func<Vector2Int, Selectable> onUp,
            Func<Vector2Int, Selectable> onDown,
            Func<Vector2Int, Selectable> onLeft,
            Func<Vector2Int, Selectable> onRight)
            where T : Selectable
        {
            if (gridLayout.constraint == GridLayoutGroup.Constraint.Flexible)
            {
                Debug.LogError("Can't use this method with flexible Grid layout");
                return;
            }

            Vector2Int gridSize = GetGridSize(gridLayout, collection.Count);
            Dictionary<Vector2Int, Selectable> itemsByGridPosition = new();

            for (int i = 0; i < collection.Count; i++)
            {
                var gridPos = GetGridPositionForIndex(gridLayout, gridSize, i);
                itemsByGridPosition.Add(gridPos, collection[i]);
            }

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    var gridPos = new Vector2Int(x, y);
                    SetupNavigationForGridCell(collection, gridPos, gridSize, itemsByGridPosition, onUp, onDown, onLeft, onRight);
                }
            }
        }

        private static void SetupNavigationForGridCell<T>(
            List<T> collection,
            Vector2Int gridPos,
            Vector2Int gridSize,
            Dictionary<Vector2Int, Selectable> itemsByGridPosition,
            Func<Vector2Int, Selectable> onUp,
            Func<Vector2Int, Selectable> onDown,
            Func<Vector2Int, Selectable> onLeft,
            Func<Vector2Int, Selectable> onRight
        )
            where T : Selectable
        {
            if(!itemsByGridPosition.ContainsKey(gridPos))
                return;

            var selectable = itemsByGridPosition[gridPos];
            // selectable.name = gridPos.ToString();
                    
            Navigation navigation = new Navigation();
            navigation.mode = Navigation.Mode.Explicit;
                    
            //Left
            if (gridPos.x == 0)
            {
                navigation.selectOnLeft = onLeft?.Invoke(gridPos);
            }
            else
            {
                Vector2Int neighborPos = gridPos + Vector2Int.left;
                        
                if(itemsByGridPosition.TryGetValue(neighborPos, out var neighbor))
                    navigation.selectOnLeft = neighbor;
            }
                    
            //Up
            if (gridPos.y == 0)
            {
                navigation.selectOnUp = onUp?.Invoke(gridPos);
            }
            else
            {
                Vector2Int neighborPos = gridPos - Vector2Int.up; //Subtracting because we're using an inverted Y (down increases)
                        
                if(itemsByGridPosition.TryGetValue(neighborPos, out var neighbor))
                    navigation.selectOnUp = neighbor;
            }
                    
            //Right
            if (gridPos.x >= gridSize.x - 1)
            {
                navigation.selectOnRight = onRight?.Invoke(gridPos);
            }
            else
            {
                Vector2Int neighborPos = gridPos + Vector2Int.right;

                if (itemsByGridPosition.TryGetValue(neighborPos, out var neighbor))
                {
                    navigation.selectOnRight = neighbor;
                }
                else
                {
                    //We're in the second-to-last columns, so select the last entry if we're not already the last entry
                    if (selectable != collection[^1])
                        navigation.selectOnRight = collection[^1];
                    else
                        navigation.selectOnRight = onRight?.Invoke(gridPos);
                }
            }
                    
            //Down
            if (gridPos.y >= gridSize.y - 1)
            {
                navigation.selectOnDown = onDown?.Invoke(gridPos);
            }
            else
            {
                Vector2Int neighborPos = gridPos - Vector2Int.down; //Subtracting because we're using an inverted Y (down increases)

                if (itemsByGridPosition.TryGetValue(neighborPos, out var neighbor))
                {
                    navigation.selectOnDown = neighbor;
                }
                else
                {
                    //We're in the second-to-last row, so select the last entry if we're not already the last entry
                    if (selectable != collection[^1])
                        navigation.selectOnDown = collection[^1];
                    else
                        navigation.selectOnDown = onDown?.Invoke(gridPos);
                }
            }
                    
            //Set navigation
            selectable.navigation = navigation;
        }

        public static Vector2Int GetGridSize(GridLayoutGroup gridLayout, int itemAmount)
        {
            Vector2Int gridSize = new();
            
            if (gridLayout.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
            {
                gridSize.x = gridLayout.constraintCount;
                gridSize.y = Mathf.CeilToInt(itemAmount/ (float)gridSize.x);
            }
            else
            {
                gridSize.y = gridLayout.constraintCount;
                gridSize.x = Mathf.CeilToInt(itemAmount / (float)gridSize.y);
            }

            return gridSize;
        }

        public static Vector2Int GetGridPositionForIndex(GridLayoutGroup gridLayout, Vector2Int gridSize, int index)
        {
            Vector2Int gridPos = new();

            switch (gridLayout.startAxis)
            {
                case GridLayoutGroup.Axis.Horizontal:
                    gridPos.x = index % gridSize.x;
                    gridPos.y = index / gridSize.x;
                    break;
                case GridLayoutGroup.Axis.Vertical:
                    gridPos.x = index / gridSize.y;
                    gridPos.y = index % gridSize.y;
                    break;
            }
            
            return gridPos;
        }
        
        public static int GetIndexFromGridPos(GridLayoutGroup gridLayout, Vector2Int gridSize, Vector2Int gridPos)
        {
            if(gridLayout.startAxis == GridLayoutGroup.Axis.Horizontal)
                return (gridPos.y * gridSize.x) + gridPos.x;
            
            return (gridPos.x * gridSize.y) + gridPos.y;
        }
        
        /// <summary>
        /// Get the selectable on a different grid to the right based on the position of the current selected grid position
        /// </summary>
        /// <param name="sourceGridPos">The current selected grid position, on the current grid. This is the position we're leaving from</param>
        /// <param name="rightGrid">The grid we want to travel to</param>
        /// <param name="rightCollection">The selectable collection of the grid we want to travel to</param>
        /// <returns>The selectable in the right grid that has the closest grid position from the sourceGridPos</returns>
        public static Selectable GetConnectionBetweenGridsOnRightInput(
            Vector2Int sourceGridPos,
            GridLayoutGroup rightGrid,
            List<Selectable> rightCollection
            )
        {
            if (rightCollection == null || rightCollection.Count == 0)
                return null;

            //If we're in the first row, go to the first item in the right grid
            if(sourceGridPos.y == 0)
                return rightCollection[0];
                        
            var rightGridSize = GetGridSize(rightGrid, rightCollection.Count);
                        
            //If the right grid only have 1 row, then go to the first item
            if(rightGridSize.y == 1)
                return rightCollection[0];
                        
            int slotIndex = 0;
                        
            //If the right grid size has at least the same amount of rows as the position we're trying to leave,
            //go to item at (0, gridPos.y)
            if (rightGridSize.y < sourceGridPos.y)
            {
                slotIndex = GetIndexFromGridPos(rightGrid, rightGridSize, new Vector2Int(0, sourceGridPos.y));
                return rightCollection[slotIndex];
            }
                        
            //If we reach this point, this means our current grid has more rows than the right grid, so we go to the
            //first item in the last row of the right grid. Since it's possible this index is greater than the collection,
            //we get the lowest of the two
            slotIndex = GetIndexFromGridPos(rightGrid, rightGridSize, new Vector2Int(0, rightGridSize.y - 1));
            slotIndex = Mathf.Min(rightCollection.Count - 1, slotIndex);
            return rightCollection[slotIndex];
        }
        
        public static Selectable GetConnectionBetweenGridsOnLeftInput(
            Vector2Int sourceGridPos,
            GridLayoutGroup leftGrid,
            List<Selectable> leftCollection
            )
        {
            if (leftCollection == null || leftCollection.Count == 0)
                return null;
            
            var leftGridSize = GetGridSize(leftGrid, leftCollection.Count);
                        
            //If the left grid just have one row or if we're in a row that doesn't exist in the left grid, return the last item
            if(leftGridSize.y == 1 || leftGridSize.y < sourceGridPos.y)
                return leftCollection[^1];
                        
            //If the left grid has at least the same amount of rows as the current one, try to go to
            //the item in (gridSize.x - 1, gridPos.y). Since it's possible that the last column contains less items
            //than the other ones, we get the lowest value between the desiredIndex and the last index available
            int desiredSlotIndex = GetIndexFromGridPos(leftGrid, leftGridSize, new Vector2Int(leftGridSize.x - 1, sourceGridPos.y));
            int actualSlotIndex = Mathf.Min(leftCollection.Count - 1, desiredSlotIndex);
            return leftCollection[actualSlotIndex];
        }
        
        public static Selectable GetConnectionBetweenGridsOnDownInput(
            Vector2Int sourceGridPos,
            GridLayoutGroup downGrid,
            List<Selectable> downCollection
        )
        {
            if (downCollection == null || downCollection.Count == 0)
                return null;
            
            //If we're in the first column, go to the first item in the next grid
            if(sourceGridPos.x == 0)
                return downCollection[0];
            
            var downGridSize = GetGridSize(downGrid, downCollection.Count);
                        
            //If the down grid only have 1 column, then go to the first item
            if(downGridSize.x == 1)
                return downCollection[0];
            
            int slotIndex = 0;
                    
            //If the down grid has at least the same amount of columns as the position we're leaving, go to
            //the item at (gridPos.x, 0)
            if (downGridSize.x < sourceGridPos.y)
            {
                slotIndex = GetIndexFromGridPos(downGrid, downGridSize, new Vector2Int(sourceGridPos.x, 0));
                return downCollection[slotIndex];
            }
                    
            //If we reached here, this means our current grid has more columns than the down one, so we go to the last
            //item in the first row. Since it's possible this index is greater than the collection, we get the lowest
            //of the two
            slotIndex = GetIndexFromGridPos(downGrid, downGridSize, new Vector2Int(downGridSize.x - 1, 0));
            slotIndex = Mathf.Min(downCollection.Count - 1, slotIndex);
            return downCollection[slotIndex];
        }
        
        public static Selectable GetConnectionBetweenGridsOnUpInput(
            Vector2Int sourceGridPos,
            GridLayoutGroup upGrid,
            List<Selectable> upCollection
        )
        {
            if (upCollection == null || upCollection.Count == 0)
                return null;
            
            var upGridSize = GetGridSize(upGrid, upCollection.Count);
                        
            //If the up grid just have one column or if we're in a column that doesn't exist in the
            //up grid, return the last item
            if(upGridSize.x == 1 || upGridSize.x < sourceGridPos.x)
                return upCollection[^1];
                        
            //If the up grid has at least the same amount of columns as the current one, try to go to
            //the item in (gridPos.x, gridSize.y - 1). Since it's possible that the last row contains less items
            //than the other ones, we get the lowest value between the desiredIndex and the last index available
            int desiredSlotIndex = GetIndexFromGridPos(upGrid, upGridSize, new Vector2Int(sourceGridPos.x, upGridSize.y - 1));
            int actualSlotIndex = Mathf.Min(upCollection.Count - 1, desiredSlotIndex);
            return upCollection[actualSlotIndex];
        }
        
        #endregion

        #region Vertical Layout Group
        
        public static void SetupVerticalLayoutNavigation<T>(
            List<T> collection,
            Func<Selectable> onUp = null,
            Func<Selectable> onDown = null,
            Func<Selectable> onLeft = null,
            Func<Selectable> onRight = null
        )
            where T : Selectable
        {
            SetupVerticalLayoutNavigation(
                collection,
                onUp?.Invoke(),
                onDown?.Invoke(),
                onLeft?.Invoke(),
                onRight?.Invoke()
            );
        }
        
        public static void SetupVerticalLayoutNavigation<T>(
            List<T> collection,
            Selectable onUp = null,
            Selectable onDown = null,
            Selectable onLeft = null,
            Selectable onRight = null
        )
            where T : Selectable
        {
            for (int i = 0; i < collection.Count; i++)
            {
                Navigation navigation = new Navigation { mode = Navigation.Mode.Explicit };

                if(i == 0)
                    navigation.selectOnUp = onUp;
                
                if(i == collection.Count - 1)
                    navigation.selectOnDown = onDown;
                
                if (i > 0)
                    navigation.selectOnUp = collection[i - 1];

                if (i < collection.Count - 1)
                    navigation.selectOnDown = collection[i + 1];
                
                navigation.selectOnLeft = onLeft;
                navigation.selectOnRight = onRight;

                collection[i].navigation = navigation;
            }
        }

        #endregion

        #region Horizontal Layout Group

        public static void SetupHorizontalLayoutNavigation<T>(
            List<T> collection,
            Func<Selectable> onUp = null,
            Func<Selectable> onDown = null,
            Func<Selectable> onLeft = null,
            Func<Selectable> onRight = null
        )
            where T : Selectable
        {
            SetupVerticalLayoutNavigation(
                collection,
                onUp?.Invoke(),
                onDown?.Invoke(),
                onLeft?.Invoke(),
                onRight?.Invoke()
            );
        }
        
        public static void SetupHorizontalLayoutNavigation<T>(
            List<T> collection,
            Selectable onUp = null,
            Selectable onDown = null,
            Selectable onLeft = null,
            Selectable onRight = null
        )
            where T : Selectable
        {
            for (int i = 0; i < collection.Count; i++)
            {
                Navigation navigation = new Navigation { mode = Navigation.Mode.Explicit };

                if(i == 0)
                    navigation.selectOnLeft = onLeft;
                
                if(i == collection.Count - 1)
                    navigation.selectOnRight = onRight;
                
                if (i > 0)
                    navigation.selectOnLeft = collection[i - 1];

                if (i < collection.Count - 1)
                    navigation.selectOnRight = collection[i + 1];
                
                navigation.selectOnUp = onUp;
                navigation.selectOnDown = onDown;

                collection[i].navigation = navigation;
            }
        }

        #endregion
    }
}
