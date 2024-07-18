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
    /// Manages trigger (item/door) tilemap, detects the agent's location when hitting a trigger
    /// and rewards if item/door is interacted with.
    /// </summary>
    public class TriggerTilemap : MonoBehaviour
    {
        private Tilemap tilemap;

        private void Start()
        {
            // Get the Tilemap component attached to the same GameObject
            tilemap = GetComponent<Tilemap>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            tilemap.RefreshAllTiles();

            // Make sure collision is with the player / ml agent
            if (!other.CompareTag("Player")) return;

            // Use the trigger 2D Collider of the player (smaller, and far more accurate at translating
            // position coordinates to cell position coordinates)
            if (!other.isTrigger) return;

            PCGSystemRefactor pcgSystem = HelperUtilities.FindParentOrChildWithComponent<PCGSystemRefactor>(transform);
            if (pcgSystem == null) return;

            Tile[,] tilemapCoordinates = pcgSystem.roomData.tilemap;
            if (tilemapCoordinates == null) return;

            // Get the position of the collision
            Vector3 collisionPosition = other.transform.position;

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

                if (collidedTile != null && collidedTile is IInteractable)
                {
                    TileInteractable interactable = (TileInteractable) collidedTile;

                    // Check if the interactable object can be interacted with.
                    if (!interactable.IsInteractable) return;

                    // Possible null reference exception handling...
                    interactable.SetOwnerTilemap(tilemap);
                    interactable.SetTilePosition(cellPosition);
                    interactable.SetTileWorldPosition(tilemap.GetCellCenterWorld(cellPosition));
                    interactable.SetPlayer(HelperUtilities.FindParentOrChildWithComponent<MLAgent>(transform));

                    // Call interact method in interactable.
                    interactable.Interact();

                    if (interactable is Item)
                    {
                        Destroy(interactable.gameObject);
                    }
                }
            }
        }
    }
}