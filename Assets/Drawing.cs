using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drawing : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler
{
    [SerializeField] Color _backGroundCor = Color.white;
    [SerializeField] DrawBrush _brush = new DrawBrush();
    [SerializeField] DrawEraser _eraser = new DrawEraser();
    Texture2D _canvasTexture = null;
    List<Vector2> _drawPointList = new List<Vector2>();

    public IDraw curDraw { private set; get; }

    void Start () 
    {
        curDraw = _brush;

        var image = transform.GetComponent<RawImage>();
        _canvasTexture = new Texture2D((int)image.rectTransform.rect.width, (int)image.rectTransform.rect.height);
        image.texture = _canvasTexture;
        Rest();
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        _drawPointList.Clear();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var point = new Vector2(eventData.position.x, eventData.position.y);
        _drawPointList.Add(point);
        Draw();
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        var point = new Vector2(eventData.position.x, eventData.position.y);
        _drawPointList.Add(point);
        Draw();
    }

    /// <summary>
    /// 清空画布 
    /// </summary>
    [ContextMenu("ClearCanvas")]
    public void Rest()
    {
        for (int x = 0; x < _canvasTexture.width; x++)
        {
            for (int y = 0; y < _canvasTexture.height; y++)
            {
                _canvasTexture.SetPixel(x, y, _backGroundCor);
            }
        }

        _canvasTexture.Apply();
    }

    // 绘图
    void Draw()
    {
        var pointList = GetPointList();
        foreach (var item in pointList)
        {
            curDraw.Draw(_canvasTexture,_backGroundCor,item);
        }
        _canvasTexture.Apply();
    }

    // 计算绘制路径
    List<Vector2> GetPointList()
    {
        List<Vector2> pointList = new List<Vector2>();
        if(_drawPointList.Count == 1)
        {
            pointList.Add(ToTexturePos(_drawPointList[0]));
            return pointList;
        }
        for (int i = 1; i < _drawPointList.Count; i++)
        {
            var fp = _drawPointList[i - 1];
            var sp = _drawPointList[i];
            var smooth = Mathf.CeilToInt(Vector2.Distance(fp, sp) / (curDraw.width / 2f));
            //Debug.Log("Vector2.Distance(fp, sp) = " + Vector2.Distance(fp, sp) + "curDraw.width = " + curDraw.width + "smooth = " + smooth);
            for (int j = 0; j <= smooth; j++)
            {
                var bt = Vector2.Lerp(fp,sp, 1f * j / smooth);
                pointList.Add(ToTexturePos(bt));
            }
        }
        return pointList;
    }

    // 转到texture坐标
    Vector2 ToTexturePos(Vector2 v)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.GetComponent<RectTransform>(), v, null, out pos);
        //Debug.Log("pos == "+ pos);
        return pos;
    }


    //#if UNITY_EDITOR
    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 150, 90), "橡皮擦")) { curDraw = _eraser; }
        if (GUI.Button(new Rect(0, 90, 150, 90), "画笔")) { curDraw = _brush; } 
    }
    //#endif
}
