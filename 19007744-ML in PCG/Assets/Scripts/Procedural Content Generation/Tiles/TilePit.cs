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