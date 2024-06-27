// Base
using System;
using System.Linq;

// Unity
using UnityEngine;

namespace Utilities
{
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