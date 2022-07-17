using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BoardSettings
{
    public int XSize, YSize;
    public Tile TileGameObject;
    public List<Sprite> TileSprite;
    public GameObject GridPointTemplate;
}
