// Base
using System.Collections;
using System.Collections.Generic;

// Unity
using UnityEngine;

namespace PCG.Tilemaps
{
    /// <summary>
    ///
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
    ///
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

            // TODO: OTHER LOGIC HERE
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public WallDirection GetWallDirection() { return wallDirection; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="direction"></param>
        public void SetWallDirection(WallDirection direction) => wallDirection = direction;
    }
}