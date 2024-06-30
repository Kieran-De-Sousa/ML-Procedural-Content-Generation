// Sub-namespace for tilemap-related utilities.
namespace PCG.Tilemaps
{
    /// <summary>
    /// Pit tiles that can be collided with (<c>ICollidable</c>).
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
        }
    }
}