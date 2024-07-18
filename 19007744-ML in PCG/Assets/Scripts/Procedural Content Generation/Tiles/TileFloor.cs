// Unity
using UnityEngine;

// Sub-namespace for tilemap-related utilities.
namespace PCG.Tilemaps
{
    /// <summary>
    /// Floor tiles that cannot be collided or triggered (decorative).
    /// </summary>
    public class TileFloor : Tile
    {
        [Tooltip("Ideally give a very small reward to agents for going on a floor tile.")]
        [SerializeField] private float playerReward = 0.2f;
        [Tooltip("A slightly higher reward than player reward for measuring engagement with a level.")]
        [SerializeField] private float explorationReward = 0.5f;
        private bool IsExplored = false;
        public TileFloor()
        {
            // Set base class properties...
            tileType = TileType.FLOOR;
        }

        protected override void Start()
        {
            // Call base implementation of start...
            base.Start();
        }

        public void Explore()
        {
            // This could be a dictionary entry, so interact doesn't need to be declared in all 3 items.
            player.RewardPlayer(playerReward);
            player.RewardExploration(explorationReward);
            IsExplored = true;
        }

        public bool GetIsExplored() { return IsExplored; }
    }
}