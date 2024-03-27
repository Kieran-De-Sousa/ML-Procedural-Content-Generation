using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace PCG.Tilemaps
{
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