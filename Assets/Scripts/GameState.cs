using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField] private UIControl _uIControl;
    [SerializeField] private GameObject _game;
    [SerializeField] private GameTimer _timer;
    [SerializeField] private GameManager _manager;
    [SerializeField] private BoardController _boardController;

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

        if (start)
        {
            _manager.StartGame();
        }
        _boardController.OnGameOver(!start);
        _timer.StartTimer(start);
    }

    private void OnGameOver(bool over)
    {
        _boardController.OnGameOver(over);
        _uIControl.GameOverPanel();
        _manager.EndGame();
    }
}
