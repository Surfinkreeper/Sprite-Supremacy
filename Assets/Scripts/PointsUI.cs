using UnityEngine;
using TMPro;

public class PointsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    /// <summary>
    /// Updates the text UI with the new number of points.
    /// </summary>
    /// <param name="points">The number of points the player has.</param>
    public void ChangePoints(int points)
    {
        _text.text = "Points: " + points;
    }
}
