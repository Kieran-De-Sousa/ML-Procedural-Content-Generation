using System.Collections;
using System.Collections.Generic;

using UnityEngine;

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
        player.AddCoin(value);
        ownerTilemap.SetTile(tilePosition, null);
    }
}
