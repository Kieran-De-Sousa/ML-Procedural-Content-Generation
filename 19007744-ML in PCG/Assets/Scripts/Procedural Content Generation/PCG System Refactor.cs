// Base
using System;
using System.Collections;
using System.Collections.Generic;

// Unity
using UnityEngine;
using UnityEngine.Tilemaps;

// PCG
using PCG.Tilemaps;
using UnityEditor;

// Helper
using Utilities;

// Ambiguous reference prevention (shares name with UnityEngine.Tilemaps.)
using Tile = PCG.Tilemaps.Tile;

// Root namespace for all Procedural Content Generation-related utilities.
namespace PCG
{
    /// <summary>
    ///
    /// </summary>
    public class PCGSystemRefactor : ManagerSystem
    {
        [SerializeField] private TilemapSystem tilemapSystem;

        [Header("Tilemap Size")]
        [Range(13, 100)]
        public int width = 17;
        [Range(7, 100)]
        public int height = 9;

        public RoomData roomData;
        public Vector3 roomOrigin;

        [Space]

        [Header("PCG Generation Method")]
        public GenerationMethod generation = GenerationMethod.NONE;

        private InputScheme _inputScheme;

        protected void Awake()
        {
            roomData = RoomData.GenerateRoom(width, height);
        }

        /// Start is called before the first frame update
        private void Start()
        {
            InitialiseInput();
        }

        /// <summary>
        /// Initalise New Unity Input System
        /// </summary>
        private void InitialiseInput()
        {
            _inputScheme = new InputScheme();
            _inputScheme.PCG.Enable();
        }

        /// Update is called once per frame
        private void Update()
        {
            PollInputs();
        }

        /// <summary>
        /// Check for all inputs in PCG Action Map.
        /// </summary>
        private void PollInputs()
        {
            InputScheme.PCGActions actions = _inputScheme.PCG;

            if (actions.Generate.WasPressedThisFrame())
            {
                GenerateRoom();
            }

            if (actions.Clear.WasPressedThisFrame())
            {
                ClearRoom();
            }

            if (actions.SpawnPlayer.WasPressedThisFrame())
            {
                SpawnPlayer();
            }
        }

        /// <summary>
        /// Regenerate roomData, clear room, generate new room, and move player and camera.
        /// </summary>
        public override void ResetSystem()
        {
            roomData = RoomData.GenerateRoom(width, height);

            ClearRoom();
            GenerateRoom();
            SpawnPlayer();
            MoveCamera();
        }

        /// <summary>
        /// Generate room level based on:
        /// - Seed (Current time).
        /// - Room width and height.
        /// - Selected generation method.
        /// </summary>
        public void GenerateRoom()
        {
            ClearRoom();

            int[,] map = new int[width, height];
            float seed = Time.time;
            Vector2Int offset = new Vector2Int((width / 2) * -1, (height / 2) * -1);

            switch (generation)
            {
                case GenerationMethod.NONE:
                {
                    break;
                }

                case GenerationMethod.RANDOM:
                {
                    map = PCGMethods.GenerateMap(width, height);
                    map = PCGMethods.RandomGeneration(map, seed);
                    break;
                }

                case GenerationMethod.PERLINNOISE:
                {
                    map = PCGMethods.GenerateMap(width, height);
                    map = PCGMethods.PerlinNoise(map, seed);

                    break;
                }

                case GenerationMethod.ASTAR:
                {
                    roomData.tilemap = PCGMethodsRefactor.GenerateTileMap(width, height);
                    roomData = PCGMethodsRefactor.GenerateRoom(roomData, tilemapSystem.tilemapData, default);

                    for (int x = 0; x < roomData.tilemap.GetUpperBound(0); ++x)
                    {
                        // Loop through the height of the map
                        for (int y = 0; y < roomData.tilemap.GetUpperBound(1); ++y)
                        {
                            roomData.tilemap[x,y].gameObject.transform.parent = tilemapSystem.instantiatedTilesParent;
                        }
                    }

                    break;
                }

                default:
                {
                    break;
                }
            }

            // Find centre tile in tilemap, then get the centre position of that centre tile found.
            roomOrigin = tilemapSystem.tilemapData.collidable.GetCellCenterWorld(HelperUtilities.GetCenterTilePosition(tilemapSystem.tilemapData.collidable));

            SpawnPlayer();
            MoveCamera();

            // TODO
            /*PCGMethods.RenderRoomOffset(map, tilemapSystem.tilemap,
                tilemapSystem.collidable,
                tilemapSystem.bases, tilemapSystem.pit, offset);*/
        }

        /// <summary>
        /// Clear all tiles in all tilemaps.
        /// </summary>
        public void ClearRoom()
        {
            foreach (var map in tilemapSystem.tilemapData.allTilemaps)
            {
                map.ClearAllTiles();
            }

            if (roomData.tilemap != null)
            {
                for (int x = 0; x < roomData.tilemap.GetUpperBound(0); ++x)
                {
                    // Loop through the height of the map
                    for (int y = 0; y < roomData.tilemap.GetUpperBound(1); ++y)
                    {
                        if (roomData.tilemap[x, y] != null)
                        {
                            Destroy(roomData.tilemap[x, y].gameObject);
                        }
                    }
                }
            }

            // Clear all instantiated tiles from parent empty.
            HelperUtilities.DestroyAllChildren(tilemapSystem.instantiatedTilesParent.gameObject);
        }

        /// <summary>
        /// Move player position to centre of room.
        /// </summary>
        public void SpawnPlayer()
        {
            ML.MLAgent player = HelperUtilities.FindParentOrChildWithComponent<ML.MLAgent>(transform);
            player.transform.position = new Vector3(roomOrigin.x, roomOrigin.y, player.transform.position.z);
        }

        /// <summary>
        /// Move the camera to look at the centre of room.
        /// </summary>
        public void MoveCamera()
        {
            Camera camera = HelperUtilities.FindParentOrChildWithComponent<Camera>(transform);
            camera.transform.position = new Vector3(roomOrigin.x, roomOrigin.y, camera.transform.position.z);
        }
    }
}