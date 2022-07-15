using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField] private UIControl _uIControl;
    [SerializeField] private GameObject _game;
    [SerializeField] private GameTimer _timer;

    private void OnEnable()
    {
        _uIControl.StartGame += OnStartGame;
        _timer.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        _uIControl.StartGame -= OnStartGame;
        _timer.GameOver -= OnGameOver;
    }

    private void OnStartGame(bool start)
    {
        _game.SetActive(start);
        _timer.StartTimer();
    }

    private void OnGameOver(bool over)
    {
       
    }
}
