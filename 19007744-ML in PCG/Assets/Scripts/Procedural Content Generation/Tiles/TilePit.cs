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
    public class TilePit : Tile, ICollidable
    {
        // ICollidable Interface implementation
        public bool IsCollidable { get; protected set; } = true;

        public TilePit()
        {
            // Set base class properties...
            tileType = TileType.PIT;
        }

        protected override void Start()
        {
            // Call base implementation of start...
            base.Start();

            // TODO: OTHER LOGIC HERE
        }
    }
}