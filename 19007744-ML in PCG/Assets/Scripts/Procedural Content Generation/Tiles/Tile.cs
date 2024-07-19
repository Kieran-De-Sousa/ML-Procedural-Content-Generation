// Base
using System;

// Unity
using UnityEngine;
using UnityEngine.Tilemaps;

// Machine Learning
using ML;

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
        [Serializable]
        public abstract class Tile : MonoBehaviour
        {
            [Header("Base Properties")]
            public TileBase tileBase;
            protected TileType tileType = TileType.FLOOR;

            // Tilemap
            protected Tilemap ownerTilemap = null;
            protected Vector3Int tilePosition = new();
            protected Vector3 tileWorldPosition = new();

            // Player
            protected MLAgent player = null;

            /// <summary>
            /// Initialise member variables of tile upon simulation start.
            /// </summary>
            protected virtual void Start()
            {
                SetBaseMembers();
            }

            /// <summary>
            /// Set base members of this tile.
            /// </summary>
            /// <note>
            /// This method does not correctly update tilemap and positional data (everything except MLAgent).
            /// However, this is okay as this data is correctly updated whenever appropriate in other systems.
            /// A TODO has been left where the error occurs.
            /// </note>
            public void SetBaseMembers()
            {
                // Find player inventory starting from root parent (the simulation).
                player = HelperUtilities.FindParentOrChildWithComponent<MLAgent>(transform);

                // Find the owner tilemap of this tile, and its position in that tilemap.
                Tilemap[] tilemaps = FindObjectsOfType<Tilemap>();
                foreach (Tilemap tilemap in tilemaps)
                {
                    // Iterate through all the positions in the tilemap
                    BoundsInt bounds = tilemap.cellBounds;
                    foreach (Vector3Int pos in bounds.allPositionsWithin)
                    {
                        // TODO: FIX THIS, AS ALL TILES ARE GONNA SHARE THE SAME TILEBASE
                        if (tilemap.HasTile(pos) && tilemap.GetTile(pos) == tileBase)
                        {
                            // If the tile is found, set protected variables
                            ownerTilemap = tilemap;
                            tilePosition = pos;
                            tileWorldPosition = ownerTilemap.GetCellCenterWorld(pos);

                            return;
                        }
                    }
                }
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

            /// <summary>
            /// Gets the owner tilemap of this tile.
            /// </summary>
            /// <returns>Owner tilemap</returns>
            public Tilemap GetOwnerTilemap() { return ownerTilemap; }

            /// <summary>
            /// Sets the owner tilemap of this tile.
            /// </summary>
            /// <param name="owner">The owner tilemap to set.</param>
            /// <returns></returns>
            public void SetOwnerTilemap(Tilemap owner) => ownerTilemap = owner;

            /// <summary>
            /// Gets the tile position in the tilemap of this tile.
            /// </summary>
            /// <returns>Tile Vector3Int position.</returns>
            public Vector3Int GetTilePosition() { return tilePosition; }

            /// <summary>
            /// Sets the tile position in the tilemap of this tile.
            /// </summary>
            /// <param name="position">Tile position in the tilemap.</param>
            public void SetTilePosition(Vector3Int position) => tilePosition = position;

            /// <summary>
            /// Gets the tile position in world space of this tile.
            /// </summary>
            /// <returns>World space position.</returns>
            public Vector3 GetTileWorldPosition() { return tileWorldPosition; }

            /// <summary>
            /// Sets the tile position in world space of this tile.
            /// </summary>
            /// <param name="position">World space position of tile.</param>
            public void SetTileWorldPosition(Vector3 position) => tileWorldPosition = position;

            /// <summary>
            /// Gets the MLAgent in the simulation of this tile.
            /// </summary>
            /// <returns>Agent of simulation.</returns>
            public MLAgent GetPlayer() { return player; }

            /// <summary>
            /// Sets the MLAgent in the simulation of this tile.
            /// </summary>
            /// <param name="agent">Agent to set.</param>
            public void SetPlayer(MLAgent agent) => player = agent;
        }
    }
}