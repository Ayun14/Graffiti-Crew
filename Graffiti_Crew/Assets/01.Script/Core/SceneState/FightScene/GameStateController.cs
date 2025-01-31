using System;
using UnityEngine;

public enum GameState
{
    None,

    // 대결씬
    Loding, Timeline, CountDown, Fight, Finish, Result,

    // 의뢰씬
    Talk, Graffiti
}

public class GameStateController : Subject
{
    public Action OnBlindEvent;
    public Action OnRivalCheckEvent; // 라이벌의 견제

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

        Debug.Log("게임 상태 변경 : " + newState.ToString());
        gameState = newState;
        NotifyObservers(); // 관찰자들에게 알리기
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
