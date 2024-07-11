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

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                PCGSystemRefactor PCGSystem = HelperUtilities.FindParentOrChildWithComponent<PCGSystemRefactor>(transform);
                if (PCGSystem != null)
                {
                    Tile[,] tilemapCoordinates = PCGSystem.roomData.tilemap;
                    if (tilemapCoordinates != null)
                    {
                        // Get the position of the collision
                        Vector3 collisionPosition = other.transform.position;
                        Debug.Log($"Player collided at: {collisionPosition}");

                        // Convert the collision position to cell position in the Unity Tilemap
                        Vector3Int cellPosition = tilemap.WorldToCell(collisionPosition);
                        Debug.Log($"Cell position at: {cellPosition}");

                        // Adjust the cell position to match your tilemap array coordinates
                        // Assumes tilemap array starts at (0,0) and matches Unity Tilemap coordinates
                        int x = cellPosition.x - tilemap.cellBounds.xMin;
                        int y = cellPosition.y - tilemap.cellBounds.yMin;

                        Debug.Log($"Player collided with trigger tile at array position: ({x}, {y})");

                        // Check bounds to avoid index out of range errors
                        if (x >= 0 && x < tilemapCoordinates.GetLength(0) &&
                            y >= 0 && y < tilemapCoordinates.GetLength(1))
                        {
                            Tile collidedTile = tilemapCoordinates[x, y];

                            if (collidedTile != null && collidedTile is IInteractable)
                            {
                                TileInteractable interactable = (TileInteractable) collidedTile;
                                Debug.Log($"Player collided with trigger tile: {interactable.name} at array position: ({x}, {y})");

                                interactable.SetOwnerTilemap(tilemap);
                                interactable.SetTilePosition(cellPosition);
                                interactable.SetPlayer(HelperUtilities.FindParentOrChildWithComponent<MLAgent>(transform));

                                //PCGSystem.roomData.tilemap[x, y].SetOwnerTilemap(tilemap);;
                                //PCGSystem.roomData.tilemap[x, y].SetTilePosition(cellPosition);
                                //PCGSystem.roomData.tilemap[x, y].SetPlayer(HelperUtilities.FindParentOrChildWithComponent<MLAgent>(transform));;
                                //TileInteractable interactable = (TileInteractable) PCGSystem.roomData.tilemap[x, y];

                                interactable.Interact();
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("Tilemap array is null in PCGSystemRefactor's roomData.");
                    }
                }
            }
        }

        /*private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                PCGSystemRefactor PCGSystem = HelperUtilities.FindParentOrChildWithComponent<PCGSystemRefactor>(transform);
                if (PCGSystem != null)
                {
                    Tile[,] tilemapCoordinates = PCGSystem.roomData.tilemap;
                    if (tilemapCoordinates != null)
                    {
                        // Get the position of the collision
                        Vector3 collisionPosition = other.transform.position;

                        // Convert the collision position to cell position in the Unity Tilemap
                        Vector3Int cellPosition = tilemap.WorldToCell(collisionPosition);

                        //Debug.Log($"Cell Position: {cellPosition}");

                        // Iterate through the tilemap array to find the matching tile position
                        for (int x = 0; x < tilemapCoordinates.GetLength(0); x++)
                        {
                            for (int y = 0; y < tilemapCoordinates.GetLength(1); y++)
                            {
                                Tile tile = tilemapCoordinates[x, y];

                                if (tile.GetOwnerTilemap() == tilemap)
                                {
                                    if (tile != null && tile.GetTilePosition() == cellPosition)
                                    {
                                        Debug.Log($"Collided Tile Position: {tile.GetTilePosition()}");
                                        Debug.Log($"Collided Tile Type: {tile.GetTileType()}");

                                        if (tile is IInteractable)
                                        {
                                            var interactableTile = (TileInteractable) tile;

                                            interactableTile.Interact();
                                            Debug.Log($"Player collided with interactable tile: {interactableTile.name} at position: ({x}, {y})");
                                        }

                                        return; // Exit the method once the collided tile is found
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }*/
    }
}