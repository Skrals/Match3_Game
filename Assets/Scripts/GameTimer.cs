using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public event UnityAction<bool> GameOver;

    [SerializeField] private float _levelTime;
    [SerializeField] private float _currentTimer;
    [SerializeField] private TMP_Text _viewText;

    private bool _gameOver;
    private float _timerViewOffset = 1f;

    public void StartTimer(bool flag)
    {
        if (flag)
        {
            _currentTimer = _levelTime;
            _gameOver = false;
        }
    }

    private void FixedUpdate()
    {
        if (_gameOver)
        {
            return;
        }

        if (_currentTimer > 0)
        {
            _currentTimer -= Time.fixedDeltaTime;
            DisplayTime(_currentTimer);
        }
        else
        {
            _gameOver = true;
            GameOver?.Invoke(_gameOver);
        }
    }

    private void DisplayTime (float timer)
    {
        timer += _timerViewOffset;
        float seconds = Mathf.FloorToInt(timer);

        _viewText.text = $"Time: {seconds}";
    }
}
