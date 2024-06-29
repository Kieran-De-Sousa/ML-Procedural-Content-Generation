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
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="map"></param>
        /// <param name="tilemapData"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
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
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="map"></param>
        /// <param name="tilemapData"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
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
        /// <param name="map">2D int array representing the map.</param>
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
        ///
        /// </summary>
        /// <param name="map"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
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
        ///
        /// </summary>
        /// <param name="map"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
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

        // TODO
        public static Tile[,] AStarPathfindingGeneration(Tile[,] map, float seed)
        {
            // Set the random number generator with seed
            Random.InitState((int) seed);



            return new Tile[,] { };
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="map"></param>
        /// <param name="tilemapData"></param>
        /// <param name="offset"></param>
        public static void GenerateRoom(Tile[,] map, TilemapData tilemapData, Vector2Int offset = default)
        {
            // Clear current tiles when generating room.
            foreach (var tilemap in tilemapData.allTilemaps)
            {
                tilemap.ClearAllTiles();
            }

            // NOTE: In a 2D map, the below are the positions for min and max
            //                 MaxY
            // (MaxY,0) |-----------------| (MaxX,MaxY)
            //          |                 |
            //     MinX |                 | MaxX
            //          |                 |
            //    (0,0) |-----------------| (MaxX,0)
            //                 MinY

            // Loop through the width of the map
            for (int x = 0; x < map.GetUpperBound(0); ++x)
            {
                // Loop through the height of the map
                for (int y = 0; y < map.GetUpperBound(1); ++y)
                {
                    int xOffset = x + offset.x;
                    int yOffset = y + offset.y;
                    // TODO
                    Tile tile;

                    // Check map position for door.
                    var doorResult = CheckDoors(x, y, map);
                    // Check map position for wall.
                    var wallResult = CheckWalls(x, y, map);

                    // If map position was a door.
                    if (doorResult.Item1)
                    {
                        tilemapData.trigger.SetTile(new Vector3Int(xOffset , yOffset, 0),
                            GenerateDoors(x, y, map, tilemapData, doorResult.Item2).GetTileBase());
                    }

                    // If map position was a wall.
                    else if (wallResult.Item1)
                    {
                        tilemapData.collidable.SetTile(new Vector3Int(xOffset, yOffset, 0),
                            GenerateWalls(x, y, map, tilemapData, wallResult.Item2).GetTileBase());
                    }

                    // If map position was not a boundary (door or wall).
                    else
                    {
                        // TODO
                        tilemapData.floor.SetTile(new Vector3Int(xOffset, yOffset, 0),
                            GenerateFloor(x, y, map, tilemapData).GetTileBase());
                    }
                }
            }
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