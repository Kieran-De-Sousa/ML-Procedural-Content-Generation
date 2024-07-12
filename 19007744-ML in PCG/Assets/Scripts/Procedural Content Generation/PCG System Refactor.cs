// Base
using System;
using System.Collections;
using System.Collections.Generic;

// Unity
using UnityEngine;
using UnityEngine.Tilemaps;

// PCG
using PCG.Tilemaps;

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
    public class PCGSystemRefactor : Singleton<PCGSystemRefactor>
    {
        [SerializeField] private TilemapSystem tilemapSystem;

        [Header("Tilemap Size")]
        [Range(13, 100)]
        public int width = 17;
        [Range(7, 100)]
        public int height = 9;

        public RoomData roomData;

        [Space]

        [Header("PCG Generation Method")]
        public GenerationMethod generation = GenerationMethod.NONE;

        private InputScheme _inputScheme;

        protected override void Awake()
        {
            base.Awake();

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

                    // TODO
                    roomData = PCGMethodsRefactor.GenerateRoom(roomData, tilemapSystem.tilemapData, default);
                    foreach (Tilemap tilemap in tilemapSystem.tilemapData.allTilemaps)
                    {
                        tilemap.RefreshAllTiles();
                    }
                    break;
                }

                default:
                {
                    break;
                }
            }

            SpawnPlayer();

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
        }

        /// <summary>
        /// Move player position to centre of room.
        /// </summary>
        public void SpawnPlayer()
        {
            ML.MLAgent player = HelperUtilities.FindParentOrChildWithComponent<ML.MLAgent>(transform);
            var origin = tilemapSystem.tilemapData.collidable.GetCellCenterWorld(HelperUtilities.GetCenterTilePosition(tilemapSystem.tilemapData.collidable));

            player.transform.position = new Vector3(origin.x, origin.y, player.transform.position.z);
        }
    }
}