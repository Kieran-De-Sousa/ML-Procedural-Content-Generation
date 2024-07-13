// Base
using System.Collections.Generic;

// Unity
using UnityEngine;

// Ambiguous reference prevention (shares name with UnityEngine.Tilemaps.)
using Tile = PCG.Tilemaps.Tile;

// Sub-namespace of PCG Methods for A* Pathfinding Utilities
namespace PCG.Methods.AStarPathfinding
{
    /// <summary>
    /// PCG Methods for an A* pathfinding algorithm generation.
    /// </summary>
    /// <reference>https://pavcreations.com/tilemap-based-a-star-algorithm-implementation-in-unity-game/#how-a-star-algorithm-works-the-basics</reference>
    public static class AStarPathfinding
    {
        /// <summary>
        /// Represents a node in the pathfinding grid.
        /// </summary>
        public class Node
        {
            public Vector3Int Position; // Node position

            public int GCost;           // Distance cost from start node to this node.
            public int HCost;           // Heuristic cost from node to end node.
            public int FCost => GCost + HCost; // Total cost

            public Node Parent;         // Parent node

            public Node(Vector3Int position)
            {
                Position = position;
            }
        }

        /// <summary>
        /// Generates paths to all doors using A* pathfinding algorithm.
        /// </summary>
        /// <param name="map">2D array representing map to be generated.</param>
        /// <param name="doorPositions">List of door positions.</param>
        /// <returns>List of floor positions ensuring paths to all doors.</returns>
        public static List<Vector3Int> GeneratePathsToDoors(Tile[,] map, List<Vector3Int> doorPositions)
        {
            List<Vector3Int> floorPositions = new List<Vector3Int>();

            // "Null" check for empty door positions list...
            if (doorPositions.Count == 0)
            {
                return floorPositions;
            }

            // Generate paths between all doors
            for (int i = 0; i < doorPositions.Count; ++i)
            {
                for (int j = i + 1; j < doorPositions.Count; ++j)
                {
                    floorPositions.AddRange(FindPathAStar(map, doorPositions[i], doorPositions[j]));
                }
            }

            return floorPositions;
        }

        /// <summary>
        /// Find path between two node points.
        /// </summary>
        /// <param name="map">2D array representing the map.</param>
        /// <param name="start">Start Position.</param>
        /// <param name="end">End Position.</param>
        /// <returns>List of positions for the path.</returns>
        private static List<Vector3Int> FindPathAStar(Tile[,] map, Vector3Int start, Vector3Int end)
        {
            // Nodes to be checked.
            List<Node> openList = new List<Node>();

            // Nodes already checked.
            HashSet<Node> closedList = new HashSet<Node>();

            Node startNode = new Node(start);
            Node endNode = new Node(end);

            // Start node checking by adding the start node to the list.
            openList.Add(startNode);

            while (openList.Count > 0)
            {
                Node currentNode = openList[0];
                for (int i = 1; i < openList.Count; ++i)
                {
                    // Find node with lowest FCost, and set current node to this.
                    if (openList[i].FCost < currentNode.FCost ||
                        (openList[i].FCost == currentNode.FCost &&
                         openList[i].HCost < currentNode.HCost))
                    {
                        currentNode = openList[i];
                    }
                }

                // Remove the current node from "To Check" list.
                openList.Remove(currentNode);
                // Add current node to "Checked" list.
                closedList.Add(currentNode);

                if (currentNode.Position == end)
                {
                    // Path found to end, retrace the path taken to get there.
                    return RetracePath(startNode, currentNode);
                }

                // Loop through each neighbour of the node.
                foreach (Node neighbour in GetNeighbours(map, currentNode))
                {
                    if (closedList.Contains(neighbour))
                    {
                        // Skip this neighbour if it has already checked.
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.GCost + GetDistanceBetweenNodes(currentNode, neighbour);

                    if (newMovementCostToNeighbour < neighbour.GCost ||
                        !openList.Contains(neighbour))
                    {
                        neighbour.GCost = newMovementCostToNeighbour;
                        neighbour.HCost = GetDistanceBetweenNodes(neighbour, endNode);
                        neighbour.Parent = currentNode;

                        if (!openList.Contains(neighbour))
                        {
                            // Add neighbour to the "To Check" list.
                            openList.Add(neighbour);
                        }
                    }
                }
            }

            // Return an empty list if no path is found.
            return new List<Vector3Int>();
        }

        /// <summary>
        /// Retrace the path from the end node to the start node.
        /// </summary>
        /// <param name="startNode">Starting node.</param>
        /// <param name="endNode">Ending node.</param>
        /// <returns>List of node positions of the path generated.</returns>
        private static List<Vector3Int> RetracePath(Node startNode, Node endNode)
        {
            List<Vector3Int> path = new List<Vector3Int>();
            Node currentNode = endNode;

            // Retraced node positions backwards, from end till reaching start...
            while (currentNode != startNode)
            {
                path.Add(currentNode.Position);
                currentNode = currentNode.Parent;
            }

            // Reverse the path to return the order to "Start-->End"
            path.Reverse();
            return path;
        }

        /// <summary>
        /// Gets neighbour nodes of an inputted node.
        /// </summary>
        /// <param name="map">2D array representing the map.</param>
        /// <param name="node">Node to grab neighbours from.</param>
        /// <returns>List of neighbouring nodes.</returns>
        private static List<Node> GetNeighbours(Tile[,] map, Node node)
        {
            List<Node> neighbours = new List<Node>();

            // 2D tilemap so neighbours can be UP, DOWN, LEFT, RIGHT
            Vector3Int[] neighbourPositions =
            {
                new (node.Position.x, node.Position.y + 1, 0),
                new (node.Position.x, node.Position.y - 1, 0),
                new (node.Position.x - 1, node.Position.y, 0),
                new (node.Position.x + 1, node.Position.y, 0),
            };

            foreach (Vector3Int position in neighbourPositions)
            {
                // Check if neighbour is within map bounds.
                if (position.x >= 0 && position.y >= 0 &&
                    position.x < map.GetLength(0) &&
                    position.y < map.GetLength(1))
                {
                    neighbours.Add(new Node(position));
                }
            }

            return neighbours;
        }

        /// <summary>
        /// Calculates the distance between two nodes.
        /// </summary>
        /// <param name="nodeA">First node.</param>
        /// <param name="nodeB">Second node.</param>
        /// <returns>Distance between nodes.</returns>
        private static int GetDistanceBetweenNodes(Node nodeA, Node nodeB)
        {
            int distanceX = Mathf.Abs(nodeA.Position.x - nodeB.Position.x);
            int distanceY = Mathf.Abs(nodeA.Position.y - nodeB.Position.y);

            return distanceX + distanceY;
        }
    }
}