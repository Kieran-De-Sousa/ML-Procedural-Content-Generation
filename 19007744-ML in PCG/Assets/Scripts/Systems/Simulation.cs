// Base
using System;
using System.Collections;
using System.Collections.Generic;

// Unity
using UnityEngine;

// Machine Learning
using ML;
// Procedural Content Generation
using PCG;
using PCG.Tilemaps;

/// <summary>
/// Singleton that overviews all systems present in each room simulation.
/// </summary>
public class Simulation : MonoBehaviour
{
    private List<ManagerSystem> managerSystems;

    public MLSystem mlSystem;
    [Space]

    public PCGSystemRefactor pcgSystemRefactor;
    [Space]

    public TilemapSystem tilemapSystem;
    [Space]
    [Space]

    [Obsolete("pcgSystem is obsolete and has been replaced by pcgSystemRefactor.")]
    public PCGSystem pcgSystem;

    /*private void OnValidate()
    {
        managerSystems.AddRange(new List<ManagerSystem>
        {
            mlSystem,
            pcgSystemRefactor,
            tilemapSystem
        });
    }*/

    private void Awake()
    {
        ResetSimulation();
    }

    public void ResetSimulation()
    {
        Debug.Log($"Simulation Ending: {this}");

        mlSystem.ResetSystem();
        pcgSystemRefactor.ResetSystem();
    }
}