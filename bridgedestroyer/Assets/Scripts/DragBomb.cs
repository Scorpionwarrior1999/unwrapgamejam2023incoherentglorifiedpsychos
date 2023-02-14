using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum BomType
{
    Small, Big, Medium

}
public class DragBomb : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private BomType _type;
    [SerializeField]
    private GameObject _bigBomb;
    [SerializeField]
    private GameObject _mediumBomb;
    [SerializeField]
    private GameObject _smallBomb;

    private GameObject _currentDragItem;

    public void OnBeginDrag(PointerEventData eventData)
    {

        if(_type == BomType.Small)
        {
            _currentDragItem =  Instantiate(_smallBomb);
        }
        if (_type == BomType.Medium)
        {
            _currentDragItem = Instantiate(_mediumBomb);
        }
        if (_type == BomType.Big)
        {
            _currentDragItem = Instantiate(_bigBomb);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        _currentDragItem.transform.position = Camera.main.ScreenToWorldPoint(pos) ;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }

}
