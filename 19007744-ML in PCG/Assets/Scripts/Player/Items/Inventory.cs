using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Items inventory;

    public void AddCoin(int coinValue) => inventory.coins += coinValue;
    public void AddBomb(int bombValue) => inventory.bombs += bombValue;
    public void AddKey(int keyValue) => inventory.keys += keyValue;

    public int GetCoinRewardValue()
    {
        return 1;
    }
}

[Serializable]
public struct Items
{
    public int coins;
    public int bombs;
    public int keys;
}
