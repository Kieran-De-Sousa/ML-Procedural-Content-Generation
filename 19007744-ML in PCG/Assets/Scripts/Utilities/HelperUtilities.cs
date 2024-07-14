// Base
using System;
using System.Linq;

// Unity
using UnityEngine;
using UnityEngine.Tilemaps;

// Root namespace for all helper classes/methods.
namespace Utilities
{
    /// <summary>
    /// Set of static helper methods for finding game components, centre positions, etc.
    /// </summary>
    public static class HelperUtilities
    {
        /// <summary>
        /// Finds the nearest parent with the specified component of type T.
        /// </summary>
        /// <param name="childTransform">Starting transform.</param>
        /// <typeparam name="T">Component to find.</typeparam>
        /// <returns>The parent owner of the component, otherwise null.</returns>
        public static T FindParentWithComponent<T>(Transform childTransform) where T : Component
        {
            Transform parent = childTransform;
            while (parent != null)
            {
                T component = parent.GetComponent<T>();
                if (component != null)
                {
                    return component;
                }
                parent = parent.parent;
            }
            return null;
        }

        /// <summary>
        /// Finds the nearest parent with the specified component of type T on itself or any of its children.
        /// </summary>
        /// <param name="childTransform">Starting transform.</param>
        /// <typeparam name="T">Component to find.</typeparam>
        /// <returns>The owner of the component, otherwise null.</returns>
        public static T FindParentOrChildWithComponent<T>(Transform childTransform) where T : Component
        {
            Transform currentTransform = childTransform;
            while (currentTransform != null)
            {
                T component = currentTransform.GetComponentInChildren<T>(true);
                if (component != null)
                {
                    return component;
                }
                currentTransform = currentTransform.parent;
            }
            return null;
        }

        /// <summary>
        /// Delete all child GameObjects of parent GameObject.
        /// </summary>
        /// <param name="parent">Parent GameObject.</param>
        public static void DestroyAllChildren(GameObject parent)
        {
            // Get the number of children
            int childCount = parent.transform.childCount;

            // Loop through each child and destroy it
            for (int i = childCount - 1; i >= 0; i--)
            {
                // Get the child at the current index
                Transform child = parent.transform.GetChild(i);

                // Destroy the child gameObject
                GameObject.Destroy(child.gameObject);
            }
        }

        /// <summary>
        /// Finds the center coordinates of a 2D map represented as a two-dimensional array.
        /// </summary>
        /// <param name="map">2D array map.</param>
        /// <returns>Tuple containing the row and column indices of the center.</returns>
        public static (int row, int column) FindCenter(int[,] map)
        {
            // Get dimensions of the map
            int rowCount = map.GetLength(0);    // Number of rows
            int columnCount = map.GetLength(1); // Number of columns

            // Calculate center indices
            int centerRow = rowCount / 2;
            int centerColumn = columnCount / 2;

            // Return center coordinates as a tuple
            return (centerRow, centerColumn);
        }

        /// <summary>
        /// Gets the center tile position in the given tilemap.
        /// </summary>
        /// <param name="tilemap">The Tilemap to find the center position in.</param>
        /// <returns>The cell position of the center tile.</returns>
        public static Vector3Int GetCenterTilePosition(Tilemap tilemap)
        {
            // Get the bounds of the tilemap
            BoundsInt bounds = tilemap.cellBounds;

            // Calculate the center position
            int centerX = bounds.xMin + (bounds.size.x / 2);
            int centerY = bounds.yMin + (bounds.size.y / 2);

            // Create and return the center cell position
            return new Vector3Int(centerX, centerY, 0);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="map"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FindNearestTileInMap<T>(GameObject gameObject, PCG.Tilemaps.Tile[,] map) where T : PCG.Tilemaps.Tile
        {
            if (map == null || gameObject == null) return null;

            Vector3 gameObjectPosition = gameObject.transform.position;
            T nearestTile = null;
            float nearestDistance = float.MaxValue;

            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    var tile = map[x, y];

                    if (tile != null && tile is T && tile.gameObject != null)
                    {
                        float distance = Vector3.Distance(gameObjectPosition, tile.gameObject.transform.position);
                        if (distance < nearestDistance)
                        {
                            nearestTile = tile as T;
                            nearestDistance = distance;
                        }
                    }
                }
            }

            return nearestTile;
        }
    }

    /// <summary>
    /// Useful extension method for getting minimum / maximum values from an enum.
    /// </summary>
    /// <reference>https://stackoverflow.com/a/1665930</reference>
    public static class EnumExtension
    {
        /// <summary>
        /// Get minimum int value from an enum type.
        /// </summary>
        /// <param name="enumType">Enum type to check.</param>
        /// <returns>Minimum int from enum.</returns>
        /// <reference>https://stackoverflow.com/a/1665930</reference>
        public static int Min(this Enum enumType)
        {
            return Enum.GetValues(enumType.GetType()).Cast<int>().Min();
        }
        /// <summary>
        /// Get maximum int value from an enum type.
        /// </summary>
        /// <param name="enumType">Enum type to check.</param>
        /// <returns>Maximum int from enum.</returns>
        /// <reference>https://stackoverflow.com/a/1665930</reference>
        public static int Max(this Enum enumType)
        {
            return Enum.GetValues(enumType.GetType()).Cast<int>().Max();
        }
    }
}