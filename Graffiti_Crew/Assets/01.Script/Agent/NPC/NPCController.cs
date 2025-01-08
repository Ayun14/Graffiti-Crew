using System;
using UnityEngine;
using UnityEngine.AI;

public enum NPCStateEnum
{
    Idle,
    Talk
}

public class NPCController : Agent
{
    public Player player { get; private set; }
    public NPCStateMachine StateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new NPCStateMachine();
        player = FindAnyObjectByType<Player>();

        foreach (NPCStateEnum stateEnum in Enum.GetValues(typeof(NPCStateEnum)))
        {
            string typeName = stateEnum.ToString();
            try
            {
                Type t = Type.GetType($"NPC{typeName}State");
                NPCState playerState = Activator.CreateInstance(
                                    t, this, StateMachine, typeName) as NPCState;
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
        StateMachine.Initialize(NPCStateEnum.Idle, this);
    }

    protected void Update()
    {
        StateMachine.CurrentState.UpdateState();
    }
}
