using UnityEngine;

public class Tile : MonoBehaviour
{
    [field: SerializeField] public SpriteRenderer SpriteRenderer { get; set; }
    [field: SerializeField] public bool isSelected { get; set; }
    public bool isEmpty { get { return SpriteRenderer.sprite == null ? true : false; } }
    [field: SerializeField] public int PositionX { get; set; }
    [field: SerializeField] public int PositionY { get; set; }

    [field: SerializeField] public int IndexX { get; set; }
    [field: SerializeField] public int IndexY { get; set; }
}
