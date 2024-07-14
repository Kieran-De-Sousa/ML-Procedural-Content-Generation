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
    public MLSystem mlSystem;
    [Space]

    public PCGSystemRefactor pcgSystemRefactor;
    [Space]

    public TilemapSystem tilemapSystem;
    [Space]
    [Space]

    public PCGSystem pcgSystem;

    private void Awake()
    {
        ResetSimulation();
    }

    public void ResetSimulation()
    {
        Debug.Log($"Simulation Ending: {this}");

        pcgSystemRefactor.ResetSystem();
        mlSystem.ResetSystem();
    }
}