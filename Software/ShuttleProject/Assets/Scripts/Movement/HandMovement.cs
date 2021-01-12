using System.Collections.Generic;
using System.Linq;
using MMIUnity.TargetEngine.Scene;
using Movement;
using UnityEngine;

public class HandMovement
{
    /// <summary>
    ///     _go     = The wrist of the hand that should be moved
    ///     _parent = The hand itself
    /// </summary>
    private readonly GameObject _go, _parent;

    /// <summary>
    ///     _gravity  = States if the object has gravity
    ///     _collider = States if the object has a collider
    /// </summary>
    private bool _gravity, _collider;
    
    /// <summary>
    ///     _x     = The mouse position on the x-Axis
    ///     _speed = The default speed of the movement
    /// </summary>
    private float _x, _speed = 5.0F;
    
    /// <summary>
    ///     Ensures that the rotation stays the same even when the object is moved
    /// </summary>
    private Vector3 _goDirectionOld;
    
    /// <summary>
    ///     Constructor for initializing parameters and adding the IKManager script to each finger of the hand object
    /// </summary>
    /// <param name="go">GameObject which logically should be a hand</param>
    public HandMovement(GameObject go)
    {
        _go = go;
        _goDirectionOld = _go.transform.up;
        _parent = go.transform.parent.gameObject;
        var bones = new List<UnityBone>(go.GetComponentsInChildren<UnityBone>());
        SafeCollider();

        foreach (UnityBone bone in bones.Skip(1))
        {
            if (bone.name.Contains("Proximal") || bone.name.Contains("ThumbMid"))
            {
                if (bone.gameObject.GetComponent<IKManager>() == null)
                {
                    bone.gameObject.AddComponent<IKManager>();
                }
            }
        }
    }

    /// <summary>
    ///     Handle hand rotation
    /// </summary>
    public void HandleRotateHand()
    {
        if (Input.GetKey(KeyCode.X))
        {
            
            //disable gravity and collider
            DisableCollider();

            //get mouse position on x axis
            _x = _speed * Input.GetAxis("Mouse X");

            //rotate object
            _go.transform.Rotate(_x, 0, 0, Space.Self);
            //needed to ensure rotation stays the same when object is moved 
            _goDirectionOld = _go.transform.up;

            RestoreCollider();
        }
    }

    
    /// <summary>
    ///     casts a Ray from Object in predetermined direction.
    ///     colliders from originating object will be disabled in order to let the rays not hit the objects own collider
    /// </summary>
    public void CastRayFromObject()
    {
        if (Input.GetKey(KeyCode.M))
        {
            // disable object collider to prevent the ray hitting the objects (hands) collider  
            DisableCollider();
            // cast ray from object in direction
            Ray rayHand = new Ray(_go.transform.position, _go.transform.right);
            RaycastHit hitInfoHand;

            // cast ray from camera position to mouse position
            if (!(Camera.main is null))
            {
                Ray rayMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfoMouse;

                if (Physics.Raycast(rayHand, out hitInfoHand, 0.2f))
                {
                    if (Physics.Raycast(rayMouse, out hitInfoMouse))
                    {
                        // check if hitInfoMouse points to parent object of hand
                        if (hitInfoMouse.collider.gameObject.GetInstanceID().Equals(_parent.GetInstanceID()))
                        {
                            // set object position to mouse position with a certain distance to the parent object
                            _go.transform.position = hitInfoMouse.point + hitInfoMouse.normal * 0.02f;
                            // ensure that the object is facing towards the parent object
                            _go.transform.rotation = Quaternion.FromToRotation(Vector3.left, hitInfoMouse.normal);
                            // restore previous rotation. needed when the object was manually rotated beforehand.
                            _go.transform.Rotate(Vector3.Angle(_go.transform.up,_goDirectionOld), 0, 0);
                        }
                    }
                    // needed to ensure that the object cant leave the dimensions of the parent if the mouse is not on the parent 
                    else
                    {
                        _go.transform.position = hitInfoHand.point  + hitInfoHand.normal * 0.02f;
                        _go.transform.rotation = Quaternion.FromToRotation(Vector3.left, hitInfoHand.normal);
                        _go.transform.Rotate(Vector3.Angle(_go.transform.up,_goDirectionOld), 0, 0);
                    }
                }
            }

            // restore collider
            RestoreCollider();
        }
    }
    
//-------------------safe, restore and disable Settings-------------------

    /// <summary>
    ///     Safe the Collider
    /// </summary>
    private void SafeCollider()
    {
        try
        {
            _collider = _go.GetComponent<Collider>().enabled;
            //_gravity = _go.GetComponent<Rigidbody>().useGravity;
        }
        catch
        {
            // ignored
        }
    }

    /// <summary>
    ///     Restore the Collider
    /// </summary>
    private void RestoreCollider()
    {
        try
        {
            //_go.GetComponent<Rigidbody>().useGravity = _gravity;
            _go.GetComponent<Collider>().enabled = _collider;
        }
        catch
        {
            // ignored
        }
    }

    /// <summary>
    ///     Disable the Collider
    /// </summary>
    public void DisableCollider()
    {
        try
        {
            //_go.GetComponent<Rigidbody>().useGravity = false;
            _go.GetComponent<Collider>().enabled = false;
        }
        catch
        {
        }
    }
}