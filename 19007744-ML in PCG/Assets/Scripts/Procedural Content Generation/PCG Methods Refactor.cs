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
                        return door;
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
                        return wall;
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
                    return floor;
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
                    return pit;
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
                    return coin;
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
                    return key;
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
                    return bomb;
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
        ///
        /// </summary>
        /// <param name="map"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static Tile[,] AStarPathfindingGeneration(Tile[,] map, float seed)
        {
            // Set the random number generator with seed
            Random.InitState((int) seed);



            return new Tile[,] { };
        }

        /// <summary>
        /// Generates a room in the map by setting tileBases on corresponding tilemaps through assigned tile in map position.
        /// </summary>
        /// <param name="map">2D array representing map to be generated.</param>
        /// <param name="tilemapData">Data of tilemaps and available tiles.</param>
        /// <param name="offset">Optional offset to apply to the tile positions from game space.</param>
        public static RoomData GenerateRoom(RoomData roomData, TilemapData tilemapData, Vector2Int offset = default)
        {
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
                    int xOffset = x + offset.x;
                    int yOffset = y + offset.y;

                    // TODO
                    Tile tile;

                    // Check map position for door.
                    var doorResult = CheckDoors(x, y, roomData.tilemap);
                    // Check map position for wall.
                    var wallResult = CheckWalls(x, y, roomData.tilemap);

                    // If map position was a door.
                    if (doorResult.Item1)
                    {
                        tilemapData.trigger.SetTile(new Vector3Int(xOffset , yOffset, 0),
                            GenerateDoors(x, y, roomData.tilemap, tilemapData, doorResult.Item2).GetTileBase());

                        roomData.tilemap[x, y] = GenerateDoors(x, y, roomData.tilemap, tilemapData, doorResult.Item2);

                        roomData = AssignDoor(roomData, doorResult.Item2, x, y);

                        doorPositions.Add(new Vector3Int(x, y, 0));
                    }

                    // If map position was a wall.
                    else if (wallResult.Item1)
                    {
                        tilemapData.collidable.SetTile(new Vector3Int(xOffset, yOffset, 0),
                            GenerateWalls(x, y, roomData.tilemap, tilemapData, wallResult.Item2).GetTileBase());

                        roomData.tilemap[x, y] = GenerateWalls(x, y, roomData.tilemap, tilemapData, wallResult.Item2);

                        wallPositions.Add(new Vector3Int(x, y, 0));
                    }


                    // If map position was not a boundary (door or wall).
                    else
                    {
                        // TODO
                        tilemapData.floor.SetTile(new Vector3Int(xOffset, yOffset, 0),
                            GenerateFloor(x, y, roomData.tilemap, tilemapData).GetTileBase());

                        tilemapData.trigger.SetTile(new Vector3Int(xOffset, yOffset, 0),
                            GenerateCoin(x, y, roomData.tilemap, tilemapData).GetTileBase());

                        roomData.tilemap[x, y] = GenerateCoin(x, y, roomData.tilemap, tilemapData);
                    }
                }
            }

            /*// Second pass: Generate floors ensuring paths to all doors
            List<Vector3Int> floorPositions = GeneratePathsToDoors(roomData.tilemap, doorPositions);
            Debug.Log($"Floor Positions: {floorPositions}");

            for (int x = 0; x < roomData.RoomWidth; ++x)
            {
                for (int y = 0; y < roomData.RoomHeight; ++y)
                {
                    int xOffset = x + offset.x;
                    int yOffset = y + offset.y;
                    Vector3Int position = new Vector3Int(xOffset, yOffset, 0);

                    if (roomData.tilemap[x, y] is TileDoor || roomData.tilemap[x, y] is TileWall)
                    {
                        continue; // Skip already placed doors and walls
                    }

                    if (floorPositions.Contains(new Vector3Int(x, y, 0)))
                    {
                        Debug.Log($"Generating floor at: {x} {y}");

                        Tile floorTile = GenerateFloor(x, y, roomData.tilemap, tilemapData);
                        tilemapData.floor.SetTile(position, floorTile.GetTileBase());
                        roomData.tilemap[x, y] = floorTile;
                    }
                    else
                    {
                        Debug.Log($"Generating pit at: {x} {y}");

                        Tile pit = GeneratePit(x, y, roomData.tilemap, tilemapData);
                        tilemapData.collidable.SetTile(position, pit.GetTileBase());
                        roomData.tilemap[x, y] = pit;
                    }
                }
            }*/

            return roomData;
        }

        /// <summary>
        /// Generates paths to all doors using a simple pathfinding algorithm.
        /// </summary>
        /// <param name="map">2D array representing map to be generated.</param>
        /// <param name="doorPositions">List of door positions.</param>
        /// <returns>List of floor positions ensuring paths to all doors.</returns>
        private static List<Vector3Int> GeneratePathsToDoors(Tile[,] map, List<Vector3Int> doorPositions)
        {
            List<Vector3Int> floorPositions = new List<Vector3Int>();

            if (doorPositions.Count == 0)
            {
                return floorPositions;
            }

            Vector3Int start = doorPositions[0];
            Queue<Vector3Int> queue = new Queue<Vector3Int>();
            queue.Enqueue(start);
            HashSet<Vector3Int> visited = new HashSet<Vector3Int> { start };

            while (queue.Count > 0)
            {
                Vector3Int current = queue.Dequeue();

                foreach (Vector3Int direction in new Vector3Int[] {
                    Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right })
                {
                    Vector3Int neighbor = current + direction;
                    if (!visited.Contains(neighbor) &&
                        neighbor.x >= 0 && neighbor.x < map.GetLength(0) &&
                        neighbor.y >= 0 && neighbor.y < map.GetLength(1) &&
                        !(map[neighbor.x, neighbor.y] is TileWall))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                        floorPositions.Add(neighbor);

                        if (doorPositions.Contains(neighbor))
                        {
                            doorPositions.Remove(neighbor);
                            if (doorPositions.Count == 0)
                                return floorPositions;
                        }
                    }
                }
            }

            return floorPositions;
        }

        /// <summary>
        /// Extension method with collidable / void tile creation.
        /// </summary>
        /// <param name="map">The map containing values to be mapped to tilemaps.</param>
        /// <param name="tilemap">The tilemap without collisions (e.g. floor).</param>
        /// <param name="collidableTilemap">The tilemap with collidable elements.</param>
        /// <param name="floor">The floor tile.</param>
        /// <param name="collidable">The collidable tile.</param>
        /// <param name="offset">The offset to move the tilemap.</param>
        public static void RenderRoom(int[,] map, Tilemap tilemap, Tilemap collidableTilemap, TileBase floor,
            TileBase collidable, Vector2Int offset)
        {
            tilemap.ClearAllTiles();
            collidableTilemap.ClearAllTiles();

            // Loop through the width of the map
            for (int x = 0; x < map.GetUpperBound(0); ++x)
            {
                // Loop through the height of the map
                for (int y = 0; y < map.GetUpperBound(1); ++y)
                {
                    // Check for door positions first.
                    if (PCGMethods.CheckDoor(x, y, map))
                    {
                        // TODO: Make this a door.......
                        tilemap.SetTile(new Vector3Int(x + offset.x, y + offset.y, 0), floor);
                    }

                    // Creates walls if edge of tile AND not a door.
                    else if ((x == 0 || x == map.GetUpperBound(0) - 1) || ((y == 0 || y == map.GetUpperBound(1) - 1)))
                    {
                        collidableTilemap.SetTile(new Vector3Int(x + offset.x, y + offset.y, 0), collidable);
                    }

                    else
                    {

                        // TODO: SWAP TO USING CUSTOM CLASSES!
                        /*var tempTileObject = PCG.Tilemaps.Tile.CreateTile((TileType)map[x,y]);
                        Debug.LogAssertion($"{tempTileObject}");
                        var tempTile = tempTileObject.GetComponent<PCG.Tilemaps.Tile>().tile;
                        Debug.LogAssertion($"{tempTile}");*/

                        // 1 = Floor, 0 = Collidable / Pit
                        if (map[x, y] == 1)
                        {
                            collidableTilemap.SetTile(new Vector3Int(x + offset.x, y + offset.y, 0), collidable);
                        }
                        else
                        {
                            tilemap.SetTile(new Vector3Int(x + offset.x, y + offset.y, 0), floor);
                        }
                    }
                }
            }
        }
    }
}