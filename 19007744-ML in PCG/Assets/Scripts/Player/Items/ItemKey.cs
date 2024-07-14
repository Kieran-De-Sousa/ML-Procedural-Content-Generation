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
        // This could be a dictionary entry, so interact doesn't need to be declared in all 3 items.
        player.GetPlayerInventory().AddKey(value);
        player.AddReward(player.GetPlayerInventory().GetKeyRewardValue());
        ownerTilemap.SetTile(tilePosition, null);

        IsInteracted = true;
        IsInteractable = false;
    }
}
