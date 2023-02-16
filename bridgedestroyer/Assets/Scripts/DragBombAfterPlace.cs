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

    private List<DragBomb> _dragBombs = new List<DragBomb>();

    private bool CanDrag = false;
    [SerializeField]
    private LayerMask _layer;

    private Material _original;
    [SerializeField]
    private Material _select;
    public List<GameObject> _hinges;
    public List<MeshRenderer> _renders;

    private void Start()
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("BombUI"))
        {
            _dragBombs.Add(g.GetComponent<DragBomb>());
        }

        _hinges.AddRange(GameObject.FindGameObjectsWithTag("HingeModel"));
        foreach (GameObject g in _hinges)
        {
            _renders.Add(g.GetComponent<MeshRenderer>());

        }
        _original = _renders[0].material;
    }
    private void Update()
    {


        if (CanDrag)
        {
            if (Input.GetMouseButton(0))
            {

                _previousMouse = true;
                if (_currentDragItem == null)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, _bomlayer))
                    {
                        if (hit.transform.parent != null)
                        {
                            _currentDragItem = hit.transform.parent.gameObject;
                            _originalPos = _currentDragItem.transform.position;
                        }

                    }
                }

                else
                {
                    foreach (MeshRenderer m in _renders)
                    {
                        m.material = _select;
                    }
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
                foreach (MeshRenderer m in _renders)
                {
                    m.material = _original;
                }
                _currentDragItem = null;
            }
        }

        foreach (DragBomb d in _dragBombs)
        {
            if (d.IsDragging)
            {
                CanDrag = false;
                break;
            }
            else
            {
                CanDrag = true;
            }
        }


    }
}
