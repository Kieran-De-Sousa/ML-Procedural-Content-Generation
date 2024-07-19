// Unity
using UnityEngine;

// PCG
using PCG.Tilemaps;
using Unity.MLAgents.Policies;

// Helper
using Utilities;

// Ambiguous reference prevention (shares name with UnityEngine.Tilemaps.)
using Tile = PCG.Tilemaps.Tile;

// Root namespace for all Procedural Content Generation-related utilities.
namespace PCG
{
    /// <summary>
    /// Manager of Procedural Content Generation (PCG) system, pulls data from <c>TilemapSystem</c> to generate
    /// 2D tile array that are mapped to tilemaps of simulation.
    /// </summary>
    public class PCGSystemRefactor : ManagerSystem
    {
        // TilemapSystem in the same simulation.
        [SerializeField] private TilemapSystem tilemapSystem;

        [Header("Tilemap Size")]
        [Range(13, 100)]
        public int width = 17;
        [Range(7, 100)]
        public int height = 9;

        // If the room has already been generated (cloned from a high engagement simulation).
        private bool preGenerated = false;
        [HideInInspector] public RoomData roomData;
        [HideInInspector] public Vector3 roomOrigin;

        // Engagement information from the previous created room and highest created room.
        public EngagementMetrics previousEngagement;
        public EngagementMetrics highestEngagement;
        [Tooltip("To ensure we don't constantly save new simulations every time a small increase happens, apply a percentage " +
                 "increase required for a save to happen.")]
        [Range(1.0f, 3.0f)]
        public float engagementIncreaseBuffer = 1.5f;

        [Space]

        [Header("PCG Generation Method")]
        public GenerationMethod generation = GenerationMethod.ASTAR;

        private InputScheme _inputScheme;

        protected void Awake()
        {
            roomData = RoomData.GenerateRoom(width, height);
        }

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
        /// Regenerate roomData, clear room, generate new room, and move player and camera.
        /// </summary>
        public override void ResetSystem()
        {
            // TODO: Needs tweaking...
            if (preGenerated)
            {
                ClearRoom();
                GeneratePremade();

                AssignTileMapMembers();
                // Find centre tile in tilemap, then get the centre position of that centre tile found.
                roomOrigin = tilemapSystem.tilemapData.collidable.GetCellCenterWorld(HelperUtilities.GetCenterTilePosition(tilemapSystem.tilemapData.collidable));
            }

            // If room is not pre-generated (active simulation), check for engagement metrics to know if the room layout is worth keeping.
            else
            {
                // Evaluate if engagement improved from previous iteration...
                if (previousEngagement.EngagementScore > highestEngagement.EngagementScore * engagementIncreaseBuffer)
                {
                    // Set new highest engagement data and keep a copy of the room layout which generated it in tilemap data.
                    highestEngagement = previousEngagement;
                    tilemapSystem.highestEngagementRoom = roomData;

                    // Check if an agent is controlling the player, if so, start saving data...
                    ML.MLAgent player = HelperUtilities.FindParentOrChildWithComponent<ML.MLAgent>(transform);
                    if (player.GetComponent<BehaviorParameters>().BehaviorType != BehaviorType.HeuristicOnly ||
                        !player.trainingMode)
                    {
                        Simulation simulation = HelperUtilities.FindParentOrChildWithComponent<Simulation>(transform);
                        if (simulation.generateEngagementPrefabs)
                        {
                            StartCoroutine(simulation.SaveRoomPrefab(highestEngagement.EngagementScore));
                        }
                    }
                }

                roomData.engagementPreviousRoom = previousEngagement;
                // Null reference exception handling...
                roomData = RoomData.GenerateRoom(width, height, previousEngagement);

                ClearRoom();
                GenerateRoom();
            }

            SpawnPlayer();
            MoveCamera();
        }

        /// <summary>
        /// Generate room level based on:
        /// - Seed (Current time).
        /// - Room width and height.
        /// - Selected generation method.
        /// </summary>
        private void GenerateRoom()
        {
            ClearRoom();
            float seed = Random.Range(1, 1000);

            switch (generation)
            {
                case GenerationMethod.RANDOM:
                {
                    roomData.tilemap = PCGMethodsRefactor.GenerateTileMap(width, height);
                    roomData = PCGMethodsRefactor.RandomGeneration(roomData, tilemapSystem.tilemapData, default, seed);
                    Debug.Log("Fully random Room Generation is currently unimplemented in new Tile system.");
                    break;
                }

                case GenerationMethod.ASTAR:
                {
                    roomData.tilemap = PCGMethodsRefactor.GenerateTileMap(width, height);
                    roomData = PCGMethodsRefactor.AStarPathFindingGeneration(roomData, tilemapSystem.tilemapData, default, seed);
                    break;
                }

                default:
                {
                    Debug.LogError($"{generation} TYPE OF PCG GENERATION IS NOT SUPPORTED!");
                    break;
                }
            }

            PCGMethodsRefactor.RenderRoom(roomData, tilemapSystem.tilemapData);

            AssignTileMapMembers();

            // Find centre tile in tilemap, then get the centre position of that centre tile found.
            roomOrigin = tilemapSystem.tilemapData.collidable.GetCellCenterWorld(HelperUtilities.GetCenterTilePosition(tilemapSystem.tilemapData.collidable));

            SpawnPlayer();
            MoveCamera();
        }

        /// <summary>
        /// Generate a pre-made room layout.
        /// </summary>
        /// <important>
        /// PLEASE NOTE: THE FOLLOWING METHOD RECIEVES NULL REFERENCE EXCEPTION ERRORS AS 2D ARRAY DATA (e.g.
        /// roomData.tilemap) IS NOT SERIALIZED AND PASSED THROUGH WHEN COPIED FROM A GAMEOBJECT!
        /// </important>
        private void GeneratePremade()
        {
            roomData.tilemap = tilemapSystem.highestEngagementRoom.tilemap;

            Debug.Log(roomData.tilemap.GetUpperBound(0));
            Debug.Log(roomData.tilemap.GetUpperBound(1));
            for (int x = 0; x < roomData.tilemap.GetUpperBound(0); ++x)
            {
                for (int y = 0; y < roomData.tilemap.GetUpperBound(1); ++y)
                {
                    Debug.Log(roomData.tilemap[x,y]);
                }
            }
            Debug.Log(tilemapSystem);
            Debug.Log(tilemapSystem.tilemapData.allTilemaps.Count);
            PCGMethodsRefactor.RenderRoom(roomData, tilemapSystem.tilemapData, default);
        }

        /// <summary>
        /// Clear all tiles in all tilemaps.
        /// </summary>
        private void ClearRoom()
        {
            foreach (var map in tilemapSystem.tilemapData.allTilemaps)
            {
                map.ClearAllTiles();
            }

            if (roomData.tilemap != null)
            {
                // Loop through width of map
                for (int x = 0; x < roomData.tilemap.GetUpperBound(0); ++x)
                {
                    // Loop through the height of the map
                    for (int y = 0; y < roomData.tilemap.GetUpperBound(1); ++y)
                    {
                        if (roomData.tilemap[x, y] != null)
                        {
                            Destroy(roomData.tilemap[x, y].gameObject);
                        }
                    }
                }
            }

            // Clear all instantiated tiles from parent empty.
            HelperUtilities.DestroyAllChildren(tilemapSystem.instantiatedTilesParent.gameObject);
        }

        /// <summary>
        /// Move player position to centre of room.
        /// </summary>
        private void SpawnPlayer()
        {
            ML.MLAgent player = HelperUtilities.FindParentOrChildWithComponent<ML.MLAgent>(transform);
            player.transform.position = new Vector3(roomOrigin.x, roomOrigin.y, player.transform.position.z);
        }

        /// <summary>
        /// Move the camera to look at the centre of room.
        /// </summary>
        private void MoveCamera()
        {
            Camera camera = HelperUtilities.FindParentOrChildWithComponent<Camera>(transform);
            camera.transform.position = new Vector3(roomOrigin.x, roomOrigin.y, camera.transform.position.z);
        }

        /// <summary>
        /// Map GameObject instances of tiles from 2D tile array to their member data variables (e.g. MLAgent, tile position in world space).
        /// </summary>
        private void AssignTileMapMembers()
        {
            if (roomData.tilemap != null)
            {
                // Loop through width of map
                for (int x = 0; x < roomData.tilemap.GetUpperBound(0); ++x)
                {
                    // Loop through the height of the map
                    for (int y = 0; y < roomData.tilemap.GetUpperBound(1); ++y)
                    {
                        if (roomData.tilemap[x, y] != null)
                        {
                            Tile tile = roomData.tilemap[x, y];
                            tile.SetPlayer(HelperUtilities.FindParentOrChildWithComponent<ML.MLAgent>(transform));

                            // Set GameObject parent to empty transform.
                            tile.gameObject.transform.parent = tilemapSystem.instantiatedTilesParent;
                            // Set GameObject position to tile's centre position.
                            tile.gameObject.transform.position = tile.GetTileWorldPosition();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Set if the room layout is pre-generated.
        /// </summary>
        /// <param name="pregenerated">Pregeneration state.</param>
        public void SetPreGenerated(bool pregenerated) => preGenerated = pregenerated;

        /// <summary>
        /// Get if the room layout was pre-generated from previous room layout.
        /// </summary>
        /// <returns>If room is pre-generated.</returns>
        public bool GetPreGenerated() { return preGenerated; }
    }
}