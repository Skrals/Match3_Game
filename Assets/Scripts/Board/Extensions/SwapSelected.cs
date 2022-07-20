using UnityEngine;
using DG.Tweening;

public static class SwapSelected 
{
    public static void SwapTile(Tile tile, Tile oldSelectionTile, float animationSpeed)
    {
        Tile oldCashSelected = oldSelectionTile;

        Vector2 oldGridPositionCash = new Vector2(oldCashSelected.PositionX, oldCashSelected.PositionY);
        Vector3 oldPosition = oldCashSelected.transform.position;
        Vector3 currentPosition = tile.transform.position;

        oldCashSelected.transform.DOMove(currentPosition, animationSpeed, false);
        oldCashSelected.PositionX = tile.PositionX;
        oldCashSelected.PositionY = tile.PositionY;
        oldCashSelected.name = $"Tile - {oldCashSelected.PositionX}, {oldCashSelected.PositionY}";

        tile.transform.DOMove(oldPosition, animationSpeed, false);
        tile.PositionX = (int)oldGridPositionCash.x;
        tile.PositionY = (int)oldGridPositionCash.y;
        tile.name = $"Tile - {tile.PositionX}, {tile.PositionY}";
    }
}
