using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : Board
{
    private Tile _oldSelectionTile;
    private Vector2[] _directionRay = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

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

    private void ChekSelectTile(Tile tile)
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
                if(NeighbourTile().Contains(tile))
                {
                    SwapTile(tile);
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

    private void SwapTile(Tile tile)
    {
        if (_oldSelectionTile.SpriteRenderer.sprite == tile.SpriteRenderer.sprite)
        {
            return;
        }

        Sprite cashSprite = _oldSelectionTile.SpriteRenderer.sprite;
        _oldSelectionTile.SpriteRenderer.sprite = tile.SpriteRenderer.sprite;
        tile.SpriteRenderer.sprite = cashSprite;
    }

    private List<Tile> NeighbourTile()
    {
        List<Tile> neighbours = new List<Tile>();

        for (int i = 0; i < _directionRay.Length; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(_oldSelectionTile.transform.position,_directionRay[i]);

            if(hit.collider != null )
            {
                neighbours.Add(hit.collider.gameObject.GetComponent<Tile>());
            }
        }
        return neighbours;
    }
}
