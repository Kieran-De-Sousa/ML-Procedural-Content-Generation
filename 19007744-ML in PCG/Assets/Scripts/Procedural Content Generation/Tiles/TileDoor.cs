// Unity

using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PCG.Tilemaps
{
    public enum DoorDirection : int
    {
        TOP    = 0,
        BOTTOM = 1,
        LEFT   = 2,
        RIGHT  = 3,
    }

    public class TileDoor : TileInteractable
    {
        [Header("Tile Door Properties")]
        [SerializeField] private TileBase doorOpen;
        [SerializeField] private DoorDirection doorDirection = DoorDirection.TOP;
        private bool isDoorOpen = true;

        // IInteractable Interface implementation
        public override void Interact()
        {
            if (isDoorOpen)
            {
                // TODO: GO THROUGH DOOR / END SIMULATION
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public DoorDirection GetDoorDirection() { return doorDirection; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="direction"></param>
        public void SetDoorDirection(DoorDirection direction) => doorDirection = direction;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool GetDoorState() { return isDoorOpen; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="open"></param>
        public void SetDoorState(bool open) => isDoorOpen = open;
    }
}