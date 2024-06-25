// Base
using System;
using System.Collections;
using System.Collections.Generic;

// Unity
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Items inventory;

    public void AddCoin(int coinValue) => inventory.coins += coinValue;
    public int GetInventoryCoins() { return inventory.coins; }

    public void AddBomb(int bombValue) => inventory.bombs += bombValue;
    public int GetInventoryBombs() { return inventory.bombs; }

    public void AddKey(int keyValue) => inventory.keys += keyValue;
    public int GetInventoryKeys() { return inventory.keys; }

    public int GetCoinRewardValue() { return 1; }
    public int GetBombRewardValue() { return 1; }
    public int GetKeyRewardValue() { return 1; }
}

[Serializable]
public struct Items
{
    public int coins;
    public int bombs;
    public int keys;
}
