using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragBombAfterPlace : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private GameObject _currentDragItem;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _currentDragItem = gameObject;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        _currentDragItem.transform.position = Camera.main.ScreenToWorldPoint(pos);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _currentDragItem = null;
    }
}
