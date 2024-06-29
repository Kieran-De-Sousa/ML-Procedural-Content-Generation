// Unity
using UnityEngine;
using UnityEngine.Tilemaps;
using Utilities;

namespace PCG
{
    namespace Tilemaps
    {
        /// <summary>
        /// The different tile types a Tile can be.
        /// </summary>
        public enum TileType : int
        {
            FLOOR  = 0,
            PIT    = 1,
            WALL   = 2,
            ENTITY = 3,
            ITEM   = 4,
        }

        /// <summary>
        /// Extended implementation of Unity's <c>TileBase</c> class providing additional properties and methods.
        /// </summary>
        public abstract class Tile : MonoBehaviour
        {
            [Header("Base Properties")]
            public TileBase tileBase;

            protected TileType tileType = TileType.FLOOR;
            protected Tilemap ownerTilemap;

            protected Vector3Int tilePosition;
            protected Inventory player;

            protected virtual void Start()
            {
                Tilemap[] tilemaps = FindObjectsOfType<Tilemap>();
                foreach (Tilemap tilemap in tilemaps)
                {
                    // Iterate through all the positions in the tilemap
                    BoundsInt bounds = tilemap.cellBounds;
                    foreach (Vector3Int pos in bounds.allPositionsWithin)
                    {
                        if (tilemap.HasTile(pos) && tilemap.GetTile(pos) == tileBase)
                        {
                            // If the tile is found, set protected variables
                            ownerTilemap = tilemap;
                            tilePosition = pos;
                            return;
                        }
                    }
                }

                player = HelperUtilities.FindParentOrChildWithComponent<Inventory>(transform);
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
            ///
            /// </summary>
            /// <returns>Tile base asset</returns>
            public TileBase GetTileBase() { return tileBase; }

            /// <summary>
            /// Sets the tile asset of this tile.
            /// </summary>
            /// <param name="tileBase">The TileBase asset to set.</param>
            public void SetTileBase(TileBase tile) => tileBase = tile;

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
        }
    }
}