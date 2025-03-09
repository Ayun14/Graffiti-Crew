using System;
using UnityEngine;

public enum GameState
{
    None,

    // ����
    Loding, Timeline, CountDown, Fight, Finish, Result,

    // �Ƿھ�
    // Loding -> Talk -> Graffiti -> Result
    Talk, Graffiti,

    // Ʃ�丮��
    // Dialogue -> Tutorial -> Dialogue -> Dialogue
    Dialogue, Tutorial
}

public class GameStateController : Subject
{
    public Action OnBlindEvent; // �þ� ����
    public Action OnNodeFailEvent; // ��� ����
    public Action OnRivalCheckEvent; // ���̹��� ����
    public Action OnSprayEmptyEvent; // �������� ��� �Ҹ�
    public Action OnSprayChangeEvent; // �������� ���� ����

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

        Debug.Log("���� ���� ���� : " + newState.ToString());
        gameState = newState;
        NotifyObservers(); // �����ڵ鿡�� �˸���
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
