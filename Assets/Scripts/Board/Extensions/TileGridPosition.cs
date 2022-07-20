public static class TileGridPosition
{
    public static Tile FindGridPosition(int xPos, int yPos, Tile[,] tilesArray)
    {
        for (int x = 0; x < tilesArray.GetLength(0); x++)
        {
            for (int y = 0; y < tilesArray.GetLength(1); y++)
            {
                if (tilesArray[x, y].PositionX == xPos && tilesArray[x, y].PositionY == yPos)
                {
                    return tilesArray[x, y];
                }
            }
        }

        return null;
    }
}
