using RayFire;
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

    [SerializeField]
    private Moneyhandler moneyHandler;

    [SerializeField]
    private LayerMask _hingeLayer;

    public bool IsDragging = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        IsDragging = true;
        
        if (moneyHandler.money > 0)
        {


            if (_type == BomType.Small)
            {
                moneyHandler.dynamiteCost = moneyHandler.smallCost;
                if ((moneyHandler.money - moneyHandler.dynamiteCost) >= 0)
                {
                    _currentDragItem = Instantiate(_smallBomb);
                    moneyHandler.dynamitePlaced = true;
                }
            }
            else if (_type == BomType.Medium)
            {

                moneyHandler.dynamiteCost = moneyHandler.midCost;
                if ((moneyHandler.money - moneyHandler.dynamiteCost) >= 0)
                {
                    _currentDragItem = Instantiate(_mediumBomb);
                    moneyHandler.dynamitePlaced = true;
                }
            }
            else if (_type == BomType.Big)
            {
                moneyHandler.dynamiteCost = moneyHandler.bigCost;
                if ((moneyHandler.money - moneyHandler.dynamiteCost) >= 0)
                {
                    _currentDragItem = Instantiate(_bigBomb);
                    moneyHandler.dynamitePlaced = true;
                }

            }

            ExplosionManager.instance.AddBombToList(_currentDragItem.GetComponent<RayfireBomb>());
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_currentDragItem != null)
        {
            Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            _currentDragItem.transform.position = Camera.main.ScreenToWorldPoint(pos);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _hingeLayer))
        {
            _currentDragItem.transform.position = hit.point;
            
            _currentDragItem = null;
        }
        else
        {
            if (_type == BomType.Small)
            {
                moneyHandler.dynamiteCost = moneyHandler.smallCost;
                moneyHandler.dynamiteRemoved = true;
            }
            else if (_type == BomType.Medium)
            {
                moneyHandler.dynamiteCost = moneyHandler.midCost;
                moneyHandler.dynamiteRemoved = true;
            }
            else if (_type == BomType.Big)
            {
                moneyHandler.dynamiteCost = moneyHandler.bigCost;
                moneyHandler.dynamiteRemoved = true;
            }
            Destroy(_currentDragItem);
            _currentDragItem = null;
        }
        IsDragging = false;
        

    }
}


