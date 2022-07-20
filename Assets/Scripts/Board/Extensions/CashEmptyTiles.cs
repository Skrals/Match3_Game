using System.Collections.Generic;

public static class CashEmptyTiles
{
    public static void CashEmpty(ref List<Tile> cashEmpty, Tile[,] tilesArray)
    {
        Tile tmp;
        int j;

        for (int x = 0; x < tilesArray.GetLength(0); x++)
        {
            for (int y = 0; y < tilesArray.GetLength(1); y++)
            {
                if (tilesArray[x, y].isEmpty)
                {
                    cashEmpty.Add(tilesArray[x, y]);
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
    }
}
