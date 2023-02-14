using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragBombAfterPlace : MonoBehaviour
{
    private GameObject _currentDragItem;

    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    _currentDragItem = this.gameObject;
    //    Debug.Log(this.gameObject);
    //}

    //public void OnDrag(PointerEventData eventData)
    //{
    //    Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
    //    _currentDragItem.transform.position = Camera.main.ScreenToWorldPoint(pos);
    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    _currentDragItem = null;
    //}
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (_currentDragItem == null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    _currentDragItem = hit.transform.parent.gameObject;
                }
            }
            else
            {
                Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
                _currentDragItem.transform.position = Camera.main.ScreenToWorldPoint(pos);
            }
        }
        else
        {
            _currentDragItem = null;
        }

    }
}
