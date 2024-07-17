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
using UnityEditor;

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

    [Header("Engagement Prefabing")]
    public bool generateEngagementPrefabs = false;
    [Tooltip("Set to this GameObject")]
    public GameObject simulationToClone;
    private GameObject clone;
    public string savePath = "Assets/Prefabs/Rooms/";

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
        tilemapSystem.ResetSystem();
        pcgSystemRefactor.ResetSystem();

        // Check if this simulation wants to create prefabs of engaging room layouts
        if (generateEngagementPrefabs)
        {
            // Turn this off so that our clone inherits this member off at INSTANTIATION
            generateEngagementPrefabs = false;
            if (clone != null)
            {
                Destroy(clone);
            }

            // Current simulation is taken as a reference for cloning.
            simulationToClone = gameObject;

            // We create a new GameObject of the simulation from the clone to save
            // the reference AWAY from the original prefab/simulation.
            clone = Instantiate(simulationToClone);
            clone.SetActive(false);

            var simulationCopyComponent = clone.GetComponent<Simulation>();
            simulationCopyComponent.generateEngagementPrefabs = false;
            simulationCopyComponent.pcgSystemRefactor.roomData = pcgSystemRefactor.roomData;
            simulationCopyComponent.tilemapSystem.tilemapData = tilemapSystem.tilemapData;
            simulationCopyComponent.tilemapSystem.highestEngagementRoom = pcgSystemRefactor.roomData;

            // Set this back, as it prevents clones from endlessly generating copies of themselves.
            generateEngagementPrefabs = true;
        }
    }

    public IEnumerator SaveRoomPrefab(float engagementScore)
    {
        // Ensure the save path exists.
        if (!System.IO.Directory.Exists(savePath))
        {
            System.IO.Directory.CreateDirectory(savePath);
        }

        // Create a unique file name for the prefab.
        string prefabName = engagementScore + "_EngagementRoom" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".prefab";
        string fullPath = savePath + prefabName;

        // Clone the instantiated clone object in the scene.
        GameObject simulationCopy = Instantiate(clone);
        simulationCopy.gameObject.SetActive(false);

        // Wait a frame to reduce Unity ML Agent errors...
        yield return new WaitForEndOfFrame();

        // Tag to be created prefabs that they will use the pre-generated room they started with.
        var simulationCopyComponent = simulationCopy.GetComponent<Simulation>();
        simulationCopyComponent.pcgSystemRefactor.preGenerated = true;

        // Save the clone as a prefab.
#if UNITY_EDITOR
        PrefabUtility.SaveAsPrefabAsset(simulationCopy, fullPath);
#endif

        // Destroy the clone after saving.
        Destroy(simulationCopy);
        Debug.Log($"Simulation prefab saved as: {fullPath}");
    }
}