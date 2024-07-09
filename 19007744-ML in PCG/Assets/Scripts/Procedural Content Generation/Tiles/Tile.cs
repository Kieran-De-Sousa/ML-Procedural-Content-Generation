// Unity
using UnityEngine;
using UnityEngine.Tilemaps;

// Helper
using Utilities;

// Root namespace for all Procedural Content Generation-related utilities.
namespace PCG
{
    // Sub-namespace for tilemap-related utilities.
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

            // Tilemap
            protected Tilemap ownerTilemap;
            protected Vector3Int tilePosition;

            // Player
            protected Inventory player;

            /// <summary>
            /// Initialise member variables of tile upon simulation start.
            /// </summary>
            protected virtual void Start()
            {
                SetBaseMembers();
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

            public void SetBaseMembers()
            {
                // Find the owner tilemap of this tile, and its position in that tilemap.
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

                // Find player inventory starting from root parent (the simulation).
                player = HelperUtilities.FindParentOrChildWithComponent<Inventory>(transform);
            }

            /// <summary>
            /// Gets the tile asset of this tile.
            /// </summary>
            /// <returns>Tile base asset.</returns>
            public TileBase GetTileBase() { return tileBase; }

            /// <summary>
            /// Sets the tile asset of this tile.
            /// </summary>
            /// <param name="tile">The TileBase asset to set.</param>
            public void SetTileBase(TileBase tile) => tileBase = tile;

            /// <summary>
            /// Gets the type of this tile.
            /// </summary>
            /// <returns>Tile type</returns>
            public TileType GetTileType() { return tileType; }

            /// <summary>
            /// Sets the type of this tile.
            /// </summary>
            /// <param name="type">The type of tile to set.</param>
            public void SetTileType(TileType type) => tileType = type;

            public Tilemap GetOwnerTilemap()
            {
                SetBaseMembers();
                return ownerTilemap;
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            public Vector3Int GetTilePosition()
            {
                SetBaseMembers();
                return tilePosition;
            }
        }
    }
}