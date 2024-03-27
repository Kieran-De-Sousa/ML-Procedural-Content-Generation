using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace PCG.Tilemaps
{
    public class TileFloor : Tile
    {
        public TileFloor()
        {
            // Set base class properties...
            tileType = TileType.FLOOR;
            isCollidable = false;
        }

        protected override void Start()
        {
            // Call base implementation of start...
            base.Start();

            // TODO: OTHER LOGIC HERE
        }
    }
}