using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

namespace PCG
{
    namespace Tilemaps
    {
        public enum TileType
        {
            NONE,
            FLOOR,
            WALL,
        }

        public class Tile : MonoBehaviour
        {
            [Header("Base Properties")]
            public TileBase tile;
            protected TileType tileType = TileType.NONE;
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
                if (isCollidable)
                {
                    // TODO: COLLISION LOGIC HERE
                }
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            public static Tile CreateTile(TileType type)
            {
                Tile tileObject = null;

                switch (type)
                {
                    case TileType.FLOOR:
                    {
                        tileObject = new TileFloor();
                        break;
                    }
                    case TileType.WALL:
                    {
                        tileObject = new TileWall();
                        break;
                    }
                    default:
                    {
                        tileObject = new Tile();
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
            public void SetIsCollidable(bool collidable) => isCollidable = collidable;
        }
    }
}