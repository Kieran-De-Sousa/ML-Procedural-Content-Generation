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
    public Items inventory = new();

    public float coinRewardValue = 3;
    public float bombRewardValue = 6;
    public float keyRewardValue = 6;

    /// <summary>
    /// Reset the players inventory by creating a new, empty one.
    /// </summary>
    public void ResetInventory() => inventory = new Items();

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
    public float GetCoinRewardValue() { return coinRewardValue; }
    /// <summary>
    /// Get the ML Agent reward value for getting a bomb.
    /// </summary>
    /// <returns></returns>
    public float GetBombRewardValue() { return bombRewardValue; }
    /// <summary>
    /// Get the ML Agent reward value for getting a key.
    /// </summary>
    /// <returns></returns>
    public float GetKeyRewardValue() { return keyRewardValue; }
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
