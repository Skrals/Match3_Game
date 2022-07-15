using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPanel;

    public void GameOverPanelSwitch()
    {
        _gameOverPanel.SetActive(!_gameOverPanel.activeSelf);
    }

    private void Awake()
    {
        _gameOverPanel.SetActive(false);
    }
}