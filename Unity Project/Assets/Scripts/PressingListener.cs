using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 监听鼠标一直按压的事件
/// </summary>
public class PressingListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public delegate void AEventHandler();
    public event AEventHandler PressingEvent;
    public event AEventHandler PointerDownEvent;

    private bool _IsPressing = false;

    void Update()
    {
        if(_IsPressing && PressingEvent != null)
        {
            PressingEvent();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _IsPressing = true;
        if(PointerDownEvent != null)
        {
            PointerDownEvent();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _IsPressing = false;
    }
}