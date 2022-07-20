using System.Collections.Generic;
using UnityEngine;

public static class FindNeighbour 
{
    public static List<Tile> NeighbourTile(Vector2[] directionRay, Tile oldSelectionTile)
    {
        List<Tile> neighbours = new List<Tile>();

        for (int i = 0; i < directionRay.Length; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(oldSelectionTile.transform.position, directionRay[i]);

            if (hit.collider != null)
            {
                neighbours.Add(hit.collider.gameObject.GetComponent<Tile>());
            }
        }

        return neighbours;
    }
}