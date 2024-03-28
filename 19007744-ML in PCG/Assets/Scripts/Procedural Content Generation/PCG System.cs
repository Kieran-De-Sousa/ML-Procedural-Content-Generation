// Base
using System;
using System.Collections;
using System.Collections.Generic;

// Unity
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PCG
{
    public class PCGSystem : MonoBehaviour
    {
        [Header("Tilemaps")]
        public List<Tilemap> tilemaps;

        public Tilemap tilemap;
        public Tilemap collidable;
        public Tilemap entities;

        [Space]

        [Header("Tiles")]
        public TileBase bases;
        public TileBase pit;

        [Space]

        [Header("Tilemap Size")]
        public int width = 13;
        public int height = 7;

        [Space]

        [Header("PCG Generation Method")]
        public GenerationMethod generation = GenerationMethod.NONE;

        private InputScheme _inputScheme;

        /// <summary>
        /// Add the tilemaps to the list whenever we change the PCG System in Editor.
        /// </summary>
        private void OnValidate()
        {
            AddTilemapToList(tilemap);
            AddTilemapToList(collidable);
            AddTilemapToList(entities);
        }

        /// <summary>
        /// Adds the tilemap to the list of <c>tilemaps</c> if not already.
        /// </summary>
        /// <param name="tilemapToAdd">Tilemap to be added to list.</param>
        private void AddTilemapToList(Tilemap tilemapToAdd)
        {
            if (tilemapToAdd != null && !tilemaps.Contains(tilemapToAdd))
            {
                tilemaps.Add(tilemapToAdd);
            }
        }

        /// Start is called before the first frame update
        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            InitialiseInput();
        }

        /// <summary>
        ///
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
                // Spawn the player here.
            }
        }

        /// <summary>
        ///
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

                default:
                {
                    break;
                }
            }

            PCGMethods.RenderRoomOffset(map, tilemap, collidable, bases, pit, offset);
        }

        /// <summary>
        ///
        /// </summary>
        public void ClearRoom()
        {
            foreach (var map in tilemaps)
            {
                map.ClearAllTiles();
            }
        }
    }
}