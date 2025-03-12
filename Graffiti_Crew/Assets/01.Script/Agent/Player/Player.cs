using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public enum PlayerStateEnum
{
    Idle,
    Run,
    Interaction,
    Computer,
    NPC,
    Sit
}

public class Player : Agent
{
    [Header("Setting Values")]
    public float moveSpeed = 4f;

    public DialogueUIController dialogueUIController;
    public PlayableDirector computerTimeline;
    public HangOutPlayerDataSO playerData;

    public PlayerStateMachine StateMachine { get; private set; }
    public NavMeshAgent NavMeshAgent { get; private set; }
    public INavigationable MovementCompo { get; protected set; }
    public PlayerInput PlayerInput { get; protected set; }
    public PlayerVFX PlayerVFXCompo => VFXCompo as PlayerVFX;

    [HideInInspector] public InteractionObject CurrentInteractionObject;

    protected override void Awake()
    {
        base.Awake();
        PlayerInput = GetComponent<PlayerInput>();
        MovementCompo = GetComponent<INavigationable>();
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
        transform.position = playerData.playerPosition;

        NavMeshAgent.speed = moveSpeed;
        StateMachine.Initialize(PlayerStateEnum.Idle, this);
    }

    private void OnApplicationQuit()
    {
        playerData.playerPosition = transform.position;
    }

    protected void Update()
    {
        StateMachine.CurrentState.UpdateState();
    }

    public NPC GetNPC()
    {
        if(CurrentInteractionObject == null) return null;

        if (CurrentInteractionObject.TryGetComponent<NPC>(out NPC npc))
            return npc;
        else
            return null;
    }
}
