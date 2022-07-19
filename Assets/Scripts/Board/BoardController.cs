using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

//TODO протестировать работу, перепроверить очередь срабатывани€ асинхронных методов
//TODO набор очков за матчинг и отображение в интерфейсе
//TODO по возможности - разъединить логику класса на составл€ющие

public class BoardController : Board
{
    public event UnityAction<int> ScoreUpdate;

    [Header("Mechanism flags")]
    [SerializeField] private bool _isFindMatch = false;
    [SerializeField] private bool _isMatch = false;
    [SerializeField] private bool _isShift = false;
    [SerializeField] private bool _isSearchEmptyTiles = false;
    [SerializeField] private bool _isSwap = false;
    [SerializeField] private bool _isFindAllMachesStart = false;
    [SerializeField] private bool _isFindAllMatches = false;
    [SerializeField] private bool _isGameOver = false;

    [Header("Animation speed")]
    [SerializeField] private float _animationSpeed = 0.3f;

    private Tile _oldSelectionTile;
    private Tile _oldCashSelected;
    private bool _isEffectsPlayed = false;
    private Vector2[] _directionRay = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    private List<Task> _tasks = new List<Task>();
    private List<Task> _secondTasks = new List<Task>();

    public void OnGameOver(bool over)
    {
        _isGameOver = over;
        _isFindMatch = _isShift = _isSearchEmptyTiles = _isSwap = _isFindAllMachesStart = _isFindAllMatches = _isMatch = false;
    }

    private async void Update()
    {
        if (_isGameOver || _isShift || _isSwap || _isFindAllMatches)
        {
            return;
        }

        if (_isSearchEmptyTiles)
        {
            SearchEmptyTile();
        }

        if (_isFindAllMachesStart)
        {
            _isFindAllMachesStart = false;
            _isFindAllMatches = true;

            await FindAnotherMatches(_tilesArray);
            await Task.Delay(100);

            _isSearchEmptyTiles = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

            if (hit != false)
            {
                if (hit.collider.gameObject.TryGetComponent(out Tile tile))
                {
                    ChekSelectTile(tile);
                }
            }
        }
    }

    private async void ChekSelectTile(Tile tile)
    {
        try
        {
            if (tile == null || tile.isEmpty || _isShift || _isSwap)
            {
                return;
            }

            if (tile.isSelected)
            {
                DeselectTile(tile);
            }
            else
            {
                if (!tile.isSelected && _oldSelectionTile == null)
                {
                    Selected(tile);
                }
                else
                {
                    if (NeighbourTile().Contains(tile))
                    {
                        SwapTile(tile);

                        _isSwap = true;
                        await Task.Delay(1000);
                        _isSwap = false;

                        FindAllMatch(tile);
                        FindAllMatch(_oldSelectionTile);

                        if (!_isMatch)
                        {
                            SwapTile(tile);
                        }

                        DeselectTile(_oldSelectionTile);
                    }
                    else
                    {
                        DeselectTile(_oldSelectionTile);
                        Selected(tile);
                    }
                }
            }
        }
        catch { Debug.Log("Null from Check selection"); }
    }

    #region tile selection/deselection

    private void Selected(Tile tile)
    {
        tile.isSelected = true;
        tile.SpriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
        _oldSelectionTile = tile;
        _isMatch = false;
    }

    private void DeselectTile(Tile tile)
    {
        tile.isSelected = false;
        tile.SpriteRenderer.color = new Color(1, 1, 1);
        _isMatch = false;
        _oldSelectionTile = null;
    }

    #endregion

    #region swap tiles
    private void SwapTile(Tile tile)
    {
        _oldCashSelected = _oldSelectionTile;

        Vector2 oldGridPositionCash = new Vector2(_oldCashSelected.PositionX, _oldCashSelected.PositionY);
        Vector3 oldPosition = _oldCashSelected.transform.position;
        Vector3 currentPosition = tile.transform.position;

        _oldCashSelected.transform.DOMove(currentPosition, _animationSpeed, false);
        _oldCashSelected.PositionX = tile.PositionX;
        _oldCashSelected.PositionY = tile.PositionY;
        _oldCashSelected.name = $"Tile - {_oldCashSelected.PositionX}, {_oldCashSelected.PositionY}";

        tile.transform.DOMove(oldPosition, _animationSpeed, false);
        tile.PositionX = (int)oldGridPositionCash.x;
        tile.PositionY = (int)oldGridPositionCash.y;
        tile.name = $"Tile - {tile.PositionX}, {tile.PositionY}";
    }

    private List<Tile> NeighbourTile()
    {
        List<Tile> neighbours = new List<Tile>();

        for (int i = 0; i < _directionRay.Length; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(_oldSelectionTile.transform.position, _directionRay[i]);

            if (hit.collider != null)
            {
                neighbours.Add(hit.collider.gameObject.GetComponent<Tile>());
            }
        }

        return neighbours;
    }

    #endregion

    #region Matching

    private List<Tile> FindMatch(Tile start, Vector2 direction)
    {
        List<Tile> findTiles = new List<Tile>();
        RaycastHit2D hit = Physics2D.Raycast(start.transform.position, direction);

        while (hit.collider != null && hit.collider.gameObject.GetComponent<Tile>().SpriteRenderer.sprite == start.SpriteRenderer.sprite)
        {
            findTiles.Add(hit.collider.gameObject.GetComponent<Tile>());
            hit = Physics2D.Raycast(hit.collider.gameObject.transform.position, direction);
        }

        return findTiles;
    }

    private void DeleteSprite(Tile tile, Vector2[] dirArray)
    {
        List<Tile> findTiles = new List<Tile>();

        for (int i = 0; i < dirArray.Length; i++)
        {
            findTiles.AddRange(FindMatch(tile, dirArray[i]));
        }

        if (findTiles.Count >= 2)
        {
            for (int i = 0; i < findTiles.Count; i++)
            {
                findTiles[i].SpriteRenderer.sprite = null;
            }

            ScoreUpdate?.Invoke(findTiles.Count + 1);
            _isFindMatch = true;
        }
    }

    private async Task FindAllMatch(Tile tile)
    {
        try
        {
            if (tile.isEmpty || _isShift)
            {
                return;
            }

            DeleteSprite(tile, new Vector2[2] { Vector2.up, Vector2.down });
            DeleteSprite(tile, new Vector2[2] { Vector2.left, Vector2.right }); ;

            if (_isFindMatch)
            {
                tile.SpriteRenderer.sprite = null;
                _isFindMatch = false;
                _isMatch = true;
                _isSearchEmptyTiles = true;
            }
        }
        catch { Debug.Log("Null from findAllMatch"); }

        await Task.Yield();
    }

    private async Task FindAnotherMatches(Tile[,] array)
    {
        for (int x = 0; x < _xSize; x++)
        {
            for (int y = 0; y < _ySize; y++)
            {
                _tasks.Add(FindAllMatch(array[x, y]));
            }
        }

        await Task.WhenAll(_tasks.ToArray());
        await Task.Delay(100);

        _tasks.Clear();

        _isFindAllMatches = false;
    }

    #endregion

    #region Search empty, shift tiles, set new sprite for empty tile

    private async void SearchEmptyTile()
    {
        List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
        List<Tile> cashEmpty = new List<Tile>();
        Tile tmp;
        int j;

        for (int x = 0; x < _xSize; x++)
        {
            for (int y = 0; y < _ySize; y++)
            {
                if (_tilesArray[x, y].isEmpty)
                {
                    cashEmpty.Add(_tilesArray[x, y]);
                }
            }
        }

        for (int i = 0; i < cashEmpty.Count; i++)//sorting because a firs element of list is not minimal; crutch)))
        {
            tmp = cashEmpty[i];

            for (j = i - 1; j >= 0 && cashEmpty[j].PositionY > tmp.PositionY; j--)
            {
                cashEmpty[j + 1] = cashEmpty[j];
            }
            cashEmpty[j + 1] = tmp;
        }

        PlayEffects(cashEmpty);

        for (int i = 0; i < cashEmpty.Count; i++)
        {
            try
            {
                _tasks.Add(ShiftTiles(cashEmpty[i].PositionX, cashEmpty[i].PositionY));
                spriteRenderers.Add(_tilesArray[cashEmpty[i].IndexX, cashEmpty[i].IndexY].SpriteRenderer);
                _secondTasks.Add(SetNewTileSprite(cashEmpty[i].IndexX, cashEmpty[i].IndexY));
                await Task.Delay(10);
            }
            catch { }
        }

        try
        {
            await Task.WhenAll(_tasks.ToArray());
            await Task.WhenAll(_secondTasks.ToArray());
        }
        catch { Debug.Log("Null from SearchEmptyTile tasks"); }

        _isSearchEmptyTiles = false;
        _isEffectsPlayed = false;
        _tasks.Clear();
        _secondTasks.Clear();

        await Task.Delay(1000);

        _isFindAllMachesStart = true;
    }

    private async Task ShiftTiles(int xPos, int yPos)
    {
        _isShift = true;

        for (int y = yPos; y < _ySize - 1; y++)
        {
            Tile first = FindTile(xPos, y);
            Tile second = FindTile(xPos, y + 1);
            Vector2 cashFirstPos = new Vector2(first.PositionX, first.PositionY);
            Vector2 cashSecondPos = new Vector2(second.PositionX, second.PositionY);

            first.transform.DOMove(_cashGrid[(int)cashFirstPos.x, (int)cashFirstPos.y + 1].transform.position, _animationSpeed, false);
            second.transform.DOMove(_cashGrid[(int)cashFirstPos.x, (int)cashFirstPos.y].transform.position, _animationSpeed, false);

            first.PositionX = (int)cashSecondPos.x;
            first.PositionY = (int)cashSecondPos.y;

            second.PositionX = (int)cashFirstPos.x;
            second.PositionY = (int)cashFirstPos.y;

            await Task.Delay(10);

            if (y + 1 >= _ySize)
            {
                break;
            }
        }

        _isShift = false;

        await Task.Delay(100);
    }

    private Tile FindTile(int xPos, int yPos)
    {
        for (int x = 0; x < _xSize; x++)
        {
            for (int y = 0; y < _ySize; y++)
            {
                if (_tilesArray[x, y].PositionX == xPos && _tilesArray[x, y].PositionY == yPos)
                {
                    return _tilesArray[x, y];
                }
            }
        }

        return null;
    }

    private async Task SetNewTileSprite(int indexX, int indexY)
    {
        try
        {
            await Task.Delay(100);
            List<Sprite> sprites = new List<Sprite>();
            sprites.AddRange(_tileSprites);

            _tilesArray[indexX, indexY].SpriteRenderer.sprite = sprites[Random.Range(0, sprites.Count)];
        }
        catch { Debug.Log("Null from SetNewTileSprite()"); }
    }
    #endregion

    private void PlayEffects(List<Tile> tiles)
    {
        if (!_isEffectsPlayed)
        {
            foreach (Tile tile in tiles)
            {
                tile.PlayVFXDestroy();
            }
            _isEffectsPlayed = true;
        }
    }
}