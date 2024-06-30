// Base
using System;
using System.Collections;
using System.Collections.Generic;

// Unity
using UnityEngine;
using UnityEngine.Tilemaps;

// Sub-namespace for tilemap-related utilities.
namespace PCG.Tilemaps
{
    /// <summary>
    ///
    /// </summary>
    public class TilemapSystem : Singleton<TilemapSystem>
    {
        public TilemapData tilemapData;

        [Header("All Tilemaps")]
        public List<Tilemap> tilemaps;

        [Header("Decoration Tilemap")]
        public Tilemap tilemap;
        [Header("Tiles")]
        public List<TileFloor> tileFloors;

        [Space]

        [Header("Collidable Tilemap")]
        [Tooltip("Tilemap Contains Rigidbody, Composite collider, tilemap collider (NOT TRIGGERS)")]
        public Tilemap collidable;
        [Header("Colliders")]
        public List<TilePit> tilePits;
        public List<TileWall> tileWalls;

        [Space]

        [Header("Trigger Tilemap")]
        [Tooltip("Tilemap Contains Rigidbody, Composite collider, tilemap collider (TRIGGERS)")]
        public Tilemap entities;
        [Header("Entities")]
        public List<TileDoor> tileDoors;
        public List<Item> tileItems;

        protected override void Awake()
        {
            base.Awake();

            // NOTE
            tilemapData.floor = tilemap;
            tilemapData.collidable = collidable;
            tilemapData.trigger = entities;
        }

        /// <summary>
        /// Add the tilemaps to the list whenever we change the PCG System in Editor.
        /// </summary>
        private void OnValidate()
        {
            AddTilemapToList(tilemap);
            AddTilemapToList(collidable);
            AddTilemapToList(entities);

            tilemapData.floor = tilemap;
            tilemapData.collidable = collidable;
            tilemapData.trigger = entities;
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
    }

    [Serializable]
    public class TilemapData
    {
        public List<Tilemap> allTilemaps = new();

        public Tilemap floor = new();
        public Tilemap collidable = new();
        public Tilemap trigger = new();

        public List<Tile> tiles = new();

        public TilemapData()
        {
            allTilemaps.AddRange(new List<Tilemap>
            {
                floor,
                collidable,
                trigger
            });
        }
    }

    [Serializable]
    public struct RoomData
    {
        public Tile[,] tilemap;

        public int RoomWidth;
        public int RoomHeight;

        public (int x, int y) RoomCentre;

        public (int x, int y) TopDoor;
        public (int x, int y) BottomDoor;
        public (int x, int y) LeftDoor;
        public (int x, int y) RightDoor;

        /// <summary>
        /// Generates a RoomData instance with specified width and height.
        /// </summary>
        /// <param name="width">Width of the room.</param>
        /// <param name="height">Height of the room.</param>
        /// <returns>RoomData instance with centre and door positions.</returns>
        public static RoomData GenerateRoom(int width, int height)
        {
            RoomData roomData = new RoomData
            {
                RoomWidth = width,
                RoomHeight = height,
                RoomCentre = (width / 2, height / 2),
                TopDoor = (width / 2, height - 1),
                BottomDoor = (width / 2, 0),
                LeftDoor = (0, height / 2),
                RightDoor = (width - 1, height / 2)
            };

            return roomData;
        }
    }
}