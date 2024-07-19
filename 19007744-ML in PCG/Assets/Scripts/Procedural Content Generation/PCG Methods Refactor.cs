// Base
using System.Collections.Generic;

// Unity
using UnityEngine;

// Procedural Content Generation
using PCG.Tilemaps;

// Ambiguous reference prevention (shares name with UnityEngine.Tilemaps.)
using Tile = PCG.Tilemaps.Tile;

// Root namespace for all Procedural Content Generation-related utilities.
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
        WFC,
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
        /// Generate a 2D Tile array with doors and walls.
        /// </summary>
        /// <param name="roomData">Data of tile coordinates on 2D array, and important tile positions (doors).</param>
        /// <param name="tilemapData">Data of tilemaps and available tiles.</param>
        /// <returns>Tuple containing updated room data, positions of doors, and position of walls.</returns>
        private static (RoomData, List<Vector3Int>, List<Vector3Int>) GenerateTileMapDoorsAndWalls(RoomData roomData,
            TilemapData tilemapData)
        {
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
                        tile = GenerateDoors(tilemapData, doorResult.Item2);
                        roomData = AssignDoor(roomData, doorResult.Item2, x, y);
                        doorPositions.Add(new Vector3Int(x, y, 0));
                    }
                    // If map position was a wall.
                    else if (wallResult.Item1)
                    {
                        tile = GenerateWalls(tilemapData, wallResult.Item2);
                        wallPositions.Add(new Vector3Int(x, y, 0));
                    }

                    if (tile != null)
                    {
                        roomData.tilemap[x, y] = tile;
                    }
                }
            }

            return (roomData, doorPositions, wallPositions);
        }


        /// <summary>
        /// Assigns a door to the RoomData based on its direction.
        /// </summary>
        /// <param name="data">RoomData to update.</param>
        /// <param name="direction">Direction of door.</param>
        /// <param name="x">X position of door.</param>
        /// <param name="y">Y position of door.</param>
        /// <returns>Updated room data with door position assigned.</returns>
        private static RoomData AssignDoor(RoomData data, DoorDirection direction, int x, int y)
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
            }

            return data;
        }

        /// <summary>
        /// Generate a door tile facing the provided direction.
        /// </summary>
        /// <param name="tilemapData">Data of available tiles.</param>
        /// <param name="direction">Direction the door will face.</param>
        /// <returns>Generated door tile, otherwise, <c>null</c></returns>
        private static Tile GenerateDoors(TilemapData tilemapData, DoorDirection direction)
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
        /// <param name="tilemapData">Data of available tiles.</param>
        /// <param name="direction">Direction the wall will face.</param>
        /// <returns>Generated wall tile, otherwise, <c>null</c></returns>
        private static Tile GenerateWalls(TilemapData tilemapData, WallDirection direction)
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
        /// <param name="tilemapData">Data of available tiles.</param>
        /// <returns>Generated floor tile, otherwise, <c>null</c></returns>
        private static Tile GenerateFloor(TilemapData tilemapData)
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
        /// Generate a pit tile.
        /// </summary>
        /// <param name="tilemapData">Data of available tiles.</param>
        /// <returns>Generated pit tile, otherwise, <c>null</c></returns>
        private static Tile GeneratePit(TilemapData tilemapData)
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

        private static Tile GenerateRandomItem(TilemapData tilemapData)
        {
            float randomValue = Random.value;

            if (randomValue < 0.33f)
            {
                return GenerateCoin(tilemapData);
            }

            else if (randomValue is >= 0.33f and < 0.66f)
            {
                return GenerateKey(tilemapData);
            }

            else
            {
                return GenerateBomb(tilemapData);
            }
        }

        /// <summary>
        /// Generate a coin tile.
        /// </summary>
        /// <param name="tilemapData">Data of available tiles.</param>
        /// <returns>Generated coin tile, otherwise, <c>null</c></returns>
        private static Tile GenerateCoin(TilemapData tilemapData)
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

        /// <summary>
        /// Generate a key tile.
        /// </summary>
        /// <param name="tilemapData">Data of available tiles.</param>
        /// <returns>Generated key tile, otherwise, <c>null</c></returns>
        private static Tile GenerateKey(TilemapData tilemapData)
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

        /// <summary>
        /// Generate a bomb tile.
        /// </summary>
        /// <param name="tilemapData">Data of available tiles.</param>
        /// <returns>Generated bomb tile, otherwise, <c>null</c></returns>
        private static Tile GenerateBomb(TilemapData tilemapData)
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
        ///
        /// </summary>
        /// <param name="tilemapData"></param>
        /// <param name="itemWeight"></param>
        /// <param name="pitWeight"></param>
        /// <returns></returns>
        private static Tile GenerateRandomTileFromWeighting(TilemapData tilemapData,
            float itemWeight, float pitWeight)
        {
            float randomValue = Random.value;

            if (randomValue < itemWeight)
            {
                return GenerateRandomItem(tilemapData);
            }

            if (randomValue < itemWeight + pitWeight)
            {
                return GeneratePit(tilemapData);
            }

            // If neither, generate a floor.
            return GenerateFloor(tilemapData);
        }

        /// <summary>
        /// Checks if the position on the map is the location of a door.
        /// </summary>
        /// <param name="x">X position of tile.</param>
        /// <param name="y">Y position of tile.</param>
        /// <param name="map">2D int array representing the map.</param>
        /// <returns><c>true</c> if the position is a door location, otherwise; <c>false</c>.</returns>
        private static (bool, DoorDirection) CheckDoors(int x, int y, Tile[,] map)
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
        private static (bool, WallDirection) CheckWalls(int x, int y, Tile[,] map)
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
        /// Generates a room in the map by setting tileBases on corresponding tilemaps through assigned tile in map position.
        /// This is done through a random algorithm, where the PCG result is not guaranteed to be a "completable" level.
        /// </summary>
        /// <param name="roomData">Data of tile coordinates on 2D array, and important tile positions (doors).</param>
        /// <param name="tilemapData">Data of tilemaps and available tiles.</param>
        /// <param name="offset">Optional offset to apply to the tile positions from game space.</param>
        /// <param name="seed">Seed value for RNG.</param>
        /// <returns>Data of generated room.</returns>
        public static RoomData RandomGeneration(RoomData roomData, TilemapData tilemapData,
            Vector2Int offset = default, float seed = default)
        {
            // Set the random number generator with seed
            Random.InitState((int) seed);

            // Clear current tiles when generating room.
            foreach (var tilemap in tilemapData.allTilemaps)
            {
                tilemap.ClearAllTiles();
            }

            // Generate the walls and doors of the room, and save their positions for the A* pathfinding algorithm to use.
            var roomBase = GenerateTileMapDoorsAndWalls(roomData, tilemapData);
            roomData = roomBase.Item1;
            List<Vector3Int> doorPositions = roomBase.Item2;
            List<Vector3Int> wallPositions = roomBase.Item3;

            // Loop through the tilemap coordinates and start assignment of tiles.
            for (int x = 0; x < roomData.tilemap.GetUpperBound(0); ++x)
            {
                for (int y = 0; y < roomData.tilemap.GetUpperBound(1); ++y)
                {
                    // Only generate on unset tiles (e.g. non-wall/door tiles)
                    if (roomData.tilemap[x, y] == null)
                    {
                        Tile tile = null;

                        // TODO: RANDOM GENERATION HERE.
                        roomData.tilemap[x, y] = tile;
                    }
                }
            }

            return roomData;
        }

        /// <summary>
        /// Generates a room in the map by setting tileBases on corresponding tilemaps through assigned tile in map position.
        /// This is done through an A* pathfinding algorithm, ensuring the PCG result is a "completable" level.
        /// </summary>
        /// <param name="roomData">Data of tile coordinates on 2D array, and important tile positions (doors).</param>
        /// <param name="tilemapData">Data of tilemaps and available tiles.</param>
        /// <param name="offset">Optional offset to apply to the tile positions from game space.</param>
        /// <param name="seed">Seed value for RNG.</param>
        /// <returns>Data of generated room.</returns>
        public static RoomData AStarPathFindingGeneration(RoomData roomData, TilemapData tilemapData,
            Vector2Int offset = default, float seed = default)
        {
            // Set the random number generator with seed
            Random.InitState((int) seed);

            // Clear current tiles when generating room.
            foreach (var tilemap in tilemapData.allTilemaps)
            {
                tilemap.ClearAllTiles();
            }

            // NOTE: Tweak a bit...
            // Based on engagement from previous room, weight the random generation elements of this room...
            float totalEngagement = roomData.engagementPreviousRoom.GetEngagementScore();
            float itemWeight = Mathf.Clamp(0.75f - roomData.engagementPreviousRoom.itemPickups / ((totalEngagement + 1) * 1.1f), 0.175f, 0.3f);
            float pitWeight = Mathf.Clamp(roomData.engagementPreviousRoom.exploration / ((totalEngagement + 1) * 1.1f), 0.25f, 0.5f);
            //Debug.Log($"Total Engagement: {totalEngagement}");
            //Debug.Log($"Item Weight: {itemWeight}");
            //Debug.Log($"Pit Weight: {pitWeight}");

            // Generate the walls and doors of the room, and save their positions for the A* pathfinding algorithm to use.
            var roomBase = GenerateTileMapDoorsAndWalls(roomData, tilemapData);
            roomData = roomBase.Item1;
            List<Vector3Int> doorPositions = roomBase.Item2;
            List<Vector3Int> wallPositions = roomBase.Item3;

            // Generate paths to doors using A* Pathfinding
            List<Vector3Int> floorPositions = Methods.AStarPathfinding.AStarPathfinding
                .GeneratePathsToDoors(roomData.tilemap, doorPositions);

            for (int x = 0; x < roomData.tilemap.GetUpperBound(0); ++x)
            {
                for (int y = 0; y < roomData.tilemap.GetUpperBound(1); ++y)
                {
                    Tile tile = null;

                    if (roomData.tilemap[x, y] is TileDoor || roomData.tilemap[x, y] is TileWall)
                    {
                        tile = roomData.tilemap[x, y];
                    }

                    else if (floorPositions.Contains(new Vector3Int(x, y, 0)))
                    {
                        tile = GenerateFloor(tilemapData);
                    }

                    else
                    {
                        if (totalEngagement == 0)
                        {
                            int random = Random.Range(0, 2);
                            tile = random == 1
                                ? GenerateRandomItem(tilemapData)
                                : GeneratePit(tilemapData);
                        }
                        else
                        {
                            float floorWeight = 1 - (itemWeight + pitWeight);
                            tile = GenerateRandomTileFromWeighting(tilemapData,
                                itemWeight, pitWeight);
                        }
                    }

                    roomData.tilemap[x, y] = tile;
                }
            }

            return roomData;
        }

        /// <summary>
        /// Renders the room based on the 2D Tile array coordinates, tilemap data, and optional offset.
        /// </summary>
        /// <param name="roomData">Data of tile coordinates on 2D array, and important tile positions (doors).</param>
        /// <param name="tilemapData">Data of tilemaps and available tiles.</param>
        /// <param name="offset">Optional offset to apply to the tile positions from game space.</param>
        public static void RenderRoom(RoomData roomData, TilemapData tilemapData,
            Vector2Int offset = default)
        {
            // Early out if tilemap is null.
            if (roomData.tilemap == null) return;

            // Loop through size of map to start setting Tilemap tileBases to coordinates.
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
                            tile.SetOwnerTilemap(tilemapData.floor);
                        }
                        // Create the floor as a background for items (since they don't have backgrounds)...
                        else if (tile is Item)
                        {
                            Tile floor = GenerateFloor(tilemapData);
                            tilemapData.floor.SetTile(position, floor.GetTileBase());

                            // Destroy the GameObject part of the tile, as we don't reward exploration on item spaces,
                            // and we have just created our background.
                            Destroy(floor.gameObject);
                        }

                        // Trigger tiles (e.g. doors, items)
                        if (tile is IInteractable)
                        {
                            tilemapData.trigger.SetTile(position, roomData.tilemap[x, y].GetTileBase());
                            tile.SetOwnerTilemap(tilemapData.trigger);
                        }

                        // Collidable tiles (e.g. pits, walls)
                        if (tile is ICollidable)
                        {
                            tilemapData.collidable.SetTile(position, roomData.tilemap[x, y].GetTileBase());
                            tile.SetOwnerTilemap(tilemapData.collidable);
                        }

                        tile.SetTilePosition(new Vector3Int(x, y));
                        tile.SetTileWorldPosition(tile.GetOwnerTilemap().GetCellCenterWorld(new Vector3Int(x, y)));
                        roomData.tilemap[x, y] = tile;
                    }
                }
            }
        }
    }
}