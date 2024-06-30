// Base
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
public class Simulation : Singleton<Simulation>
{
    public MLSystem mlSystem;
    public PCGSystemRefactor pcgSystemRefactor;
    public TilemapSystem tilemapSystem;

    public PCGSystem pcgSystem;
}