using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

namespace PCG
{
    public enum GenerationMethod
    {
        NONE,
        RANDOM,
        PERLINNOISE
    }

    public class PCGMethods : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static int[,] GenerateMap(int width, int height)
        {
            int[,] map = new int[width, height];
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
            for (int x = 0; x < map.GetUpperBound(0); x++)
            {
                for (int y = 0; y < map.GetUpperBound(1); y++)
                {
                    map[x, y] = Random.Range(0, 1);
                }
            }

            return map;
        }

        /// <summary>
        /// Extension method with collidable / void tile creation.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="tilemap"></param>
        /// <param name="floor"></param>
        /// <param name="collidable"></param>
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
        /// <param name="map"></param>
        /// <param name="tilemap"></param>
        /// <param name="floor"></param>
        /// <param name="collidable"></param>
        public static void RenderRoomOffset(int[,] map, Tilemap tilemap, TileBase floor, TileBase collidable, Vector2Int offset)
        {
            tilemap.ClearAllTiles();
            //Loop through the width of the map
            for (int x = 0; x < map.GetUpperBound(0); x++)
            {
                //Loop through the height of the map
                for (int y = 0; y < map.GetUpperBound(1); y++)
                {

                    // Creates walls if edge of tile
                    if ((x == 0 || x == map.GetUpperBound(0) - 1) || ((y == 0 || y == map.GetUpperBound(1) - 1)))
                    {
                        tilemap.SetTile(new Vector3Int(x + offset.x, y + offset.y, 0), collidable);
                    }

                    //if (CheckDoor(x, y, map))
                    //{
                       // tilemap.SetTile(new Vector3Int(x + //offset.x, y + offset.y, 0), floor);
                    //}

                    else
                    {
                        // 1 = Floor, 0 = Collidable / Pit
                        tilemap.SetTile(new Vector3Int(x + offset.x, y + offset.y, 0), map[x, y] == 1 ? floor : collidable);
                    }
                }
            }
        }

        private static bool CheckDoor(int x, int y, int[,] map)
        {
            // Top door
            if ((x == map.GetUpperBound(0) / 2) && (y == 0))
            {
                return true;
            }

            // Bottom door
            if ((x == map.GetUpperBound(0) / 2) && (y == map.GetUpperBound(1)))
            {
                return true;
            }

            // Left door
            if ((x == 0) && (y == map.GetUpperBound(1) / 2))
            {
                return true;
            }

            // Right door
            if ((x == map.GetUpperBound(0)) && (y == map.GetUpperBound(1) / 2))
            {
                return true;
            }

            return false;
        }
    }
}