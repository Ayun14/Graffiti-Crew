using System;
using UnityEngine;
using UnityEngine.AI;

public enum PlayerStateEnum
{
    Idle,
    Run
}

public class Player : Agent
{
    [Header("Setting Values")]
    public float moveSpeed = 12f;

    public PlayerStateMachine StateMachine { get; private set; }
    public NavMeshAgent NavMeshAgent { get; private set; }
    [SerializeField] private PlayerInput _playerInput;
    public PlayerInput PlayerInput => _playerInput;
    public PlayerVFX PlayerVFXCompo => VFXCompo as PlayerVFX;

    protected override void Awake()
    {
        base.Awake();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        StateMachine = new PlayerStateMachine();

        foreach(PlayerStateEnum stateEnum in Enum.GetValues(typeof(PlayerStateEnum)))
        {
            string typeName = stateEnum.ToString();
            try
            {
                Type t = Type.GetType($"Player{typeName}State");
                PlayerState playerState = Activator.CreateInstance(
                                    t, this, StateMachine, typeName) as PlayerState;
                StateMachine.AddState(stateEnum, playerState);
            }catch(Exception ex)
            {
                Debug.LogError($"{typeName} is loading error, check Message {ex.Message}");
            }
        }
    }

    protected void Start()
    {
        StateMachine.Initialize(PlayerStateEnum.Idle, this);
    }

    protected void Update()
    {
        StateMachine.CurrentState.UpdateState();
    }

    public void PlayBladeVFX()
    {
        //PlayerVFXCompo.PlayBladeVFX(currentComboCounter);
    }
}
