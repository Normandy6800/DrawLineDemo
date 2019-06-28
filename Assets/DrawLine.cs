using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawLine : MonoBehaviour {

    struct Point
    {
        public int x;
        public int y;

        public Point(int x,int y) { this.x = x; this.y = y; }
    }

    class BrushPoint
    {
        private List<Point> _point = new List<Point>();

        public int Count { get { return _point.Count; } }

        public List<Point> List { get { return _point; } }

        public Point this[int index]{ get { return _point[index]; } }

        public void Add(Point p)
        {
            _point.Add(p); 
        }

        public void Clear()
        {
            _point.Clear(); 
        }

        public static BrushPoint Lerp(BrushPoint a, BrushPoint b, float t)
        {
            BrushPoint bt = new BrushPoint();
            var num = Mathf.Min(a.Count,b.Count);
            for (int i = 1; i < num; i++)
            {
                int temX = (int)Mathf.Lerp(a[i].x, b[i].x, t);
                int temY = (int)Mathf.Lerp(a[i].y, b[i].y, t);
                bt.Add(new Point(temX,temY));
            }
            return bt;
        }
    }

    [SerializeField] [Range(1,100)] int _brush = 10;
    [SerializeField] Color _backGroundCor = Color.black;
    [SerializeField] Color _drawCor = Color.red;
    [SerializeField] [Range(1,100)]int _smooth = 50;

    RawImage _image = null;
    Texture2D _canvasTexture = null;
    Dictionary<BrushPoint, int> _drawPointDic = new Dictionary<BrushPoint, int>();
    List<BrushPoint> _drawPointList = new List<BrushPoint>();
    bool _ispress = false;


	void Start () 
    {
        _image = transform.GetComponent<RawImage>();
        _canvasTexture = new Texture2D((int)_image.rectTransform.rect.width, (int)_image.rectTransform.rect.height);
        _image.texture = _canvasTexture;
        Clear();
    }

    void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _ispress = true;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            _ispress = false;
            _drawPointDic.Clear();
            _drawPointList.Clear();
            Clear();
        }
        if(_ispress)
        {
            var point = new Point((int)Input.mousePosition.x, (int)Input.mousePosition.y);
            var bt = GetBrushpixels(point, _canvasTexture, _brush);
            if (!_drawPointDic.ContainsKey(bt))
            {
                _drawPointDic.Add(bt, 0);
                _drawPointList.Add(bt);
            }
            Draw();
        }
    }

    // 绘图
    void Draw()
    {
        for (int i = 1; i < _drawPointList.Count; i++)
        {
            for (int j = 0; j < _smooth; j++)
            {
                var bt = BrushPoint.Lerp(_drawPointList[i - 1], _drawPointList[i], 1f * j / (_smooth - 1));
                foreach (var p in bt.List)
                {
                    if (IsInCanvas(_image.rectTransform, p))
                    {
                        int x = p.x - (int)(_image.rectTransform.position.x - _image.rectTransform.rect.width / 2);
                        int y = p.y - (int)(_image.rectTransform.position.y + _image.rectTransform.rect.height / 2);
                        _canvasTexture.SetPixel(x, y, _drawCor);
                    }
                }
            }
        }
        _canvasTexture.Apply();

    }

    // 清空画布
    void Clear()
    {
        for (int x = 0; x < _canvasTexture.width; x++)
        {
            for (int y = 0; y < _canvasTexture.height; y++)
            {
                //_canvasTexture.SetPixel(x,y,_backGroundCor);
            }
        }

        for (int x = 0; x < 200; x++)
        {
            _canvasTexture.SetPixel(x, 0, _backGroundCor);
            _canvasTexture.SetPixel(0, x, _backGroundCor);
        }
        _canvasTexture.Apply();
    }

    // 生成笔刷
    BrushPoint GetBrushpixels(Point point,Texture2D canvas,int brush)
    {
        var bt = new BrushPoint();
        int num = brush / 2;
        for (int x = 0; x < num; x++)
        {
            for (int y = 0; y < num; y++)
            {
                Point p = new Point(point.x + x, point.y - y);
                bt.Add(p);
            }
        }
        return bt;
    }

    // 判断是否在画布上
    bool IsInCanvas(RectTransform tr, Point p)
    {
        int minX = (int)(tr.position.x - tr.rect.width / 2f);
        int maxX = (int)(tr.position.x + tr.rect.width / 2f);
        int minY = (int)(tr.position.y - tr.rect.height / 2f);
        int maxY = (int)(tr.position.y + tr.rect.height / 2f);

        return p.x >= minX && p.x <= maxX && p.y >= minY && p.y <= maxY;
    }
}
