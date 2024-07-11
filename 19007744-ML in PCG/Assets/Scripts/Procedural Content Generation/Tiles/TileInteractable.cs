// Unity
using UnityEngine;

// Sub-namespace for tilemap-related utilities.
namespace PCG.Tilemaps
{
    /// <summary>
    /// Base class for "Interactable" GameObjects (e.g. Doors, pickups) to inherit from.
    /// Derived classes will have the <c>Interact</c> method called when hitting a trigger collider.
    /// </summary>
    public abstract class TileInteractable : Tile, IInteractable
    {
        // IInteractable Interface implementation
        public bool IsInteractable { get; set; } = true;
        public bool IsInteracted { get; protected set; } = false;
        public abstract void Interact();

        protected TileInteractable()
        {
            tileType = TileType.ENTITY;
        }

        protected override void Start()
        {
            // Call base implementation of start...
            base.Start();
        }
    }
}