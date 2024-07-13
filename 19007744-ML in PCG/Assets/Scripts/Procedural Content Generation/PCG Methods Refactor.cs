// Base
using System.Collections;
using System.Collections.Generic;
using PCG.Tilemaps;

// Unity
using UnityEngine;
using UnityEngine.Tilemaps;

// Ambiguous reference prevention (shares name with UnityEngine.Tilemaps.)
using Tile = PCG.Tilemaps.Tile;

namespace PCG
{
    /// <summary>
    /// The different generation methods of Procedural Content Generation (PCG).
    /// </summary>
    public enum GenerationMethod
    {
        NONE,
        RANDOM,
        PERLINNOISE,
        ASTAR,
    }

    /// <summary>
    /// A collection of different methods for Procedural Content Generation (PCG).
    /// </summary>
    public class PCGMethodsRefactor : MonoBehaviour
    {
        /// <summary>
        /// Generates a new int map array with the passed width and height.
        /// </summary>
        /// <param name="width">Width of the map.</param>
        /// <param name="height">Height of the map.</param>
        /// <returns>2D array of generated map.</returns>
        public static int[,] GenerateIntMap(int width, int height)
        {
            int[,] map = new int[width + 1, height + 1]; // +1 = Add missing wall
            return map;
        }

        /// <summary>
        /// Generates a new Tile map array with the passed width and height.
        /// </summary>
        /// <param name="width">Width of the map.</param>
        /// <param name="height">Height of the map.</param>
        /// <returns>2D array of generated map.</returns>
        public static Tile[,] GenerateTileMap(int width, int height)
        {
            Tile[,] map = new Tile[width + 1, height + 1]; // +1 = Add missing wall
            return map;
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <param name="direction"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static RoomData AssignDoor(RoomData data, DoorDirection direction, int x, int y)
        {
            switch (direction)
            {
                case DoorDirection.TOP:
                {
                    data.TopDoor = (x, y);
                    break;
                }
                case DoorDirection.BOTTOM:
                {
                    data.BottomDoor = (x, y);
                    break;
                }
                case DoorDirection.LEFT:
                {
                    data.LeftDoor = (x, y);
                    break;
                }
                case DoorDirection.RIGHT:
                {
                    data.RightDoor = (x, y);
                    break;
                }
                default:
                {
                    break;
                }
            }

            return data;
        }

        /// <summary>
        /// Generate a door tile facing the provided direction.
        /// </summary>
        /// <param name="x">X position of the tile on map.</param>
        /// <param name="y">Y position of the tile on map.</param>
        /// <param name="map">2D tile array representing map.</param>
        /// <param name="tilemapData">Data of available tiles.</param>
        /// <param name="direction">Direction the door will face.</param>
        /// <returns>Generated door tile, otherwise, <c>null</c></returns>
        public static Tile GenerateDoors(int x, int y, Tile[,] map,
            TilemapData tilemapData, DoorDirection direction)
        {
            foreach (var tile in tilemapData.tiles)
            {
                if (tile is TileDoor door)
                {
                    if (door.GetDoorDirection() == direction)
                    {
                        return Instantiate(door);
                    }
                }
            }

            Debug.LogError("DOOR TILE COULD NOT BE FOUND IN TILEMAPDATA!");
            return null;
        }


        /// <summary>
        /// Generate a wall tile facing the provided direction.
        /// </summary>
        /// <param name="x">X position of the tile on map.</param>
        /// <param name="y">Y position of the tile on map.</param>
        /// <param name="map">2D tile array representing map.</param>
        /// <param name="tilemapData">Data of available tiles.</param>
        /// <param name="direction">Direction the wall will face.</param>
        /// <returns>Generated wall tile, otherwise, <c>null</c></returns>
        public static Tile GenerateWalls(int x, int y, Tile[,] map,
            TilemapData tilemapData, WallDirection direction)
        {
            foreach (var tile in tilemapData.tiles)
            {
                if (tile is TileWall wall)
                {
                    if (wall.GetWallDirection() == direction)
                    {
                        return Instantiate(wall);
                    }
                }
            }

            Debug.LogError("WALL TILE COULD NOT BE FOUND IN TILEMAPDATA!");
            return null;
        }

        /// <summary>
        /// Generate a floor tile.
        /// </summary>
        /// <param name="x">X position of the tile on map.</param>
        /// <param name="y">Y position of the tile on map.</param>
        /// <param name="map">2D tile array representing map.</param>
        /// <param name="tilemapData">Data of available tiles.</param>
        /// <returns>Generated floor tile, otherwise, <c>null</c></returns>
        public static Tile GenerateFloor(int x, int y, Tile[,] map, TilemapData tilemapData)
        {
            foreach (var tile in tilemapData.tiles)
            {
                if (tile is TileFloor floor)
                {
                    return Instantiate(floor);
                }
            }

            Debug.LogError("FLOOR TILE COULD NOT BE FOUND IN TILEMAPDATA!");
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="map"></param>
        /// <param name="tilemapData"></param>
        /// <returns></returns>
        public static Tile GeneratePit(int x, int y, Tile[,] map, TilemapData tilemapData)
        {
            foreach (var tile in tilemapData.tiles)
            {
                if (tile is TilePit pit)
                {
                    return Instantiate(pit);
                }
            }

            Debug.LogError("PIT TILE COULD NOT BE FOUND IN TILEMAPDATA!");
            return null;
        }

        public static Tile GenerateCoin(int x, int y, Tile[,] map, TilemapData tilemapData)
        {
            foreach (var tile in tilemapData.tiles)
            {
                if (tile is ItemCoin coin)
                {
                    return Instantiate(coin);
                }
            }

            Debug.LogError("COIN ITEM COULD NOT BE FOUND IN TILEMAPDATA!");
            return null;
        }

        public static Tile GenerateKey(int x, int y, Tile[,] map, TilemapData tilemapData)
        {
            foreach (var tile in tilemapData.tiles)
            {
                if (tile is ItemKey key)
                {
                    return Instantiate(key);
                }
            }

            Debug.LogError("KEY ITEM COULD NOT BE FOUND IN TILEMAPDATA!");
            return null;
        }

        public static Tile GenerateBomb(int x, int y, Tile[,] map, TilemapData tilemapData)
        {
            foreach (var tile in tilemapData.tiles)
            {
                if (tile is ItemBomb bomb)
                {
                    return Instantiate(bomb);
                }
            }

            Debug.LogError("BOMB ITEM COULD NOT BE FOUND IN TILEMAPDATA!");
            return null;
        }

        /// <summary>
        /// Checks if the position on the map is the location of a door.
        /// </summary>
        /// <param name="x">X position of tile.</param>
        /// <param name="y">Y position of tile.</param>
        /// <param name="map">2D int array representing the map.</param>
        /// <returns><c>true</c> if the position is a door location, otherwise; <c>false</c>.</returns>
        public static (bool, DoorDirection) CheckDoors(int x, int y, Tile[,] map)
        {
            int minX = map.GetLowerBound(0);
            int minY = map.GetLowerBound(1);
            int maxX = map.GetUpperBound(0) - 1;
            int maxY = map.GetUpperBound(1) - 1;

            // Bottom door
            if (y == minY && x == maxX / 2)
            {
                return (true, DoorDirection.BOTTOM);
            }

            // Left door
            if (x == minX && y == maxY / 2)
            {
                return (true, DoorDirection.LEFT);
            }

            // Top door
            if (y == maxY && x == maxX / 2)
            {
                return (true, DoorDirection.TOP);
            }

            // Right door
            if (x == maxX && y == maxY / 2)
            {
                return (true, DoorDirection.RIGHT);
            }

            // Return false if this was not a door location.
            return (false, default);
        }

        /// <summary>
        /// Checks if the position on the map is the location of a wall.
        /// </summary>
        /// <param name="x">X position of tile.</param>
        /// <param name="y">Y position of tile.</param>
        /// <param name="map">2D tile array representing the map.</param>
        /// <returns><c>true</c> if the position is a wall location, otherwise; <c>false</c>.</returns>
        public static (bool, WallDirection) CheckWalls(int x, int y, Tile[,] map)
        {
            // Map boundaries.
            int minX = map.GetLowerBound(0);
            int minY = map.GetLowerBound(1);
            int maxX = map.GetUpperBound(0) - 1;
            int maxY = map.GetUpperBound(1) - 1;

            // If first on width of map (LEFT SIDE).
            if (x == minX)
            {
                // If first on height, then BOTTOM LEFT corner.
                if (y == minY)
                {
                    return (true, WallDirection.BOTTOMLEFT);
                }

                // If last on height, then TOP LEFT corner.
                if (y == maxY)
                {
                    return (true, WallDirection.TOPLEFT);
                }

                return (true, WallDirection.LEFT);
            }

            // If last on width of map (RIGHT SIDE).
            if (x == maxX)
            {
                // If first on height, then BOTTOM RIGHT corner.
                if (y == minY)
                {
                    return (true, WallDirection.BOTTOMRIGHT);
                }

                // If last on height, then TOP RIGHT corner.
                if (y == maxY)
                {
                    return (true, WallDirection.TOPRIGHT);
                }

                return (true, WallDirection.RIGHT);
            }

            // If first on height of map (BOTTOM).
            if (y == minY)
            {
                return (true, WallDirection.BOTTOM);
            }

            // If last on height of map (TOP).
            if (y == maxY)
            {
                return (true, WallDirection.TOP);
            }

            // Not a wall location
            return (false, default);
        }

        /// <summary>
        /// Generate a random map based on seed.
        /// </summary>
        /// <param name="map">2D int array representing the map.</param>
        /// <param name="seed">Seed value for RNG.</param>
        /// <returns>Randomly generated int map.</returns>
        public static int[,] RandomGeneration(int[,] map, float seed)
        {
            // Set the random number generator with seed
            Random.InitState((int) seed);

            for (int x = 0; x < map.GetUpperBound(0); x++)
            {
                for (int y = 0; y < map.GetUpperBound(1); y++)
                {
                    // TODO: Use enum upperbound / lowerbound here...
                    map[x, y] = Random.Range(0, 2);
                }
            }

            return map;
        }

        /// <summary>
        /// Generates a map using perlin noise based on provided seed.
        /// </summary>
        /// <param name="map">2D int array representing the map.</param>
        /// <param name="seed">Seed value for RNG.</param>
        /// <returns>Generated int map with perlin noise.</returns>
        public static int[,] PerlinNoise(int[,] map, float seed)
        {
            int newPoint;

            // Used to reduce the position of the perlin point
            float reduction = 0.5f;
            // Create the perlin
            for (int x = 0; x < map.GetUpperBound(0); x++)
            {
                newPoint = Mathf.FloorToInt((Mathf.PerlinNoise(x, seed) - reduction) * map.GetUpperBound(1));

                // Make sure the noise starts near the halfway point of the height
                newPoint += (map.GetUpperBound(1) / 2);
                for (int y = newPoint; y >= 0; y--)
                {
                    map[x, y] = 1;
                }
            }

            return map;
        }

        /// <summary>
        /// Generates a room in the map by setting tileBases on corresponding tilemaps through assigned tile in map position.
        /// </summary>
        /// <param name="roomData">Data of tile coordinates on 2D array, and important tile positions (doors).</param>
        /// <param name="tilemapData">Data of tilemaps and available tiles.</param>
        /// <param name="offset">Optional offset to apply to the tile positions from game space.</param>
        /// <param name="seed">Seed value for RNG.</param>
        /// <returns>Data of generated room.</returns>
        public static RoomData AStarPathFindingGeneration(RoomData roomData, TilemapData tilemapData,
            Vector2Int offset = default, float seed = default)
        {
            Random.InitState((int) seed);

            // Clear current tiles when generating room.
            foreach (var tilemap in tilemapData.allTilemaps)
            {
                tilemap.ClearAllTiles();
            }

            List<Vector3Int> doorPositions = new List<Vector3Int>();
            List<Vector3Int> wallPositions = new List<Vector3Int>();

            // NOTE: In a 2D map, the below are the positions for min and max
            //                 MaxY
            // (MaxY,0) |-----------------| (MaxX,MaxY)
            //          |                 |
            //     MinX |                 | MaxX
            //          |                 |
            //    (0,0) |-----------------| (MaxX,0)
            //                 MinY

            // Loop through the width of the map
            for (int x = 0; x < roomData.tilemap.GetUpperBound(0); ++x)
            {
                // Loop through the height of the map
                for (int y = 0; y < roomData.tilemap.GetUpperBound(1); ++y)
                {
                    Tile tile = null;

                    // Check map position for door.
                    var doorResult = CheckDoors(x, y, roomData.tilemap);
                    // Check map position for wall.
                    var wallResult = CheckWalls(x, y, roomData.tilemap);

                    // If map position was a door.
                    if (doorResult.Item1)
                    {
                        tile = GenerateDoors(x, y, roomData.tilemap, tilemapData, doorResult.Item2);
                        roomData = AssignDoor(roomData, doorResult.Item2, x, y);
                        doorPositions.Add(new Vector3Int(x, y, 0));
                    }
                    // If map position was a wall.
                    else if (wallResult.Item1)
                    {
                        tile = GenerateWalls(x, y, roomData.tilemap, tilemapData, wallResult.Item2);
                        wallPositions.Add(new Vector3Int(x, y, 0));
                    }

                    if (tile != null)
                    {
                        roomData.tilemap[x, y] = tile;
                    }
                }
            }

            // Generate paths to doors using A* Pathfinding
            List<Vector3Int> floorPositions = Methods.AStarPathfinding.AStarPathfinding
                .GeneratePathsToDoors(roomData.tilemap, doorPositions);

            for (int x = 0; x < roomData.tilemap.GetUpperBound(0); ++x)
            {
                for (int y = 0; y < roomData.tilemap.GetUpperBound(1); ++y)
                {
                    int xOffset = x + offset.x;
                    int yOffset = y + offset.y;

                    Tile tile = null;
                    Vector3Int position = new Vector3Int(xOffset, yOffset, 0);

                    if (roomData.tilemap[x, y] is TileDoor || roomData.tilemap[x, y] is TileWall)
                    {
                        tile = roomData.tilemap[x, y];
                    }

                    else if (floorPositions.Contains(new Vector3Int(x, y, 0)))
                    {
                        tile = GenerateFloor(x, y, roomData.tilemap, tilemapData);
                    }

                    else
                    {
                        tile = GeneratePit(x, y, roomData.tilemap, tilemapData);
                    }

                    roomData.tilemap[x, y] = tile;
                }
            }

            return roomData;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="roomData"></param>
        /// <param name="tilemapData"></param>
        /// <param name="offset"></param>
        public static void RenderRoom(RoomData roomData, TilemapData tilemapData,
            Vector2Int offset = default)
        {
            // Create the floor as a background for the tilemap...
            //Vector3Int position = new Vector3Int(xOffset, yOffset, 0);
            //Tile floor = GenerateFloor(x, y, roomData.tilemap, tilemapData);
            //tilemapData.floor.SetTile(position, floor.GetTileBase());

            // NOTE: For implementation that would reward "exploration", this would need to be changed.
            // as all tiles would need to have a tracker for "ifExplored"
            //Destroy(floor.gameObject);

            for (int x = 0; x < roomData.tilemap.GetUpperBound(0); ++x)
            {
                for (int y = 0; y < roomData.tilemap.GetUpperBound(1); ++y)
                {
                    int xOffset = x + offset.x;
                    int yOffset = y + offset.y;

                    Tile tile = roomData.tilemap[x, y];
                    Vector3Int position = new Vector3Int(xOffset, yOffset, 0);

                    if (tile != null)
                    {
                        roomData.tilemap[x, y] = tile;

                        // Decorative tiles (not collidable or triggers)
                        if (tile is not IInteractable && tile is not ICollidable)
                        {
                            tilemapData.floor.SetTile(position, roomData.tilemap[x, y].GetTileBase());
                        }

                        // Trigger tiles (e.g. doors, items)
                        if (tile is IInteractable)
                        {
                            tilemapData.trigger.SetTile(position, roomData.tilemap[x, y].GetTileBase());
                        }

                        // Collidable tiles (e.g. pits, walls)
                        if (tile is ICollidable)
                        {
                            tilemapData.collidable.SetTile(position, roomData.tilemap[x, y].GetTileBase());
                        }
                    }
                }
            }
        }
    }
}