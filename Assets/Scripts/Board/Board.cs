using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board BoardInstance;

    protected int _xSize, _ySize;
    protected List<Sprite> _tileSprites = new List<Sprite>();
    protected Tile[,] _tilesArray;

    private Tile _tileGameObject;

    public void SetValue(int xSize, int ySize, Tile tile, List<Sprite> tileSprites)
    {
        _xSize = xSize;
        _ySize = ySize;
        _tileGameObject = tile;
        _tileSprites = tileSprites;

        CreateBoard();
    }

    public void ClearBoard()
    {
        for (int x = 0; x < _tilesArray.GetLength(0); x++)
        {
            for (int y = 0; y < _tilesArray.GetLength(1); y++)
            {
                Destroy(_tilesArray[x, y].gameObject);
            }
        }
    }

    private void CreateBoard()
    {
        Tile[,] tileArray = new Tile[_xSize, _ySize];
        _tilesArray = tileArray;
        float xPos = transform.position.x;
        float yPos = transform.position.y;
        Vector2 tileSize = _tileGameObject.SpriteRenderer.bounds.size;

        Sprite currentSprite = null;

        for (int x = 0; x < tileArray.GetLength(0); x++)
        {
            for (int y = 0; y < tileArray.GetLength(1); y++)
            {
                Tile tile = Instantiate(_tileGameObject, transform.position, Quaternion.identity);
                tile.transform.position = new Vector3(xPos + (tileSize.x * x), yPos + (tileSize.y * y));
                tile.transform.parent = transform;

                tileArray[x, y] = tile;

                List<Sprite> tempSprites = new List<Sprite>();
                tempSprites.AddRange(_tileSprites);
                tempSprites.Remove(currentSprite);

                if(x>0)
                {
                    tempSprites.Remove(tileArray[x - 1, y].SpriteRenderer.sprite);
                }

                tile.SpriteRenderer.sprite = tempSprites[Random.Range(0, tempSprites.Count)];
                currentSprite = tile.SpriteRenderer.sprite;

                _tilesArray[x, y] = tileArray[x, y];
            }
        }
    }

    private void Awake()
    {
        BoardInstance = this;
    }
}
