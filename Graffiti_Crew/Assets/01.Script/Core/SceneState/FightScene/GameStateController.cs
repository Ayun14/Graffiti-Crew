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
    public Action OnRivalCheckEvent; // ���̹��� ����

    [SerializeField] private GameState gameState = GameState.None;
    public GameState GameState => gameState;

    private bool _isBlind = false;
    public bool IsBlind => _isBlind;

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

    public void InvokeBlindEvent()
    {
        OnBlindEvent?.Invoke();
    }

    public void SetIsBlind(bool isBlind) => _isBlind = isBlind;

    public void InvokeRivalCheckEvent()
    {
        OnRivalCheckEvent?.Invoke();
    }
}
