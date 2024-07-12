// Unity
using UnityEngine;
using UnityEngine.Tilemaps;

// Helper
using Utilities;

// Sub-namespace for tilemap-related utilities.
namespace PCG.Tilemaps
{
    /// <summary>
    /// The different directions/positions the door can be.
    /// </summary>
    public enum DoorDirection : int
    {
        TOP    = 0,
        BOTTOM = 1,
        LEFT   = 2,
        RIGHT  = 3,
    }

    /// <summary>
    /// Derived class of <c>TileInteractable</c> which are door tiles that are interacted with upon trigger collision.
    /// </summary>
    public class TileDoor : TileInteractable
    {
        [Header("Tile Door Properties")]
        [SerializeField] private TileBase doorOpen;
        [SerializeField] private DoorDirection doorDirection = DoorDirection.TOP;
        [SerializeField] private float doorReward = 10.0f;
        private bool isDoorOpen = true;

        // IInteractable Interface implementation
        public override void Interact()
        {
            if (isDoorOpen)
            {
                // Reward Agent for interacting with door.
                player.RewardPlayer(doorReward);

                IsInteracted = true;
                IsInteractable = false;

                // End this simulation / episode.
                Simulation simulation = HelperUtilities.FindParentOrChildWithComponent<Simulation>(ownerTilemap.transform);
                simulation.ResetSimulation();
            }
        }

        /// <summary>
        /// Gets the direction the door is facing.
        /// </summary>
        /// <returns>Direction of the door.</returns>
        public DoorDirection GetDoorDirection() { return doorDirection; }

        /// <summary>
        /// Sets the direction the door is facing.
        /// </summary>
        /// <param name="direction">Direction to set for door.</param>
        public void SetDoorDirection(DoorDirection direction) => doorDirection = direction;

        /// <summary>
        /// Gets the current state of the door (open or closed).
        /// </summary>
        /// <returns><c>true</c> if the door is open, otherwise <c>false</c></returns>
        public bool GetDoorState() { return isDoorOpen; }

        /// <summary>
        /// Sets the state of the door (open or closed).
        /// </summary>
        /// <param name="open">State to set for door.</param>
        public void SetDoorState(bool open) => isDoorOpen = open;
    }
}