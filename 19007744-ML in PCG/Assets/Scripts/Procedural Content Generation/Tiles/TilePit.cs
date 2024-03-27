using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace PCG.Tilemaps
{
    public class TilePit : Tile
    {
        public TilePit()
        {
            // Set base class properties...
            tileType = TileType.PIT;
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