using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private int _currentScore;

    [Header("Score settins for match x3, x4, x5 and more")]
    [SerializeField] private int _scoreFor3 = 10;
    [SerializeField] private int _scoreFor4 = 15;
    [SerializeField] private int _scoreFor5 = 20;


    public void OnMatchFound(int count)
    {
        _currentScore += CalculateScore(count);
        ShowScore(_currentScore);
    }

    public void ClearScore()
    {
        _currentScore = 0;
        ShowScore(_currentScore);
    }

    private int CalculateScore(int count)
    {
        switch (count)
        {
            case 3:
                return count * _scoreFor3;
            case 4:
                return count * _scoreFor4;
            case 5:
                return count * _scoreFor5;
            default:
                return count * _scoreFor5; ;
        }
    }

    private void ShowScore(int score)
    {
        _scoreText.text = score.ToString();
    }
}
