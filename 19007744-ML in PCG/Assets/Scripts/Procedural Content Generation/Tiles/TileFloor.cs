// Sub-namespace for tilemap-related utilities.
namespace PCG.Tilemaps
{
    /// <summary>
    /// Floor tiles that cannot be collided or triggered (decorative).
    /// </summary>
    public class TileFloor : Tile
    {
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
    }
}