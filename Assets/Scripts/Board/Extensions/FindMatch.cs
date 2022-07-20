using System.Collections.Generic;
using UnityEngine;

public static class FindMatch
{
    public static List<Tile> FindMatchTiles(Tile start, Vector2 direction)
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
}
