// Unity
using UnityEngine;
using UnityEngine.Tilemaps;

// Machine Learning
using ML;

// Helper
using Utilities;

namespace PCG.Tilemaps
{
    public class CollidableTilemap : MonoBehaviour
    {
        [Tooltip("This will move the cell position correctly if the collision was just outside of" +
                 "the tile cell position. WorldToCell will always round down, so this alleviates that.")]
        public Vector2 collisionBuffer = new(0.02f, 0.02f);

        private Tilemap tilemap;

        // Start is called before the first frame update
        void Start()
        {
            // Get the Tilemap component attached to the same GameObject
            tilemap = GetComponent<Tilemap>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            tilemap.RefreshAllTiles();

            // Make sure collision is with the player / ml agent
            if (!other.gameObject.CompareTag("Player")) return;

            PCGSystemRefactor PCGSystem = HelperUtilities.FindParentOrChildWithComponent<PCGSystemRefactor>(transform);
            if (PCGSystem == null) return;

            Tile[,] tilemapCoordinates = PCGSystem.roomData.tilemap;
            if (tilemapCoordinates == null) return;

            // Get the position of the collision
            Vector3 collisionPosition = other.GetContact(0).point;

            // Adjust the collision position based on the collision side.
            // Adds a small buffer depending on hit direction, as WorldToCell
            // will ALWAYS round down, so we might miss collisions that are barely out of scope.
            Vector3 collisionNormal = other.GetContact(0).normal;

            // Collision from left
            if (collisionNormal.x >= 1) collisionPosition.x += collisionBuffer.x;
            // Collision from right
            else if (collisionNormal.x <= -1) collisionPosition.x -= collisionBuffer.x;

            // Collision from below
            if (collisionNormal.y >= 1) collisionPosition.y += collisionBuffer.y;
            // Collision from above
            else if (collisionNormal.y <= -1) collisionPosition.y -= collisionBuffer.y;


            // Convert the collision position to cell position in the Unity Tilemap
            Vector3Int cellPosition = tilemap.WorldToCell(collisionPosition);

            // Adjust the cell position to match tilemap array coordinates
            int x = cellPosition.x - tilemap.cellBounds.xMin;
            int y = cellPosition.y - tilemap.cellBounds.yMin;

            // Check bounds to avoid index out of range errors
            if (x >= 0 && x < tilemapCoordinates.GetLength(0) &&
                y >= 0 && y < tilemapCoordinates.GetLength(1))
            {
                Tile collidedTile = tilemapCoordinates[x, y];

                if (collidedTile != null && collidedTile is ICollidable)
                {
                    TileCollidable collidable = (TileCollidable) collidedTile;

                    // Check if the interactable object can be collided with.
                    if (!collidable.IsCollidable) return;

                    // Possible null reference exception handling...
                    collidable.SetOwnerTilemap(tilemap);
                    collidable.SetTilePosition(cellPosition);
                    collidable.SetTileWorldPosition(tilemap.GetCellCenterWorld(cellPosition));
                    collidable.SetPlayer(HelperUtilities.FindParentOrChildWithComponent<MLAgent>(transform));

                    // Call collide method in interactable.
                    collidable.Collide();
                }
            }
        }
    }
}