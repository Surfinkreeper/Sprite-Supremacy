using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinUI : MonoBehaviour
{
    /// <summary>
    /// Display the Win text when the player wins the game.
    /// </summary>
    public void SetYouWin ()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Hides the You win text when the player restarts the game.
    /// </summary>
    public void HideYouWin ()
    {
        gameObject.SetActive(false);
    }
}
