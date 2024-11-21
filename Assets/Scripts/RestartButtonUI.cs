using UnityEngine;

public class RestartButtonUI : MonoBehaviour
{
    [SerializeField] private GameController _gameController;

    /// <summary>
    /// Display the Restart button when the game is over.
    /// </summary>
    public void SetGameOver ()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Hides the Restart button when player clicks it.
    /// Lets GameController know to restart game.
    /// </summary>
    public void ClickRestart ()
    {
        _gameController.StartGame();
        gameObject.SetActive(false);
    }
}
