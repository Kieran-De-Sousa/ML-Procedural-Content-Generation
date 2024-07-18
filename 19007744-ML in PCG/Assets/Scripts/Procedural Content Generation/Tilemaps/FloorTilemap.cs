// Unity

using System;
using UnityEngine;
using UnityEngine.Tilemaps;

// Machine Learning
using ML;

// Helper
using Utilities;

namespace PCG.Tilemaps
{
    public class FloorTilemap : MonoBehaviour
    {
        private Tilemap tilemap;
        private MLAgent player;
        private PCGSystemRefactor pcgSystem;

        // Start is called before the first frame update
        private void Start()
        {
            // Get the Tilemap component attached to the same GameObject
            tilemap = GetComponent<Tilemap>();
            player = HelperUtilities.FindParentOrChildWithComponent<MLAgent>(transform);
            pcgSystem = HelperUtilities.FindParentOrChildWithComponent<PCGSystemRefactor>(transform);
        }

        private void FixedUpdate()
        {
            if (pcgSystem == null) return;

            Tile[,] tilemapCoordinates = pcgSystem.roomData.tilemap;
            if (tilemapCoordinates == null) return;

            // Get the position of the collision
            Vector3 collisionPosition = player.transform.position;

            // Convert the collision position to cell position in the Unity Tilemap
            Vector3Int cellPosition = tilemap.WorldToCell(collisionPosition);

            // Adjust the cell position to match the tilemap array coordinates
            int x = cellPosition.x - tilemap.cellBounds.xMin;
            int y = cellPosition.y - tilemap.cellBounds.yMin;

            // Check bounds to avoid index out of range errors
            if (x >= 0 && x < tilemapCoordinates.GetLength(0) &&
                y >= 0 && y < tilemapCoordinates.GetLength(1))
            {
                Tile collidedTile = tilemapCoordinates[x, y];

                if (collidedTile != null && collidedTile is TileFloor floor)
                {
                    if (!floor.GetIsExplored())
                    {
                        floor.SetOwnerTilemap(tilemap);
                        floor.SetTilePosition(cellPosition);
                        floor.SetTileWorldPosition(tilemap.GetCellCenterWorld(cellPosition));
                        floor.SetPlayer(HelperUtilities.FindParentOrChildWithComponent<MLAgent>(transform));

                        floor.Explore();
                    }
                }
            }
        }
    }
}