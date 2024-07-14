// Unity
using UnityEngine;

// Sub-namespace for tilemap-related utilities.
namespace PCG.Tilemaps
{
    /// <summary>
    /// Base class for "Collidable" GameObjects (e.g. Walls, Pits) to inherit from.
    /// </summary>
    public abstract class TileCollidable : Tile, ICollidable
    {
        // ICollidable Interface implementation
        public bool IsCollidable { get; protected set; } = true;
        public bool IsCollided { get; protected set; } = false;
        public abstract void Collide();

        protected TileCollidable()
        {
            tileType = TileType.WALL;
        }

        protected override void Start()
        {
            // Call base implementation of start...
            base.Start();
        }
    }
}