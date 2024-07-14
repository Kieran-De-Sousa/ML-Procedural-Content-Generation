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
        // This could be a dictionary entry, so interact doesn't need to be declared in all 3 items.
        player.GetPlayerInventory().AddBomb(value);
        player.AddReward(player.GetPlayerInventory().GetBombRewardValue());
        ownerTilemap.SetTile(tilePosition, null);

        IsInteracted = true;
        IsInteractable = false;
    }
}
