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
        [RequireComponent(typeof(Rigidbody2D), typeof(TilemapCollider2D),
            typeof(CompositeCollider2D))]
        public class Tile : MonoBehaviour
        {
            [Header("Base Properties")]
            public TileBase tile;
            protected TileType tileType = TileType.FLOOR;
            protected bool isCollidable = false;

            protected Rigidbody2D _rigidbody2D;
            protected TilemapCollider2D _tilemapCollider2D;
            protected CompositeCollider2D _compositeCollider2D;

            /// <summary>
            /// Assign components if not already set.
            /// </summary>
            protected void OnEnable()
            {
                // Add Rigidbody2D component if not attached.
                if (!_rigidbody2D)
                {
                    _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
                    if (!_rigidbody2D)
                    {
                        _rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
                        _rigidbody2D.bodyType = RigidbodyType2D.Static;
                    }
                }

                // Add TilemapCollider2D component if not attached.
                if (!_tilemapCollider2D)
                {
                    _tilemapCollider2D = gameObject.GetComponent<TilemapCollider2D>();
                    if (!_tilemapCollider2D)
                    {
                        _tilemapCollider2D = gameObject.AddComponent<TilemapCollider2D>();
                    }

                    // Disable by default
                    _tilemapCollider2D.enabled = false;
                }

                // Add CompositeCollider2D component if not attached.
                if (!_compositeCollider2D)
                {
                    _compositeCollider2D = gameObject.GetComponent<CompositeCollider2D>();
                    if (!_compositeCollider2D)
                    {
                        _compositeCollider2D = gameObject.AddComponent<CompositeCollider2D>();
                    }
                }
            }

            protected virtual void Start()
            {
                // Enable / Disable the tilemap collider based on collidable state.
                _tilemapCollider2D.enabled = isCollidable;
            }

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
            /// Sets the tile asset of this tile.
            /// </summary>
            /// <param name="tileBase">The TleBase asset to set.</param>
            public void SetTile(TileBase tileBase) => tile = tileBase;

            /// <summary>
            /// Sets the type of this tile.
            /// </summary>
            /// <param name="type">The type of tile to set.</param>
            public void SetTileType(TileType type) => tileType = type;

            /// <summary>
            /// Sets whether this tile is collidable.
            /// </summary>
            /// <param name="collidable"><c>true</c> if the tile is collidable, otherwise; <c>false</c>.</param>
            public void SetIsCollidable(bool collidable)
            {
                isCollidable = collidable;
                _tilemapCollider2D.enabled = true;
            }
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