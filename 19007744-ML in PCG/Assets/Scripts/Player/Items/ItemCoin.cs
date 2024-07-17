using UnityEngine;

/// <summary>
/// Derived class of <c>Item</c> which are coin tiles that are interacted with upon trigger collision.
/// </summary>
public class ItemCoin : Item
{
    public ItemCoin()
    {
        // Set base class properties...
        itemType = ItemType.COIN;
    }

    /// <summary>
    /// Coin was touched by a player. Pick up coin and add to their inventory, then delete coin.
    /// </summary>
    public override void Interact()
    {
        // This could be a dictionary entry, so interact doesn't need to be declared in all 3 items.
        player.GetPlayerInventory().AddCoin(value);
        player.RewardPlayer(player.GetPlayerInventory().GetCoinRewardValue());
        player.engagement.AddItemPickup(value);

        ownerTilemap.SetTile(tilePosition, null);
        IsInteracted = true;
        IsInteractable = false;
    }
}
