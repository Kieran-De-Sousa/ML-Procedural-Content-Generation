using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace PCG.Tilemaps
{
    /// <summary>
    /// Base class for "Interactable" GameObjects (e.g. Doors, pickups) to inherit from.
    /// Derived classes will ideally have the <c>Interact</c> method called when hitting a trigger collider.
    /// </summary>
    public abstract class TileInteractable : Tile
    {
        protected bool isInteracted = false;

        protected TileInteractable()
        {
            isInteractable = true;
        }

        protected override void Start()
        {
            // Call base implementation of start...
            base.Start();

            // TODO: OTHER LOGIC HERE
        }

        /// <summary>
        /// Virtual overridable method inherited by tiles that have "Triggers".
        /// </summary>
        public abstract void Interact();
    }
}