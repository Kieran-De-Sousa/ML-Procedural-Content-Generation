// Base
using System.Collections;
using System.Collections.Generic;

// Unity
using UnityEngine;

// Unity ML Agents
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

// PCG Tilemaps
using PCG.Tilemaps;

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
        [Tooltip("Whether this is training mode or gameplay mode")]
        public bool trainingMode;

        // The rigidbody of the agent
        new public Rigidbody2D rigidbody;

        [Header("Movement")]
        public Vector2 movementSpeed = new(5.0f, 5.0f);
        private Vector2 _inputVector = Vector2.zero;
        private InputScheme _inputScheme;

        private Inventory inventory;

        private Simulation _simulation;
        private Tile[,] _tilemapCoordinates;
        private TileDoor _nearestDoor;

        // Whether the agent is frozen (intentionally not moving)
        private bool frozen = false;

        // -------------------- Methods -------------------- //
        private void Awake()
        {
            inventory = GetComponent<Inventory>();
            InitialiseInput();
        }

        private void InitialiseInput()
        {
            _inputScheme = new InputScheme();
            _inputScheme._2DTopDown.Enable();
        }

        private void OnDestroy()
        {
            _inputScheme.Dispose();
        }

        /// <summary>
        /// Initialize the agent.
        /// </summary>
        public override void Initialize()
        {
            _simulation = GetComponentInParent<Simulation>();
            _tilemapCoordinates = _simulation.pcgSystemRefactor.roomData.tilemap;

            // If not in training mode, no max step, play forever.
            if (!trainingMode)
            {
                MaxStep = 0;
            }
        }

        /// <summary>
        /// Reset the agent when an episode begins.
        /// </summary>
        public override void OnEpisodeBegin()
        {

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
        /// Collect vector observations from the environment.
        /// </summary>
        /// <param name="sensor">The vector sensor.</param>
        public override void CollectObservations(VectorSensor sensor)
        {
            // (3 observations)
            // Observe the agent's local position.
            sensor.AddObservation(transform.localPosition.normalized);

            // 3 Total Observations //
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

        public void ResetAgent()
        {

        }

        private void UpdateNearestDoor()
        {

        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reward"></param>
        public void RewardPlayer(float reward)
        {
            if (trainingMode)
            {
                AddReward(reward);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Inventory GetPlayerInventory() { return inventory; }
    }
}