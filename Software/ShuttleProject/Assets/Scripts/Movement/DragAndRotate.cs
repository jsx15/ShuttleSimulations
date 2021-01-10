using System;
using UnityEngine;

public class DragAndRotate
{
    /// <summary>
    ///     object to move
    /// </summary>
    private GameObject _go;

    //drag variables
    
    /// <summary>
    ///     Offset of mouse position
    /// </summary>
    private Vector3 _mOffset;
    
    /// <summary>
    ///     mouse to screen coordinate
    /// </summary>
    private float _mZCoord;
    
    /// <summary>
    ///     sate if Y-axis movement is locked 
    /// </summary>
    private readonly bool _lockY;
    
    /// <summary>
    ///     y position of object
    /// </summary>
    private float _yPos;
    
    /// <summary>
    ///     saved old position
    /// </summary>
    private Vector3 _goPosOld;
    
    /// <summary>
    ///     position offset
    /// </summary>
    private Vector3 _dragOffset;

    
    //rotate variables
    
    /// <summary>
    ///     gravity state
    /// </summary>
    private bool _gravity;
    
    /// <summary>
    ///     collider state
    /// </summary>
    private bool _collider;
    
    /// <summary>
    ///     rotate speed combined with mouse
    /// </summary>
    private float _speed = 5.0F;
    

    //classes
    
    /// <summary>
    ///     object of class ShowAxis
    /// </summary>
    private ShowAxis showAxis;
    
    /// <summary>
    ///     object of class ObjectBounds
    /// </summary>
    private ObjectBounds _objectBounds;

    
    /// <summary>
    ///     get Object, save gravity and collider state, set offset, show axis
    /// </summary>
    /// <param name="go">gameobject to move</param>
    /// <param name="lockY">parameter if y-axis is locked</param>
    public DragAndRotate(GameObject go, bool lockY)
    {
        _go = go;
        _goPosOld = go.transform.position;
        _lockY = lockY;

        //safe settings
        SafeGravityAndCollider();

        //set Offset for Drag
        SetOffset();

        showAxis = new ShowAxis(go);
    }

    //-------------------rotate-------------------

    /// <summary>
    ///     handle rotate when button is pressed
    /// </summary>
    public void HandleRotate()
    {
        //if button "X" is pressed
        if (Input.GetKey(KeyCode.X))
        {
            //show x rotate axis
            showAxis.ShowX();
            
            //disable gravity and collider
            DisableGravityAndCollider();

            //get mouse position on x axis
            float x = _speed * Input.GetAxis("Mouse X");

            //rotate object
            _go.transform.Rotate(x, 0, 0, Space.World);
        }
        //if button "Y" is pressed
        else if (Input.GetKey(KeyCode.Y))
        {
            //show y rotate axis
            showAxis.ShowY();
            
            //disable gravity and collider
            DisableGravityAndCollider();

            //get mouse position on x axis
            float y = -1 * _speed * Input.GetAxis("Mouse X");

            //rotate object
            _go.transform.Rotate(0, y, 0, Space.World);
        }
        //if button "Z" is pressed
        else if (Input.GetKey(KeyCode.Z))
        {
            //show z rotate axis
            showAxis.ShowZ();
            
            //disable gravity and collider
            DisableGravityAndCollider();

            //get mouse position on x axis
            float z = _speed * Input.GetAxis("Mouse X");

            //rotate object
            _go.transform.Rotate(0, 0, z, Space.World);
        }
        //if no rotate button is pressed
        else
        {
            //restore gravity and collider settings
            RestoreGravityAndCollider();
            
            //hide x axis
            showAxis.HideX();
            
            //hide y axis
            showAxis.HideY();
            
            //hide z axis
            showAxis.HideZ();
        }
    }

    //-------------------drag-------------------

    /// <summary>
    ///     set world to screen offset for mouse move position
    /// </summary>
    private void SetOffset()
    {
        // store camera wold to mouse z axis offset
        if (!(Camera.main is null)) _mZCoord = Camera.main.WorldToScreenPoint(_go.transform.position).z;
        
        // store position
        var pos = _go.transform.position;
        
        //get y position
        _yPos = pos.y;

        // Store offset = gameobject world pos - mouse world pos
        _mOffset = pos - GetMouseAsWorldPoint();
    }
    
    /// <summary>
    ///     get mouse position in world 
    /// </summary>
    /// <returns>converted mouse to world coordinates</returns>
    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen
        mousePoint.z = _mZCoord;

        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    /// <summary>
    ///     handle move when button is pressed
    /// </summary>
    public void HandleDrag()
    {
        //if move button M is pressed
        if (Input.GetKey(KeyCode.M))
        {
            //disable gravity and colliders 
            DisableGravityAndCollider();

            //if y locked, stay at this height
            if (_lockY)
            {
                //convert to position with fixed height
                _go.transform.position = new Vector3(GetMouseAsWorldPoint().x + _mOffset.x, _yPos, GetMouseAsWorldPoint().z + _mOffset.z);
                
                //get new offset
                _dragOffset = _go.transform.position - _goPosOld;
            }
            //if y not locked, change height with mouse
            else
            {
                //set new postion with offset
                _go.transform.position = GetMouseAsWorldPoint() + _mOffset;
                
                //store position
                var pos = _go.transform.position;
                
                //if height is under 0 pull it back to 0
                if (pos.y < 0)
                {
                    //hold it over the ground
                    _go.transform.position = new Vector3(_go.transform.position.x, 0, pos.z);
                }

                //store new offset
                _dragOffset = _go.transform.position - _goPosOld;
            }
        }
        //if move button M is not pressed
        else
        {
            //restore all gravity and collider settings
            RestoreGravityAndCollider();
        }
    }

    /// <summary>
    ///     to get the new Offset
    /// </summary>
    /// <returns>new offset of the object</returns>
    public Vector3 GetOffsetAfterDrag() => _dragOffset;

    //-------------------safe, restore and disable Settings-------------------

    /// <summary>
    ///     save gravity and collider settings
    /// </summary>
    private void SafeGravityAndCollider()
    {
        try
        {
            //save the collider setting
            _collider = _go.GetComponent<Collider>().enabled;
            
            //save the gravity setting
            _gravity = _go.GetComponent<Rigidbody>().useGravity;
        }
        //catch exceptions
        catch
        {
            // ignored
        }
    }

    /// <summary>
    ///     restore gravity and collider settings
    /// </summary>
    private void RestoreGravityAndCollider()
    {
        try
        {
            //restore gravity setting
            _go.GetComponent<Rigidbody>().useGravity = _gravity;
            
            //restore collider setting
            _go.GetComponent<Collider>().enabled = _collider;
        }
        //catch exceptions
        catch
        {
            // ignored
        }
    }

    /// <summary>
    ///     disable gravity and collider settings
    /// </summary>
    public void DisableGravityAndCollider()
    {
        try
        {
            //disable gravity
            _go.GetComponent<Rigidbody>().useGravity = false;
            
            //disable colliders
            _go.GetComponent<Collider>().enabled = false;
        }
        //catch exceptions
        catch
        {
            // ignored
        }
    }
}