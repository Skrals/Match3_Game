using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class BoardController : Board
{
    protected Tile _oldSelectionTile;
    protected Tile _oldCashSelected;
    protected Vector2[] _directionRay = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    protected bool isFindMatch = false;
    protected bool isShift = false;
    private bool isSearchEmptyTiles = false;
    private Vector2 _emptyCashed;

    private void Update()
    {
        if (isSearchEmptyTiles)
        {
            SearchEmptyTile();
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
        if (tile.isEmpty || isShift)
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
                    await Task.Delay(2000);

                    FindAllMatch(tile);
                    FindAllMatch(_oldSelectionTile);
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

    #region tile selection/deselection

    private void Selected(Tile tile)
    {
        tile.isSelected = true;
        tile.SpriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
        _oldSelectionTile = tile;
    }

    private void DeselectTile(Tile tile)
    {
        tile.isSelected = false;
        tile.SpriteRenderer.color = new Color(1, 1, 1);
        _oldSelectionTile = null;
    }

    #endregion

    #region swap tiles
    private void SwapTile(Tile tile)
    {
        if (_oldSelectionTile.SpriteRenderer.sprite == tile.SpriteRenderer.sprite)
        {
            return;
        }

        _oldCashSelected = _oldSelectionTile;

        Vector3 oldPosition = _oldCashSelected.transform.position;
        Vector3 currentPosition = tile.transform.position;

        _oldCashSelected.transform.DOMove(currentPosition, 1, false);
        tile.transform.DOMove(oldPosition, 1, false);
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
            isFindMatch = true;
        }
    }

    private void FindAllMatch(Tile tile)
    {
        if (tile.isEmpty)
        {
            return;
        }

        DeleteSprite(tile, new Vector2[2] { Vector2.up, Vector2.down });
        DeleteSprite(tile, new Vector2[2] { Vector2.left, Vector2.right }); ;

        if (isFindMatch)
        {
            tile.SpriteRenderer.sprite = null;
            isFindMatch = false;
            isSearchEmptyTiles = true;
        }

    }

    private void FindAnotherMatches(Tile[,] array)
    {
        for (int x = 0; x < _xSize; x++)
        {
            for (int y = 0; y < _ySize; y++)
            {
                FindAllMatch(array[x, y]);
            }
        }
    }

    #endregion

    #region New tile

    private void SearchEmptyTile()
    {
        for (int x = 0; x < _xSize; x++)
        {
            for (int y = 0; y < _ySize; y++)
            {
                if (_tilesArray[x, y].isEmpty)
                {
                    //ToDO анимация падения элементов вниз
                }
            }
        }
        isSearchEmptyTiles = false;
    }

    //private void ShiftTileDown(int xPos, int yPos)
    //{
    //    isShift = true;
    //    List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    //    for (int y = yPos; y < _ySize; y++)
    //    {
    //        Tile tile = _tilesArray[xPos, yPos];

    //        if (tile.isEmpty)
    //        {
    //            spriteRenderers.Add(tile.SpriteRenderer);
    //        }
    //    }

    //    SetNewTileSprite(xPos, spriteRenderers);
    //    isShift = false;
    //}

    //private void SetNewTileSprite(int xPos, List<SpriteRenderer> renderer)
    //{
    //    for (int y = 0; y < renderer.Count - 1; y++)
    //    {
    //        renderer[y].sprite = renderer[y + 1].sprite;
    //        renderer[y + 1].sprite = GetNewSptite(xPos, _ySize - 1);
    //    }
    //}

    //private Sprite GetNewSptite(int xPos, int yPos)
    //{
    //    List<Sprite> sprites = new List<Sprite>();
    //    sprites.AddRange(_tileSprites);

    //    if (xPos > 0)
    //    {
    //        sprites.Remove(_tilesArray[xPos - 1, yPos].SpriteRenderer.sprite);
    //    }

    //    if (xPos < _xSize - 1)
    //    {
    //        sprites.Remove(_tilesArray[xPos + 1, yPos].SpriteRenderer.sprite);
    //    }

    //    if (yPos > 0)
    //    {
    //        sprites.Remove(_tilesArray[xPos, yPos - 1].SpriteRenderer.sprite);
    //    }

    //    return sprites[Random.Range(0, sprites.Count)];
    //}

    #endregion
}
