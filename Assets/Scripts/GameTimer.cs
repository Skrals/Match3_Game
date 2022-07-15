using UnityEngine;
using UnityEngine.Events;

public class GameTimer : MonoBehaviour
{
    public event UnityAction<bool> GameOver;

    [SerializeField] private float _levelTime;
    [SerializeField] private float _currentTimer;

    private bool _gameOver;

    public void StartTimer(bool flag)
    {
        if (flag)
        {
            _currentTimer = _levelTime;
            _gameOver = false;
        }
    }

    private void Update()
    {
        if (_gameOver)
        {
            return;
        }

        if (_currentTimer > 0)
        {
            _currentTimer -= Time.deltaTime;
        }
        else
        {
            _gameOver = true;
            GameOver?.Invoke(_gameOver);
        }
    }
}
