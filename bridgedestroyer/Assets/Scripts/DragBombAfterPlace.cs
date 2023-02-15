using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragBombAfterPlace : MonoBehaviour
{
    private GameObject _currentDragItem;
    private bool _previousMouse = false;
    [SerializeField]
    private LayerMask _hingeLayer;
    [SerializeField]
    private LayerMask _bomlayer;
    private Vector3 _originalPos;

    public bool HasbeenDropped = true;

    private void Update()
    {
        if (HasbeenDropped)
        {
            if (Input.GetMouseButton(0))
            {

                _previousMouse = true;
                if (_currentDragItem == null)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        
                            _currentDragItem = hit.transform.parent.gameObject;
                            _originalPos = _currentDragItem.transform.position;
                        
                    }
                }

                else
                {
                    if (_currentDragItem != null)
                    {
                        Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
                        _currentDragItem.transform.position = Camera.main.ScreenToWorldPoint(pos);
                    }
                }
            }
            else
            {
                if (_previousMouse == true && !Input.GetMouseButton(0) && _currentDragItem != null)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, _hingeLayer))
                    {
                        _currentDragItem.transform.position = hit.point;
                    }
                    else
                    {
                        _currentDragItem.transform.position = _originalPos;
                        _currentDragItem = null;
                    }
                }
                _currentDragItem = null;
            }
        }

    }
}
