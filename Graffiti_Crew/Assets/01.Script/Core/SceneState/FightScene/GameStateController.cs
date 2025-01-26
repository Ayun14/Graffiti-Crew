using System;
using UnityEngine;

public enum GameState
{
    None,

    // ����
    Loding, Timeline, CountDown, Fight, Finish, Result,

    // �Ƿھ�
    Talk, Graffiti
}

public class GameStateController : Subject
{
    public Action OnBlindEvent;

    [SerializeField] private GameState gameState = GameState.None;
    public GameState GameState => gameState;

    private void Start()
    {
        NotifyObservers();
    }

    public void ChangeGameState(GameState newState)
    {
        if (newState == gameState) return;

        Debug.Log("���� ���� ���� : " + newState.ToString());
        gameState = newState;
        NotifyObservers(); // �����ڵ鿡�� �˸���
    }
}
