// Unity
using UnityEngine;
using UnityEngine.Tilemaps;

// Machine Learning
using ML;

// Helper
using Utilities;

// Sub-namespace for tilemap-related utilities.
namespace PCG.Tilemaps
{
    /// <summary>
    /// Manages floor (decorative) tilemap, detects the agent's location and rewards exploration if done.
    /// </summary>
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

        /// <summary>
        /// Note: This implementation is very similar to <c>TriggerTilemap</c>, however, checking for tiles required
        /// being in a FixedUpdate loop due to how TriggerEvents are registered. Manual checking in FixedUpdate was the preferred
        /// choice in this case.
        /// </summary>
        private void FixedUpdate()
        {
            if (pcgSystem == null) return;

            Tile[,] tilemapCoordinates = pcgSystem.roomData.tilemap;
            if (tilemapCoordinates == null) return;

            // Get the position of the player.
            Vector3 collisionPosition = player.transform.position;

            // Convert the player position to cell position in the Unity Tilemap.
            Vector3Int cellPosition = tilemap.WorldToCell(collisionPosition);

            // Adjust the cell position to match the tilemap array coordinates.
            int x = cellPosition.x - tilemap.cellBounds.xMin;
            int y = cellPosition.y - tilemap.cellBounds.yMin;

            // Check bounds to avoid index out of range errors.
            if (x >= 0 && x < tilemapCoordinates.GetLength(0) &&
                y >= 0 && y < tilemapCoordinates.GetLength(1))
            {
                Tile collidedTile = tilemapCoordinates[x, y];

                if (collidedTile != null && collidedTile is TileFloor floor)
                {
                    // Query for if the floor tile has already been explored...
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