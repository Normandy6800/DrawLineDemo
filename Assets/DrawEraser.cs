using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DrawEraser : IDraw
{
    [SerializeField] [Range(1, 10)] float _size = 1;
    Texture2D _texture = null;

    public float size { set { _size = value; UpdateTexutre(); } get { return _size; } }
    public int width { get { return _texture.width; } }
    public int height { get { return _texture.height; } }


    public void UpdateTexutre()
    {
        _texture = new Texture2D(Mathf.CeilToInt(3 * _size), Mathf.CeilToInt(3 * _size));
        for (int x = 0; x < _texture.width; x++)
        {
            for (int y = 0; y < _texture.height; y++)
            {
                _texture.SetPixel(x,y,Color.white);
            }
        }
    }

    public void Draw(Texture2D canvas, Color bgCor, Vector2 pos)
    {
        if (null == _texture) { UpdateTexutre(); }

        var px = (int)pos.x;
        var py = (int)pos.y;
        var lenW = Mathf.FloorToInt(_texture.width / 2);
        var lenH = Mathf.FloorToInt(_texture.height / 2);

        for (int w = -lenW; w <= lenW; w++)
        {
            for (int h = -lenH; h <= lenH; h++)
            {
                var x = px + w;
                var y = py + h;
                if (x >= 0 && x < canvas.width && y >= 0 && y < canvas.height)
                {
                    canvas.SetPixel(x, y, bgCor);
                }
            }
        }
    }
}
