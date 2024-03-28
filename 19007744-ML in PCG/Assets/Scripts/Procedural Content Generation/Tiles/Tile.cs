// Base
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Unity
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PCG
{
    namespace Tilemaps
    {
        /// <summary>
        /// The different tile types a Tile can be.
        /// </summary>
        public enum TileType : int
        {
            FLOOR = 0,
            PIT   = 1,
            WALL  = 2,
        }

        /// <summary>
        /// Extended implementation of Unity's <c>TileBase</c> class providing additional properties and methods.
        /// </summary>
        public abstract class Tile : MonoBehaviour
        {
            [Header("Base Properties")]
            public TileBase tile;
            protected TileType tileType = TileType.FLOOR;
            protected bool isCollidable = false;

            protected virtual void Start() {}

            /// <summary>
            /// Creates a new GameObject with an extended functionality <c>tile</c> component.
            /// </summary>
            /// <param name="type">The tile type to create.</param>
            /// <returns>GameObject with the tile component attached.</returns>
            public static GameObject CreateTile(TileType type)
            {
                GameObject tileObject = new GameObject("Tile");
                Tile tileComponent = null;

                switch (type)
                {
                    case TileType.FLOOR:
                    {
                        // Add TileFloor component
                        tileComponent = tileObject.AddComponent<TileFloor>();
                        break;
                    }
                    case TileType.WALL:
                    {
                        // Add TileWall component
                        tileComponent = tileObject.AddComponent<TileWall>();
                        break;
                    }
                    case TileType.PIT:
                    {
                        // Add TilePit component
                        tileComponent = tileObject.AddComponent<TilePit>();
                        break;
                    }

                    default:
                    {
                        Debug.LogError("ERROR: TILE COULD NOT BE CREATED THROUGH CreateTile METHOD!");
                        break;
                    }
                }

                return tileObject;
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns>Tile base asset</returns>
            public TileBase GetTile() { return tile; }

            /// <summary>
            /// Sets the tile asset of this tile.
            /// </summary>
            /// <param name="tileBase">The TileBase asset to set.</param>
            public void SetTile(TileBase tileBase) => tile = tileBase;

            /// <summary>
            ///
            /// </summary>
            /// <returns>Tile type</returns>
            public TileType GetTileType() { return tileType; }

            /// <summary>
            /// Sets the type of this tile.
            /// </summary>
            /// <param name="type">The type of tile to set.</param>
            public void SetTileType(TileType type) => tileType = type;

            /// <summary>
            ///
            /// </summary>
            /// <returns>IsCollidable</returns>
            public bool GetIsCollidable() { return isCollidable; }

            /// <summary>
            /// Sets whether this tile is collidable.
            /// </summary>
            /// <param name="collidable"><c>true</c> if the tile is collidable, otherwise; <c>false</c>.</param>
            public void SetIsCollidable(bool collidable) => isCollidable = collidable;

        }
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