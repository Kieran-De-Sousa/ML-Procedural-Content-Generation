// Base
using System.Collections;
using System.Collections.Generic;

// Unity
using UnityEngine;
using UnityEngine.Tilemaps;
using Tile = UnityEngine.Tilemaps.Tile;

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
    public class PCGMethods : MonoBehaviour
    {
        /// <summary>
        /// Generates a new int map array with the passed width and height.
        /// </summary>
        /// <param name="width">Width of the map.</param>
        /// <param name="height">Height of the map.</param>
        /// <returns>2D array of generated map.</returns>
        public static int[,] GenerateMap(int width, int height)
        {
            int[,] map = new int[width + 1, height + 1];
            return map;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="map"></param>
        public static void GenerateWalls(int[,] map)
        {

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

            //Used to reduced the position of the perlin point
            float reduction = 0.5f;
            //Create the perlin
            for (int x = 0; x < map.GetUpperBound(0); x++)
            {
                newPoint = Mathf.FloorToInt((Mathf.PerlinNoise(x, seed) - reduction) * map.GetUpperBound(1));

                //Make sure the noise starts near the halfway point of the height
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
        public static int[,] RandomGeneration(int[,] map, float seed)
        {
            // Set the random number generator with seed
            Random.InitState((int)seed);

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
        /// Extension method with collidable / void tile creation.
        /// </summary>
        /// <param name="map">The map containing values to be mapped to tilemaps.</param>
        /// <param name="tilemap">The tilemap without collisions (e.g. floor).</param>
        /// <param name="floor">The floor tile.</param>
        /// <param name="collidable">The collidable tile.</param>
        public static void RenderRoom(int[,] map, Tilemap tilemap, TileBase floor, TileBase collidable)
        {
            tilemap.ClearAllTiles();
            //Loop through the width of the map
            for (int x = 0; x < map.GetUpperBound(0) ; x++)
            {
                //Loop through the height of the map
                for (int y = 0; y < map.GetUpperBound(1); y++)
                {
                    // 1 = Floor, 0 = Collidable / Pit
                    tilemap.SetTile(new Vector3Int(x, y, 0), map[x, y] == 1 ? floor : collidable);
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
        public static void RenderRoomOffset(int[,] map, Tilemap tilemap, Tilemap collidableTilemap, TileBase floor, TileBase collidable, Vector2Int offset)
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
                    if (CheckDoor(x, y, map))
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

        /// <summary>
        /// Checks if the position on the map is the location of a door.
        /// </summary>
        /// <param name="x">X position of tile.</param>
        /// <param name="y">Y position of tile.</param>
        /// <param name="map">2D int array representing the map.</param>
        /// <returns><c>true</c> if the position is a door location, otherwise; <c>false</c>.</returns>
        public static bool CheckDoor(int x, int y, int[,] map)
        {
            int minX = map.GetLowerBound(0);
            int minY = map.GetLowerBound(1);
            int maxX = map.GetUpperBound(0);
            int maxY = map.GetUpperBound(1);

            // Bottom door
            if (y == minY && x == maxX / 2)
            {
                return true;
            }

            // Left door
            if (x == minX && y == maxY / 2)
            {
                return true;
            }

            //----- NOTE: These magic numbers of +1 are the only way to get this working correctly... -----//
            // Top door
            if (y + 1 == maxY && x == maxX / 2)
            {
                return true;
            }

            // Right door
            if (x + 1 == maxX && y == maxY / 2)
            {
                return true;
            }

            // Return false if this was not a door location.
            return false;
        }

    }
}