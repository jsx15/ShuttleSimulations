﻿using System;
using Scripts;
using UnityEngine;

public class SelectObject : MonoBehaviour
{
    public bool lockY;
    
    //Button manager
    private AddObjectMenu _addObjectMenu;
    
    //object ,child and hitpoint
    private GameObject _go;
    private Transform _child;
    private Vector3 _hitPoint;
    private Vector3 _hitPointNormal;
    
    //selector variables
    private RaycastHit _hit;
    private Ray _ray;
    private Color _originalColor;
    private readonly Color _selectColor = Color.red;
    private MeshRenderer _mRenderer;
    private MeshRenderer _mRendererChild;
    
    //Class objects
    private ObjectBounds _objectBounds;
    private DragAndRotate _dragAndRotate;

    private HandMovement _handMovement;
    
    
    private void Start()
    {
        _addObjectMenu = GameObject.Find("Scene").GetComponent<AddObjectMenu>();
    }
    
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl))
        {
            if (!(Camera.main is null)) _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hit, 100.0f))
            {
                if (_hit.transform)
                {
                    //If an object was already selected change it to it's original color
                    if (!(_go is null))
                    {
                        if (HandChecker.IsHand(_go))
                        {
                            _mRendererChild.material.color = _originalColor;
                            _child = null;
                        }    
                        _mRenderer.material.color = _originalColor;
                    }
                    
                    //Get object and Hit Point
                    _hitPoint = _hit.point;
                    _hitPointNormal = _hit.normal;
                    _go = _hit.transform.gameObject;

                    //Mark the selected object as red
                    _mRenderer = _go.GetComponent<MeshRenderer>();
                    _originalColor = _mRenderer.material.color;
                    
                    _addObjectMenu.ObjectSelected(_go);

                    if (HandChecker.IsHand(_go))
                    {
                        _child = _go.transform.GetChild(0);
                        _mRendererChild = _child.GetComponent<MeshRenderer>();
                        _mRendererChild.material.color = _selectColor;
                    }
                    _mRenderer.material.color = _selectColor;

                    _dragAndRotate = new DragAndRotate(_go, lockY);
                    if(HandChecker.IsHand(_go)) _handMovement = new HandMovement(_go);
                }
            }
            else
            {
                //If an object was already selected change it to it's original color and set the object to null
                if (!(_go is null))
                {
                    if (HandChecker.IsHand(_go))
                    {
                        _mRendererChild.material.color = _originalColor;
                        _child = null;
                    }    
                    _mRenderer.material.color = _originalColor;
                    _go = null;
                    _addObjectMenu.ObjectSelected(_go);
                }
            }
        }
        try
        {
            if (HandChecker.IsHand(_go))
            {
                //Handle rotate OR Drag for a hand object
                if (!Input.GetKey(KeyCode.X)) _handMovement.CastRayFromObject();
                if (!Input.GetKey(KeyCode.M)) _handMovement.HandleRotateHand();
            }
            else
            {
                //handle Rotate OR Drag
                if (!Input.GetKey(KeyCode.M)) _dragAndRotate.handleRotate();
                if (!Input.GetKey(KeyCode.X) && !Input.GetKey(KeyCode.Y) && !Input.GetKey(KeyCode.Z)) _dragAndRotate.handleDrag();
            }
        }
        catch (Exception)
        {
            //Catch if no object is selected
        }
    }
    
    //-------------------often used/helpful methods-------------------

    //Change color back to the original one
    public void ResetColor()
    {
        if (!(_child is null))
        {
            _mRendererChild.material.color = _originalColor;
        }
        _mRenderer.material.color = _originalColor;
        _go = null;
        _addObjectMenu.ObjectSelected(_go);
    }
    
    //Return the selected object
    public GameObject GetObject() => _go;

    //Return the selected point
    public Vector3 GetHitPoint() => _hitPoint;
    
    //Return the selected points normal
    public Vector3 GetHitPointNormal() => _hitPointNormal;
}