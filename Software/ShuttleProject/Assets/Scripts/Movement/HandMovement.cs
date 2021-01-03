using System.Collections.Generic;
using System.Linq;
using MMIUnity.TargetEngine.Scene;
using Movement;
using UnityEngine;

public class HandMovement
{
    private readonly GameObject _go;
    private readonly GameObject _parent;
    private List<UnityBone> _bones;

    //rotate variables
    private bool _gravity;
    private bool _collider;
    private float _x;
    private float _speed = 5.0F;
    private Vector3 _goDirectionOld;

    //classes
    private ShowAxis showAxis;

    public HandMovement(GameObject go)
    {
        _go = go;
        _goDirectionOld = _go.transform.up;
        _parent = go.transform.parent.gameObject;
        _bones = new List<UnityBone>(go.GetComponentsInChildren<UnityBone>());
        SafeCollider();

        foreach (UnityBone bone in _bones.Skip(1))
        {
            if (bone.name.Contains("Proximal") || bone.name.Contains("ThumbMid"))
            {
                bone.gameObject.AddComponent<IKManager>();
            }
        }
    }

    //Handle the rotation of the hand
    public void HandleRotateHand()
    {
        if (Input.GetKey(KeyCode.X))
        {
            //showAxis.showY();
            //disable gravity and collider
            DisableCollider();

            //get mouse position on x axis
            _x = _speed * Input.GetAxis("Mouse X");

            //rotate object
            _go.transform.Rotate(_x, 0, 0, Space.Self);
            _goDirectionOld = _go.transform.up;

            RestoreCollider();
        }
    }

    //casts a Ray from Object in predetermined direction
    //colliders from originating object have to be disabled in order to let the rays not hit the objects own collider 
    public void CastRayFromObject()
    {
        if (Input.GetKey(KeyCode.M))
        {
            DisableCollider();
            //cast ray from object in direction
            Ray rayHand = new Ray(_go.transform.position, _go.transform.right);
            RaycastHit hitInfoHand;

            //cast ray from camera position to mouse position
            Ray rayMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfoMouse;

            if (Physics.Raycast(rayHand, out hitInfoHand, 0.2f))
            {
                if (Physics.Raycast(rayMouse, out hitInfoMouse))
                {
                    // check if hitInfoMouse points to parent object of hand
                    if (hitInfoMouse.collider.gameObject.GetInstanceID().Equals(_parent.GetInstanceID()))
                    {
                        _go.transform.position = hitInfoMouse.point + hitInfoMouse.normal * 0.02f;
                        _go.transform.rotation = Quaternion.FromToRotation(Vector3.left, hitInfoMouse.normal);
                        _go.transform.Rotate(Vector3.Angle(_goDirectionOld,_go.transform.up), 0, 0);
                    }
                }
                else
                {
                    _go.transform.position = hitInfoHand.point  + hitInfoHand.normal * 0.02f;
                    _go.transform.rotation = Quaternion.FromToRotation(Vector3.left, hitInfoHand.normal);
                    _go.transform.Rotate(Vector3.Angle(_goDirectionOld,_go.transform.up), 0, 0);
                }
            }
            RestoreCollider();
        }
    }
    
//-------------------safe, restore and disable Settings-------------------

    //safe gravity and collider settings
    private void SafeCollider()
    {
        try
        {
            _collider = _go.GetComponent<Collider>().enabled;
            //_gravity = _go.GetComponent<Rigidbody>().useGravity;
        }
        catch
        {
        }
    }

    //restore gravity and collider settings
    private void RestoreCollider()
    {
        try
        {
            //_go.GetComponent<Rigidbody>().useGravity = _gravity;
            _go.GetComponent<Collider>().enabled = _collider;
        }
        catch
        {
        }
    }

    //disable gravity and collider settings
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