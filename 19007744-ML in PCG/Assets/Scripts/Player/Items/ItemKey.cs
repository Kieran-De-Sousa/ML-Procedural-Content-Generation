using System.Collections;
using System.Collections.Generic;

using UnityEngine;

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
        player.AddKey(value);
        ownerTilemap.SetTile(tilePosition, null);

        IsInteracted = true;
    }
}
