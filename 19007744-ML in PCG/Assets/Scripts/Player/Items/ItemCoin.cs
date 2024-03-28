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
    /// Coin was touched by a player. Pick up coin and add to their inventory.
    /// </summary>
    public override void Interact()
    {
        // TODO: Add coin value to players inventory

        // TODO: Delete this coin
    }
}
