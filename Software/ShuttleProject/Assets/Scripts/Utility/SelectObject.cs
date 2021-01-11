using System;
using MMIUnity.TargetEngine.Scene;
using Scripts;
using UnityEngine;


public class SelectObject : MonoBehaviour
{
    /// <summary>
    ///     The selected GameObject
    /// </summary>
    private GameObject _go;
    
    /// <summary>
    ///     The wrist of a selected hand. This is needed in order to make the selection of a hand visible.
    ///     The hand itself is so small that the change in colour is not visible. Therefore, its first child is selected. This is the wrist of that hand.
    /// </summary>
    private Transform _child;
    
    /// <summary>
    ///     _hitPoint       = The hitpoint where the object was clicked
    ///     _hitPointNormal = The normalized form of the _hitpoint
    /// </summary>
    private Vector3 _hitPoint, _hitPointNormal;

    /// <summary>
    ///     The ray to determine the clicked object
    /// </summary>
    private Ray _ray;
    
    //selector variables
    /// <summary>
    ///     The RaycastHit on the object
    /// </summary>
    private RaycastHit _hit;
    
    /// <summary>
    ///     _originalColor = The orirginal color the object had before it was clicked
    ///     _selectColor   = The color the object will be in when it is selected
    /// </summary>
    private Color _originalColor, _selectColor = Color.red;

    /// <summary>
    ///     The dafault material of the clicked object
    /// </summary>
    private Material _defaultMaterial;
    
    /// <summary>
    ///     The transparent material that MoveTargets get if they are clicked
    /// </summary>
    private Material _materialTransparent;
    
    /// <summary>
    ///     _mRenderer      = The MeshRenderer of the clicked object
    ///     _mRendererChild = The MeshRenderer of the _child object
    /// </summary>
    private MeshRenderer _mRenderer, _mRendererChild;
    
    /// <summary>
    ///     Can be set in the scene to lock objects in their y axis and make them only movable in the other axes
    /// </summary>
    public bool lockY;

    /// <summary>
    ///     The button manager
    /// </summary>
    private AddObjectMenu _addObjectMenu;
    
    //Class objects
    /// <summary>
    ///     The movement of normal objects
    /// </summary>
    private DragAndRotate _dragAndRotate;

    /// <summary>
    ///     The movement of hands
    /// </summary>
    private HandMovement _handMovement;

    /// <summary>
    ///     Indicates if a hand is moving
    /// </summary>
    private bool _moving = false;
    
    // Start is called once per start
    private void Start()
    {
        _addObjectMenu = GameObject.Find("Scene").GetComponent<AddObjectMenu>();
        _materialTransparent = (Material) Resources.Load("Materials/SelectedMoveTarget", typeof(Material));
    }
    
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl))
        {
            if (!(Camera.main is null)) _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hit, 100.0f) && !_hit.transform.gameObject.name.Equals("Plane"))
            {
                if (_hit.transform)
                {
                    //If an object was already selected change it to it's original color
                    if (_go != null)
                    {
                        //Look after the child if the object is a hand
                        if (HandChecker.IsAnyHand(_go))
                        {
                            _mRendererChild.material.color = _originalColor;
                            _child = null;
                        }
                        //Change of the material if the object is a moveTarget
                        else if (MoveTargetChecker.IsMoveTarget(_go))
                        {
                            _mRenderer.material = _defaultMaterial;
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

                    //Look after the child if the object is a hand 
                    if (HandChecker.IsAnyHand(_go))
                    {
                        _child = _go.transform.GetChild(0);
                        _mRendererChild = _child.GetComponent<MeshRenderer>();
                        _mRendererChild.material.color = _selectColor;
                    }
                    //Change of the material if the object is a moveTarget
                    else if (MoveTargetChecker.IsMoveTarget(_go))
                    {
                        _defaultMaterial = _mRenderer.material;
                        _mRenderer.material = _materialTransparent;
                    }
                    //Standard procedure for normal GameObjects
                    else
                    {
                        _mRenderer.material.color = _selectColor;   
                    }

                    _dragAndRotate = new DragAndRotate(_go, lockY);
                    if(HandChecker.IsHand(_go)) _handMovement = new HandMovement(_go);
                }
            }
            else
            {
                //If an object was already selected change it to it's original color and set the object to null
                if (_go != null)
                {
                    //Look after the child if the object is a hand
                    if (HandChecker.IsAnyHand(_go))
                    {
                        _mRendererChild.material.color = _originalColor;
                        _child = null;
                        _mRendererChild = null;
                    }    
                    //Change of the material if the object is a moveTarget
                    else if (MoveTargetChecker.IsMoveTarget(_go))
                    {
                        _mRenderer.material = _defaultMaterial;
                    }    
                    _mRenderer.material.color = _originalColor;
                    _go = null;
                    _mRenderer = null;
                    _addObjectMenu.ObjectSelected(_go);
                }
            }
        }
        try
        {
            if (HandChecker.IsHand(_go))
            {
                //Handle rotate OR Drag for a hand object
                if (Input.GetKey(KeyCode.M) || Input.GetKey(KeyCode.X))
                {
                    _moving = true;
                }
                else
                {
                    _moving = false;
                }

                if (!Input.GetKey(KeyCode.X)) _handMovement.CastRayFromObject();
                if (!Input.GetKey(KeyCode.M)) _handMovement.HandleRotateHand();
            }
            else
            {
                //handle Rotate OR Drag
                if (!Input.GetKey(KeyCode.M)) _dragAndRotate.HandleRotate();
                if (!Input.GetKey(KeyCode.X) && !Input.GetKey(KeyCode.Y) && !Input.GetKey(KeyCode.Z)) _dragAndRotate.HandleDrag();
            }
        }
        catch (Exception)
        {
            //Catch if no object is selected
        }
    }
    
    //-------------------often used/helpful methods-------------------

    /// <summary>
    ///     Change the color back to its original one
    /// </summary>
    public void ResetColor()
    {
        //Look after the child if the object is a hand
        if (_child != null)
        {
            _mRendererChild.material.color = _originalColor;
            _child = null;
            _mRendererChild = null;
        }
        //Change of the material if the object is a moveTarget
        else if (MoveTargetChecker.IsMoveTarget(_go))
        {
            _mRenderer.material = _defaultMaterial;
        }  
        //Reset the color if the gameobject is not null
        if (_go != null)
        {
            _mRenderer.material.color = _originalColor;
            _go = null;
            _mRenderer = null;
            _addObjectMenu.ObjectSelected(_go);
        }
    }

    /// <summary>
    ///     Returns the _moving variable
    /// </summary>
    /// <returns> _moving </returns>
    public bool IsMoving() => _moving;
    
    /// <summary>
    ///     Returns the _dragAndRotate variable
    /// </summary>
    /// <returns> _dragAndRotate </returns>
    public DragAndRotate GetDragAndRotate() => _dragAndRotate;
    
    /// <summary>
    ///     Returns the selected GameObject _go
    /// </summary>
    /// <returns> _go </returns>
    public GameObject GetObject() => _go;

    /// <summary>
    ///     Returns the _hitpoint
    /// </summary>
    /// <returns> _hitpoint </returns>
    public Vector3 GetHitPoint() => _hitPoint;
    
    /// <summary>
    ///     Returns the normalized hitpoint
    /// </summary>
    /// <returns> _hitPointNormal </returns>
    public Vector3 GetHitPointNormal() => _hitPointNormal;
}
