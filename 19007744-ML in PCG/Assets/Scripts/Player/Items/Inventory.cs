// Base
using System;
using System.Collections;
using System.Collections.Generic;

// Unity
using UnityEngine;

/// <summary>
/// Class managing the player's obtained items of coins, bombs, and keys.
/// </summary>
public class Inventory : MonoBehaviour
{
    public Items inventory;

    /// <summary>
    /// Adds value to inventory coin count.
    /// </summary>
    /// <param name="coinValue">Value of coins to add.</param>
    public void AddCoin(int coinValue) => inventory.coins += coinValue;
    /// <summary>
    /// Gets the current number of coins in inventory.
    /// </summary>
    /// <returns>The value of coins in inventory.</returns>
    public int GetInventoryCoins() { return inventory.coins; }

    /// <summary>
    /// Adds value to inventory bomb count.
    /// </summary>
    /// <param name="bombValue">Value of bombs to add.</param>
    public void AddBomb(int bombValue) => inventory.bombs += bombValue;
    /// <summary>
    /// Gets the current number of bombs in inventory.
    /// </summary>
    /// <returns>The value of bombs in inventory.</returns>
    public int GetInventoryBombs() { return inventory.bombs; }

    /// <summary>
    /// Adds value to inventory key count.
    /// </summary>
    /// <param name="keyValue">Value of keys to add.</param>
    public void AddKey(int keyValue) => inventory.keys += keyValue;
    /// <summary>
    /// Gets the current number of keys in inventory.
    /// </summary>
    /// <returns>The value of keys in inventory.</returns>
    public int GetInventoryKeys() { return inventory.keys; }

    /// <summary>
    /// Get the ML Agent reward value for getting a coin.
    /// </summary>
    /// <returns></returns>
    public int GetCoinRewardValue() { return 1; }
    /// <summary>
    /// Get the ML Agent reward value for getting a bomb.
    /// </summary>
    /// <returns></returns>
    public int GetBombRewardValue() { return 1; }
    /// <summary>
    /// Get the ML Agent reward value for getting a key.
    /// </summary>
    /// <returns></returns>
    public int GetKeyRewardValue() { return 1; }
}

/// <summary>
/// Data container holding value of items in the inventory.
/// </summary>
[Serializable]
public struct Items
{
    public int coins;
    public int bombs;
    public int keys;
}
