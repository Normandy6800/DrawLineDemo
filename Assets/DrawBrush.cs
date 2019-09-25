using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DrawBrush : IDraw
{
    [SerializeField] Texture2D _texture = null;
    [SerializeField] Color _cor = Color.black;
    [SerializeField][Range(1,10)] float _size = 1;

    public Texture2D texture { set { _texture = value; UpdateTexutre(); } get { if (null == _texture) { UpdateTexutre(); } return _texture; } }
    public Color color { set { _cor = value; UpdateTexutre(); } get { return _cor; } }
    public float size { set { _size = value; UpdateTexutre(); } get { return _size; } }
    public int width { get{ return _texture.width; } }
    public int height { get { return _texture.height; } }


    public void UpdateTexutre()
    {
        _texture = new Texture2D(Mathf.CeilToInt(3 * _size),Mathf.CeilToInt(3 * _size));
        for (int x = 0; x < _texture.width; x++)
        {
            for (int y = 0; y < _texture.height; y++)
            {
                _texture.SetPixel(x, y, _cor);
            }
        }
    }

    public void Draw(Texture2D canvas, Color bgCor, Vector2 pos)
    {
        var px = (int)pos.x;
        var py = (int)pos.y;
        var lenW = Mathf.FloorToInt(texture.width / 2);
        var lenH = Mathf.FloorToInt(texture.height / 2);

        for (int w = -lenW; w <= lenW; w++)
        {
            for (int h = -lenH; h <= lenH; h++)
            {
                var x = px + w;
                var y = py + h;
                var cor = texture.GetPixel(w + lenW, h + lenH);
                if (x >= 0 && x < canvas.width && y >= 0 && y < canvas.height)
                {
                    canvas.SetPixel(x, y, cor);
                }
            }
        }
    }
}
