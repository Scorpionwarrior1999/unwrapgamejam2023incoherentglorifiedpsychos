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

    private Material _original;
    [SerializeField]
    private Material _select;
    public List<GameObject> _hinges;
    public List<MeshRenderer> _renders;
    private bool _canDrag = false;


    private void Start()
    {

        _hinges.AddRange(GameObject.FindGameObjectsWithTag("HingeModel"));
        foreach(GameObject g in _hinges)
        {
            _renders.Add(g.GetComponent<MeshRenderer>());

        }
        _original = _renders[0].material;

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        IsDragging = true;
        
        


        if (moneyHandler.money > 0)
        {


            if (_type == BomType.Small)
            {
               
                if ((moneyHandler.money - moneyHandler.smallCost) >= 0)
                {
                    _canDrag = true;
                    _currentDragItem = Instantiate(_smallBomb);
                    moneyHandler.dynamitePlaced = true;

                    foreach (MeshRenderer m in _renders)
                    {
                        m.material = _select;
                    }

                    moneyHandler.dynamiteCost = moneyHandler.smallCost;
                    moneyHandler.dynamitePlaced = true;
                }
            }
            else if (_type == BomType.Medium)
            {

               
                if ((moneyHandler.money - moneyHandler.midCost) >= 0)
                {
                    _canDrag = true;
                    _currentDragItem = Instantiate(_mediumBomb);
                    moneyHandler.dynamitePlaced = true;

                    foreach (MeshRenderer m in _renders)
                    {
                        m.material = _select;
                    }

                    moneyHandler.dynamiteCost = moneyHandler.midCost;
                    moneyHandler.dynamitePlaced = true;
                }
            }
            else if (_type == BomType.Big)
            {
                
                if ((moneyHandler.money - moneyHandler.bigCost) >= 0)
                {
                    _canDrag = true;
                    _currentDragItem = Instantiate(_bigBomb);
                    moneyHandler.dynamitePlaced = true;

                    foreach (MeshRenderer m in _renders)
                    {
                        m.material = _select;
                    }

                    moneyHandler.dynamiteCost = moneyHandler.bigCost;
                    moneyHandler.dynamitePlaced = true;
                }

            }
            if (_currentDragItem != null)
            {
                ExplosionManager.instance.AddBombToList(_currentDragItem.GetComponent<RayfireBomb>());
            }
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
        if (_canDrag)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _hingeLayer))
            {
                if (_currentDragItem != null)
                {
                    _currentDragItem.transform.position = hit.point;

                    _currentDragItem = null;
                }
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

            foreach (MeshRenderer m in _renders)
            {
                m.material = _original;
            }

            _canDrag = false;
        }

            IsDragging = false;
        
        

    }
}


