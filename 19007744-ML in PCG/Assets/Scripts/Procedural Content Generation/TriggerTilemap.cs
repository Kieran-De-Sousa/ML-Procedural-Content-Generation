using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

using ML;

using Utilities;

namespace PCG.Tilemaps
{
    public class TriggerTilemap : MonoBehaviour
    {
        private Tilemap tilemap;

        // Start is called before the first frame update
        void Start()
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

            PCGSystemRefactor PCGSystem = HelperUtilities.FindParentOrChildWithComponent<PCGSystemRefactor>(transform);
            if (PCGSystem == null) return;

            Tile[,] tilemapCoordinates = PCGSystem.roomData.tilemap;
            if (tilemapCoordinates == null) return;

            // Get the position of the collision
            Vector3 collisionPosition = other.transform.position;

            // Convert the collision position to cell position in the Unity Tilemap
            Vector3Int cellPosition = tilemap.WorldToCell(collisionPosition);

            // Adjust the cell position to match your tilemap array coordinates
            // Assumes tilemap array starts at (0,0) and matches Unity Tilemap coordinates
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
                    interactable.SetPlayer(HelperUtilities.FindParentOrChildWithComponent<MLAgent>(transform));

                    // Call interact method in interactable.
                    interactable.Interact();
                }
            }
        }
    }
}