using System;
using UnityEngine;
using UnityEngine.AI;

public enum PlayerStateEnum
{
    Idle,
    Run,
    Interaction,
    Computer,
    NPC
}

public class Player : Agent
{
    [Header("Setting Values")]
    public float moveSpeed = 4f;
    public DialogueUIController dialogueUIController;

    [SerializeField] private PlayerInput _playerInput;

    public PlayerStateMachine StateMachine { get; private set; }
    public NavMeshAgent NavMeshAgent { get; private set; }
    public INavigationable MovementCompo { get; protected set; }
    public PlayerInput PlayerInput => _playerInput;
    public PlayerVFX PlayerVFXCompo => VFXCompo as PlayerVFX;

    [HideInInspector] public InteractionObject CurrentInteractionObject;

    protected override void Awake()
    {
        base.Awake();
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
        NavMeshAgent.speed = moveSpeed;
        StateMachine.Initialize(PlayerStateEnum.Idle, this);
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

    public void PlayBladeVFX()
    {
        //PlayerVFXCompo.PlayBladeVFX(currentComboCounter);
    }
}
