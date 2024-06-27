// Base
using System.Collections;
using System.Collections.Generic;

// Unity
using UnityEngine;

// Procedural Content Generation
using PCG.Tilemaps;

public enum ItemType : int
{
    COIN = 1,
    BOMB = 2,
    KEY  = 3,
}

public abstract class Item : TileInteractable
{
    /// The items type.
    public ItemType itemType;
    /// Value of the item picked up.
    public int value;

    protected Item()
    {
        tileType = TileType.ITEM;
    }
}
