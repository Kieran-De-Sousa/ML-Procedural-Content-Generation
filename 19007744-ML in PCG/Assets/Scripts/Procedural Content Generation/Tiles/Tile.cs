using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

namespace PCG
{
    namespace Tilemaps
    {
        /// <summary>
        ///
        /// </summary>
        public enum TileType : int
        {
            FLOOR = 0,
            PIT   = 1,
            WALL  = 2,
        }

        /// <summary>
        ///
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
            ///
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            public static GameObject CreateTile(TileType type)
            {
                GameObject tileObject = new GameObject("Tile");
                Tile tileComponent = null;

                switch (type)
                {
                    case TileType.FLOOR:
                    {
                        tileComponent = tileObject.AddComponent<TileFloor>(); // Add TileFloor component
                        break;
                    }
                    case TileType.WALL:
                    {
                        tileComponent = tileObject.AddComponent<TileWall>(); // Add TileWall component
                        break;
                    }
                    case TileType.PIT:
                    {
                        tileComponent = tileObject.AddComponent<TilePit>(); // Add TilePit component
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
            /// <param name="tileBase"></param>
            public void SetTile(TileBase tileBase) => tile = tileBase;

            /// <summary>
            ///
            /// </summary>
            /// <param name="type"></param>
            public void SetTileType(TileType type) => tileType = type;

            /// <summary>
            ///
            /// </summary>
            /// <param name="collidable"></param>
            public void SetIsCollidable(bool collidable)
            {
                isCollidable = collidable;
                _tilemapCollider2D.enabled = true;
            }
        }
    }
}