using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

namespace ML
{
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

        // Whether the agent is frozen (intentionally not moving)
        private bool frozen = false;


        // -------------------- Methods -------------------- //
        private void Awake()
        {
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

        private void Update()
        {
            _inputVector = (_inputScheme._2DTopDown.Move.ReadValue<Vector2>()).normalized;
        }

        private void FixedUpdate()
        {
            rigidbody.MovePosition(rigidbody.position + (_inputVector * movementSpeed *Time.fixedDeltaTime));
        }

        /// <summary>
        /// Initialize the agent.
        /// </summary>
        public override void Initialize() {}

        /// <summary>
        /// Reset the agent when an episode begins.
        /// </summary>
        public override void OnEpisodeBegin() {}

        /// <summary>
        /// Called when an action is received from either the player input or the neural network.
        ///
        /// actions.ContinuousActions[i] represents:
        /// Index 0: ACTION HERE
        /// </summary>
        /// <param name="actions">The actions to take.</param>
        public override void OnActionReceived(ActionBuffers actions)
        {
        }

        /// <summary>
        /// Collect vector observations from the environment.
        /// </summary>
        /// <param name="sensor">The vector sensor.</param>
        public override void CollectObservations(VectorSensor sensor) {}

        /// <summary>
        /// When Behavior Type is set to "Heuristic Only" on the agent's Behavior Parameters,
        /// this function will be called. Its return values will be fed into
        /// <see cref="OnActionReceived(ActionBuffers)"/> instead of using the neural network
        /// </summary>
        /// <param name="actionsOut">Output action array.</param>
        public override void Heuristic(in ActionBuffers actionsOut)
        {
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
    }
}