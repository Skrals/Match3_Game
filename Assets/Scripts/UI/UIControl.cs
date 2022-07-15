using UnityEngine;
using UnityEngine.Events;

public class UIControl : MonoBehaviour
{
    public event UnityAction<bool> StartGame;

    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private GameUI _gameUI;

    public void PlayButton()
    {
        UIStateSwitch();
        StartGame?.Invoke(true);
    }

    public void OkButton()
    {
        UIStateSwitch();
        GameOverPanel();
        StartGame?.Invoke(false);
    }

    public void GameOverPanel()
    {
        _gameUI.GameOverPanelSwitch();
    }

    private void Awake()
    {
        _mainMenu.gameObject.SetActive(true);
        _gameUI.gameObject.SetActive(false);
    }

    private void UIStateSwitch()
    {
        _mainMenu.gameObject.SetActive(!_mainMenu.gameObject.activeSelf);
        _gameUI.gameObject.SetActive(!_gameUI.gameObject.activeSelf);
    }
}
