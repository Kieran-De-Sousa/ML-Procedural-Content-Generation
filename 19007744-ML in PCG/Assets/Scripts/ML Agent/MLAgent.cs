// Unity
using UnityEngine;

// Unity ML Agents
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

// PCG Tilemaps
using PCG.Tilemaps;
using Utilities;

// Root namespace for all Machine Learning-related utilities.
namespace ML
{
    /// <summary>
    /// Handles agent behaviour for 2D, top-down environment using ML-Agents.
    /// </summary>
    [RequireComponent(typeof(Inventory))]
    public class MLAgent : Agent
    {
        // -------------------- Member Variables -------------------- //
        [Header("Agent Parameters")]
        [Tooltip("Whether this is training mode or gameplay mode")]
        public bool trainingMode;
        public int maxSteps = 5000;
        public EngagementMetrics engagement = new EngagementMetrics(0, 0,0 );
        public float currentReward => GetCumulativeReward();

        // The rigidbody of the agent
        new public Rigidbody2D rigidbody;

        [Header("Movement")]
        public Vector2 movementSpeed = new(3.5f, 3.5f);
        private Vector2 _inputVector = Vector2.zero;
        private InputScheme _inputScheme;

        // Private members tracking the agents own inventory and simulation.
        private Inventory inventory;
        private Simulation _simulation;

        // TILEMAP ARRAY MEMBERS //
        private Tile[,] _tilemapCoordinates => _simulation.pcgSystemRefactor.roomData.tilemap;
        private Vector3Int _cellPosition => GetAgentCellPosition();

        // NEAREST TILES MEMBERS //
        private Item _nearestItem => UpdateNearestItem();
        private TileDoor _nearestDoor => UpdateNearestDoor();
        private TileCollidable _nearestObstacle => UpdateNearestObstacle();
        private TileFloor _nearestUnexploredFloor => UpdateNearestFloor();

        // Whether the agent is frozen (intentionally not moving)
        private bool frozen = false;

        // -------------------- Methods -------------------- //
        private void Awake()
        {
            inventory = GetComponent<Inventory>();
            InitialiseInput();
        }

        /// <summary>
        /// Initialise new Unity Input System scheme for the agent.
        /// </summary>
        private void InitialiseInput()
        {
            _inputScheme = new InputScheme();
            _inputScheme._2DTopDown.Enable();
        }

        private void OnDestroy()
        {
            // Prevent possible memory leaks by disposing newly created scheme.
            _inputScheme.Dispose();
        }

        /// <summary>
        /// Initialize the agent.
        /// </summary>
        public override void Initialize()
        {
            _simulation = GetComponentInParent<Simulation>();

            // If not in training mode, no max step, play forever.
            MaxStep = trainingMode ? maxSteps : 0;

        }

        /// <summary>
        /// Reset the agent when an episode begins.
        /// </summary>
        public override void OnEpisodeBegin()
        {
            _simulation.pcgSystemRefactor.ResetSystem();
            ResetAgent();
        }

        /// <summary>
        /// Called when an action is received from either the player input or the neural network.
        ///
        /// actions.ContinuousActions[i] represents:
        /// Index 0 (Float): Horizontal Movement (-1 to 1)
        /// Index 1 (Float): Vertical Movement (-1 to 1)
        /// </summary>
        /// <param name="actions">The actions to take.</param>
        public override void OnActionReceived(ActionBuffers actions)
        {
            if (frozen) return;

            // Get the continuous actions
            float horizontal = actions.ContinuousActions[0];
            float vertical = actions.ContinuousActions[1];

            // Set the input vector based on actions received
            _inputVector = new Vector2(horizontal, vertical).normalized;

            // Apply movement
            rigidbody.MovePosition(rigidbody.position + (_inputVector * movementSpeed * Time.fixedDeltaTime));
        }

        /// <summary>
        /// Collect vector observations from the environment. Called every step.
        /// </summary>
        /// <param name="sensor">The vector sensor.</param>
        public override void CollectObservations(VectorSensor sensor)
        {
            // (3 observations)
            // Observe the agent's local position (X, Y, Z) normalized.
            sensor.AddObservation(transform.localPosition.normalized);

            // (2 observations)
            // Observe the agent's tilemap position (X, Y)
            sensor.AddObservation(_cellPosition.x);
            sensor.AddObservation(_cellPosition.y);

            // (1 observation)
            // Observe the distance between the agent and the nearest item.
            float itemDistance = _nearestItem != null ? Vector3.Distance(transform.position, _nearestItem.transform.position) : -1f;
            sensor.AddObservation(itemDistance);

            // (2 observations)
            // Observe the nearest items tilemap position (X, Y)
            sensor.AddObservation(_nearestItem != null ? _nearestItem.GetTilePosition().x : -1f);
            sensor.AddObservation(_nearestItem != null ? _nearestItem.GetTilePosition().y : -1f);

            // (1 observation)
            // Observe the distance between the agent and the nearest door.
            float doorDistance = _nearestDoor != null ? Vector3.Distance(transform.position, _nearestDoor.transform.position) : -1f;
            sensor.AddObservation(doorDistance);

            // (2 observations)
            // Observe the nearest doors tilemap position (X, Y)
            sensor.AddObservation(_nearestDoor != null ? _nearestDoor.GetTilePosition().x : -1f);
            sensor.AddObservation(_nearestDoor != null ? _nearestDoor.GetTilePosition().y : -1f);

            // (1 observation)
            // Observe the distance between the agent and the nearest obstacle.
            float obstacleDistance = _nearestObstacle != null ? Vector3.Distance(transform.position, _nearestObstacle.transform.position) : -1f;
            sensor.AddObservation(obstacleDistance);

            // (2 observations)
            // Observe the nearest obstacles tilemap position (X, Y)
            sensor.AddObservation(_nearestObstacle != null ? _nearestObstacle.GetTilePosition().x : -1f);
            sensor.AddObservation(_nearestObstacle != null ? _nearestObstacle.GetTilePosition().y : -1f);

            // 14 Total Observations //
        }

        /// <summary>
        /// When Behavior Type is set to "Heuristic Only" on the agent's Behavior Parameters,
        /// this function will be called. Its return values will be fed into
        /// <see cref="OnActionReceived(ActionBuffers)"/> instead of using the neural network
        /// </summary>
        /// <param name="actionsOut">Output action array.</param>
        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var input = (_inputScheme._2DTopDown.Move.ReadValue<Vector2>()).normalized;

            var continuousActions = actionsOut.ContinuousActions;
            continuousActions[0] = input.x;
            continuousActions[1] = input.y;
        }

        /// <summary>
        /// Prevent the agent from moving and taking actions
        /// </summary>
        public void FreezeAgent()
        {
            Debug.Assert(trainingMode == false, "Freeze/Unfreeze not supported in training");
            frozen = true;
            rigidbody.Sleep();
        }

        /// <summary>
        /// Resume agent movement and actions
        /// </summary>
        public void UnfreezeAgent()
        {
            Debug.Assert(trainingMode == false, "Freeze/Unfreeze not supported in training");
            frozen = false;
            rigidbody.WakeUp();
        }

        /// <summary>
        /// Reset the agents state and relevant members. Called <c>OnEpisodeBegin</c>.
        /// </summary>
        private void ResetAgent()
        {
            // Stop the agent moving.
            rigidbody.velocity = Vector3.zero;

            // Reset the agents inventory.
            inventory.ResetInventory();

            // Pass engagement of previous episode before resetting
            _simulation.pcgSystemRefactor.previousEngagement = engagement;
            engagement.ResetEngagement();
        }

        /// <summary>
        /// Reward / punish agent with reward passed.
        /// </summary>
        /// <param name="reward">Reward to be added the agent's cumulative reward and reward metric in engagement.</param>
        public void RewardPlayer(float reward)
        {
            AddReward(reward);
            engagement.SetRewardScore(currentReward);
        }

        /// <summary>
        /// Reward agent for exploring / not exploring the room.
        /// </summary>
        /// <param name="reward">Reward to be added the the agent's exploration metric in engagement.</param>
        public void RewardExploration(float reward)
        {
            engagement.AddExploration(reward);
        }

        /// <summary>
        /// Get the agent's owned inventory.
        /// </summary>
        /// <returns>Agent inventory.</returns>
        public Inventory GetPlayerInventory() { return inventory; }

        /// <summary>
        /// Get the agent's engagement metrics data.
        /// </summary>
        /// <returns>Agent engagement data.</returns>
        public EngagementMetrics GetAgentEngagement() { return engagement; }

        /// <summary>
        /// Get the agent's current position in the cell grid.
        /// </summary>
        /// <returns>Cell position on the grid, returned as <c>Vector3Int</c>.</returns>
        private Vector3Int GetAgentCellPosition()
        { return _simulation.tilemapSystem.tilemapData.floor.WorldToCell(transform.position); }

        /// <summary>
        /// Finds and updates the nearest item to the agent on the grid.
        /// </summary>
        /// <returns>Nearest item instance to the agent on the grid.</returns>
        private Item UpdateNearestItem()
        { return HelperUtilities.FindNearestTileInMap<Item>(gameObject, _tilemapCoordinates); }

        /// <summary>
        /// Finds and updates the nearest door to the agent on the grid.
        /// </summary>
        /// <returns>Nearest door instance to the agent on the grid.</returns>
        private TileDoor UpdateNearestDoor()
        { return HelperUtilities.FindNearestTileInMap<TileDoor>(gameObject, _tilemapCoordinates); }

        /// <summary>
        /// Finds and updates the nearest obstacle to the agent on the grid.
        /// </summary>
        /// <returns>Nearest obstacle instance to the agent on the grid.</returns>
        private TileCollidable UpdateNearestObstacle()
        { return HelperUtilities.FindNearestTileInMap<TileCollidable>(gameObject, _tilemapCoordinates); }

        /// <summary>
        /// Finds and updates the nearest floor to the agent on the grid.
        /// </summary>
        /// <returns>Nearest floor instance to the agent on the grid.</returns>
        private TileFloor UpdateNearestFloor()
        {
            TileFloor floor = HelperUtilities.FindNearestTileInMap<TileFloor>(gameObject, _tilemapCoordinates);
            return floor;
        }
    }
}