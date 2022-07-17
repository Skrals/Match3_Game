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

    private void Update()
    {
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
        if (tile.isEmpty)
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
                    await Task.Delay(1000);

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

    #region Mathc3 and delete matching

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
        }
    }

    #endregion
}
