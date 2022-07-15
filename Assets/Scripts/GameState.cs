using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField] private GameObject _game;
    [SerializeField] private UIControl _uIControl;

    private void OnEnable() => _uIControl.StartGame += OnStartGame;

    private void OnDisable() => _uIControl.StartGame -= OnStartGame;

    private void OnStartGame(bool start)
    {
        _game.SetActive(start);
    }
}
