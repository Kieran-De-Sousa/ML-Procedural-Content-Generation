// Unity
using UnityEngine;

// Sub-namespace for tilemap-related utilities.
namespace PCG.Tilemaps
{
    /// <summary>
    /// Pit tiles that can be collided with (<c>ICollidable</c>).
    /// </summary>
    public class TilePit : TileCollidable
    {
        [Tooltip("Ideally make this a minus number as we want to negatively reward agents for hitting obstacles.")]
        [SerializeField] private float reward = -1f;

        public TilePit()
        {
            // Set base class properties...
            tileType = TileType.PIT;
        }

        protected override void Start()
        {
            // Call base implementation of start...
            base.Start();
        }

        public override void Collide()
        {
            player.AddReward(reward);
            IsCollided = true;
        }
    }
}