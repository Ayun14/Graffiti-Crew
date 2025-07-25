using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public enum PlayerStateEnum
{
    Idle,
    Run,
    Interaction,
    Computer,
    Talk,
    Sit,
    SitStay,
    Mirror,
    BoomBox
}

public class Player : Agent
{
    [Header("Setting Values")]
    public float moveSpeed = 4f;

    public DialogueController dialogueController;
    public DialogueUIController dialogueUIController;
    public HangOutPlayerDataSO playerData;

    public PlayerStateMachine StateMachine { get; private set; }
    public NavMeshAgent NavMeshAgent { get; private set; }
    public INavigationable MovementCompo { get; protected set; }
    public PlayerInput PlayerInput { get; protected set; }
    public PlayerVFX PlayerVFXCompo => VFXCompo as PlayerVFX;

    [HideInInspector] public InteractionObject CurrentInteractionObject;

    protected override void Awake()
    {
        HangOutEvent.SetPlayerMovementEvent += HandlePlayerMove;

        base.Awake();

        PlayerInput = GetComponent<PlayerInput>();
        MovementCompo = GetComponent<INavigationable>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        StateMachine = new PlayerStateMachine();

        foreach (PlayerStateEnum stateEnum in Enum.GetValues(typeof(PlayerStateEnum)))
        {
            string typeName = stateEnum.ToString();
            try
            {
                Type t = Type.GetType($"Player{typeName}State");
                PlayerState playerState = Activator.CreateInstance(
                                    t, this, StateMachine, typeName) as PlayerState;
                StateMachine.AddState(stateEnum, playerState);
            }
            catch (Exception ex)
            {
                Debug.LogError($"{typeName} is loading error, check Message {ex.Message}");
            }
        }
    }

    protected void Start()
    {
        transform.position = playerData.playerPosition;
        transform.rotation = playerData.playerRotation;

        NavMeshAgent.speed = moveSpeed;
        StateMachine.Initialize(PlayerStateEnum.Idle, this);
    }

    private void OnDisable()
    {
        HangOutEvent.SetPlayerMovementEvent -= HandlePlayerMove;
    }

    private void HandlePlayerMove(bool isMove)
    {
        PlayerInput.SetPlayerInput(isMove);
    }

    private void OnApplicationQuit()
    {
        playerData.playerPosition = transform.position;
        playerData.playerRotation = transform.rotation;
    }

    protected void Update()
    {
        StateMachine.CurrentState.UpdateState();
    }

    public NPC GetNPC()
    {
        if (CurrentInteractionObject == null) return null;

        if (CurrentInteractionObject.TryGetComponent<NPC>(out NPC npc))
            return npc;
        else
            return null;
    }
}
