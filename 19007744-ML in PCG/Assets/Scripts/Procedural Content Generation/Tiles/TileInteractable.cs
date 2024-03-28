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
        /// <summary>
        /// Virtual overridable method inherited by tiles that have "Triggers".
        /// </summary>
        public abstract void Interact();
    }
}