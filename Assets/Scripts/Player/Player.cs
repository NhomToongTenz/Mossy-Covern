using Player.Data;
using Player.SateMachine.SupState;
using Player.State;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        public PlayerStateMachine StateMachine { get; private set; }

        public PlayerIdleState IdleState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }

        public Animator Anim { get; private set; }
        public PlayerInputHandler InputHandler { get; private set; }
        public Rigidbody2D RB { get; private set; }
        public Vector2 CurrentVelocity { get; private set; }


        [SerializeField] private PlayerData playerData;

        private Vector2 workspace;

        private void Awake()
        {
            StateMachine = new PlayerStateMachine();

            IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
            MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        }

        void Start()
        {
            Anim = GetComponentInChildren<Animator>();
            InputHandler = GetComponent<PlayerInputHandler>();
            RB = GetComponent<Rigidbody2D>();

            StateMachine.Initialize(IdleState);
        }

        void Update()
        {
            CurrentVelocity = RB.velocity;
            StateMachine.CurrentState.LogicUpdate();
        }

        void FixedUpdate()
        {
            StateMachine.CurrentState.PhysicsUpdate();
        }

        public void SetVelocityX(float velocity)
        {
            workspace.Set(velocity,CurrentVelocity.y);
            RB.velocity = workspace;
            CurrentVelocity = workspace;
        }


    }
}