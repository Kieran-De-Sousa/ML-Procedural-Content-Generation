/// <summary>
/// Derived class of <c>Item</c> which are bomb tiles that are interacted with upon trigger collision.
/// </summary>
public class ItemBomb : Item
{
    public ItemBomb()
    {
        // Set base class properties...
        itemType = ItemType.BOMB;
    }

    /// <summary>
    /// Bomb was touched by a player. Pick up bomb and add to their inventory, then delete bomb.
    /// </summary>
    public override void Interact()
    {
        player.GetPlayerInventory().AddCoin(value);
        ownerTilemap.SetTile(tilePosition, null);

        IsInteracted = true;
        IsInteractable = false;
    }
}
