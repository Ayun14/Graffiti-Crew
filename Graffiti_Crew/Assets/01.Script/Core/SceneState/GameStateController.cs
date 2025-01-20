using UnityEngine;

public enum GameState
{
    None,

    // ´ë°á¾À
    Timeline, CountDown, Fight, Finish, Result,

    // ÀÇ·Ú¾À
    Talk, Graffiti
}

public class GameStateController : Subject
{
    [SerializeField] private GameState gameState = GameState.None;
    public GameState GameState => gameState;

    private void Start()
    {
        NotifyObservers();
    }

    public void ChangeGameState(GameState newState)
    {
        if (newState == gameState) return;

        gameState = newState;
        NotifyObservers(); // °üÂûÀÚµé¿¡°Ô ¾Ë¸®±â
    }
}
