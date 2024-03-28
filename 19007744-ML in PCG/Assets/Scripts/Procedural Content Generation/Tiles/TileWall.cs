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
    public class TileWall : Tile
    {
        public TileWall()
        {
            // Set base class properties...
            tileType = TileType.WALL;
            isCollidable = true;
        }

        protected override void Start()
        {
            // Call base implementation of start...
            base.Start();

            // TODO: OTHER LOGIC HERE
        }
    }
}