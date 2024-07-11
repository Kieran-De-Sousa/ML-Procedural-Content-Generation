/// <summary>
/// Derived class of <c>Item</c> which are key tiles that are interacted with upon trigger collision.
/// </summary>
public class ItemKey : Item
{
    public ItemKey()
    {
        // Set base class properties...
        itemType = ItemType.KEY;
    }

    /// <summary>
    /// Key was touched by a player. Pick up key and add to their inventory, then delete key.
    /// </summary>
    public override void Interact()
    {
        player.GetPlayerInventory().AddCoin(value);
        ownerTilemap.SetTile(tilePosition, null);

        IsInteracted = true;
    }
}
