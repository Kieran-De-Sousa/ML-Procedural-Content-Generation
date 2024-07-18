// Procedural Content Generation
using PCG.Tilemaps;

/// <summary>
/// Different types of item this tile can be.
/// </summary>
public enum ItemType : int
{
    COIN = 1,
    BOMB = 2,
    KEY  = 3,
}

/// <summary>
/// Derived class of <c>TileInteractable</c> which are item tiles that are interacted with upon trigger collision.
/// </summary>
public abstract class Item : TileInteractable
{
    /// The items type.
    public ItemType itemType;
    /// Value of the item picked up.
    public int value;

    protected Item()
    {
        // Set base class properties...
        tileType = TileType.ITEM;
    }
}
