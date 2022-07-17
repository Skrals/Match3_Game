using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BoardSettings BoardSetting;

    private Board _board;

    public void StartGame()
    {
        Board.BoardInstance.SetValue(BoardSetting.XSize, BoardSetting.YSize, BoardSetting.TileGameObject, BoardSetting.TileSprite,BoardSetting.GridPointTemplate);
        _board = Board.BoardInstance;
    }

    public void EndGame()
    {
        if (_board != null)
        {
            _board.ClearBoard();
        }
    }
}