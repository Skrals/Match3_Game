using System.Collections.Generic;

public static class EffectsPlayer 
{
    public static void PlayDestoryEffects(List<Tile> tiles, ref bool effectFlag)
    {
        if (!effectFlag)
        {
            foreach (Tile tile in tiles)
            {
                tile.PlayVFXDestroy();
            }
            effectFlag = true;
        }
    }
}
