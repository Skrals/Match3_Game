using UnityEngine;

public class Tile : MonoBehaviour
{
    [field: SerializeField] public SpriteRenderer SpriteRenderer { get; set; }
    [field: SerializeField] public bool isSelected { get; set; }
    public bool isEmpty { get { return SpriteRenderer.sprite == null ? true : false; } }
    [field: SerializeField] public int PositionX { get; set; }
    [field: SerializeField] public int PositionY { get; set; }
    [field: SerializeField] public int IndexX { get; private set; }
    [field: SerializeField] public int IndexY { get; private set; }

    [SerializeField] private ParticleSystem _destroyVFX;

    private float _effectPositionZOffset = 1f;

    public void PlayVFXDestroy()
    {
        Instantiate(_destroyVFX, new Vector3(transform.position.x,transform.position.y,transform.position.z-_effectPositionZOffset),Quaternion.identity);
    }

    public void SetTileIndex(int x, int y)
    {
        IndexX = x;
        IndexY = y;
    }
}
