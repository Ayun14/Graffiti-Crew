using System;
using UnityEngine;

public enum GameState
{
    None,

    // 대결씬
    Loding, Timeline, CountDown, Fight, Finish, Result,

    // 의뢰씬
    // Loding -> Talk -> Graffiti -> Result
    Talk, Graffiti,

    // 튜토리얼
    // Dialogue -> Tutorial -> Dialogue -> Dialogue
    Dialogue, Tutorial
}

public class GameStateController : Subject
{
    public Action OnBlindEvent; // 시야 방해
    public Action OnNodeFailEvent; // 노드 실패
    public Action OnRivalCheckEvent; // 라이벌의 견제
    public Action OnSprayEmptyEvent; // 스프레이 모두 소모
    public Action OnSprayChangeEvent; // 스프레이 갈기 성공

    [SerializeField] private GameState gameState = GameState.None;
    public GameState GameState => gameState;

    private bool _isBlind = false;
    public bool IsBlind => _isBlind;

    private bool _isSprayEmpty = false;
    public bool IsSprayEmpty => _isSprayEmpty;

    private bool _isPlayerWin = false;
    public bool IsPlayerWin => _isPlayerWin;

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

    #region Set Bool

    public void SetIsBlind(bool isBlind) => _isBlind = isBlind;
    public void SetWhoIsWin(bool isPlayerWind) => _isPlayerWin = isPlayerWind;
    public void SetIsSprayEmpty(bool isSprayEmpty) => _isSprayEmpty = isSprayEmpty;

    #endregion

    #region Invoke Event

    public void InvokeBlindEvent() => OnBlindEvent?.Invoke();

    public void InvokeNodeFailEvent() => OnNodeFailEvent?.Invoke();

    public void InvokeRivalCheckEvent() => OnRivalCheckEvent?.Invoke();

    public void InvokeSprayEmptyEvent() => OnSprayEmptyEvent?.Invoke();

    public void InvokeSprayChangeEvent() => OnSprayChangeEvent?.Invoke();

    #endregion
}
