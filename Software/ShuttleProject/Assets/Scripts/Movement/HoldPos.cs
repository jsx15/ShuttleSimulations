using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class HoldPos : MonoBehaviour
{
    /// <summary>
    ///     position of object
    /// </summary>
    private Vector3 _pos;
    
    /// <summary>
    ///     rotation of object
    /// </summary>
    private Quaternion _rotation;

    /// <summary>
    ///     selected object
    /// </summary>
    private GameObject _go;
    
    /// <summary>
    ///     camera object
    /// </summary>
    private GameObject _camera;

    // Start is called before the first frame update
    void Start()
    {
        var trans = transform;
        //save position of target move object
        _pos = trans.position;
        
        //save rotation of target move object
        _rotation = trans.rotation;
        
        //save camera to object
        _camera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        // save selected object in _go
        _go = _camera.GetComponent<SelectObject>().GetObject();
        
        // check if _go is a move target object
        if (MoveTargetChecker.IsMoveTarget(_go) && (Input.GetKey(KeyCode.M) || Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.Y) || Input.GetKey(KeyCode.Z)))
        {
            var trans = transform;
            // save position of move target in _pos
            _pos = trans.position;
            
            //save rotation of move target in _rotation
            _rotation = trans.rotation;
        }
        // if _go is not a move target object
        else
        {
            // overwrite position and rotation
            transform.SetPositionAndRotation(_pos, _rotation);
        }
    }
}