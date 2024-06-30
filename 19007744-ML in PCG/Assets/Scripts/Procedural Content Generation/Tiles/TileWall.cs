// Unity
using UnityEngine;

// Sub-namespace for tilemap-related utilities.
namespace PCG.Tilemaps
{
    /// <summary>
    /// Direction the wall is facing, including corner pieces.
    /// </summary>
    public enum WallDirection : int
    {
        // Wall faces
        TOP = 0,
        BOTTOM = 1,
        LEFT = 2,
        RIGHT = 3,

        // Corners
        TOPLEFT = 4,
        TOPRIGHT = 5,
        BOTTOMLEFT = 6,
        BOTTOMRIGHT = 7,
    }

    /// <summary>
    /// Wall tiles that can be collided with (<c>ICollidable</c>).
    /// </summary>
    public class TileWall : Tile, ICollidable
    {
        [Header("Tile Wall Properties")]
        [SerializeField] private WallDirection wallDirection;

        // ICollidable Interface implementation
        public bool IsCollidable { get; protected set; } = true;

        public TileWall()
        {
            // Set base class properties...
            tileType = TileType.WALL;
        }

        protected override void Start()
        {
            // Call base implementation of start...
            base.Start();
        }

        /// <summary>
        /// Gets the direction the wall is facing.
        /// </summary>
        /// <returns>Direction of the wall.</returns>
        public WallDirection GetWallDirection() { return wallDirection; }

        /// <summary>
        /// Sets the direction the wall is facing.
        /// </summary>
        /// <param name="direction">Direction to set for the wall.</param>
        public void SetWallDirection(WallDirection direction) => wallDirection = direction;
    }
}