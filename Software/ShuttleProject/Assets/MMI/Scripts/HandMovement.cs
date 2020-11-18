using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DefaultNamespace;
using MMIStandard;
using UnityEngine;

public class HandMovement 
{
    private GameObject go;
    private bool collider;

    public HandMovement(GameObject go)
    {
        this.go = go;
        safeCollider();
    }
    
    //casts a Ray from Object in predetermined direction
    //colliders from originating object have to be disabled in order to let the rays not hit the objects own collider 
    public void castRayFromObject()
    {
        disableCollider();
        
        //cast ray from object in direction  
        Ray rayHand = new Ray(go.transform.position,go.transform.right);
        RaycastHit hitInfoHand;
        
        //cast ray from camera position to mouse position
        Ray rayMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfoMouse;
        
        if (Physics.Raycast(rayHand, out hitInfoHand, 0.2f))
        {
            if (Input.GetKey(KeyCode.M) && Physics.Raycast(rayMouse, out hitInfoMouse))
            {
                go.transform.position = hitInfoMouse.point + hitInfoMouse.normal * 0.01f;
                go.transform.rotation = Quaternion.FromToRotation(Vector3.left, hitInfoMouse.normal);
            }
            else
            {
                go.transform.position = hitInfoHand.point + hitInfoHand.normal * 0.01f;
                go.transform.rotation = Quaternion.FromToRotation(Vector3.left, hitInfoHand.normal);
            }
        }
        restoreCollider();
    }
    
    
    
    private void safeCollider()
    {
        try
        {
            collider = go.GetComponent<Collider>().enabled;
        }
        catch
        {
        }
    }

    //restore collider settings
    private void restoreCollider()
    {
        try
        {
            go.GetComponent<Collider>().enabled = collider;
        }
        catch
        {
        }
    }

    //disable collider settings
    private void disableCollider()
    {
        try
        {
            go.GetComponent<Collider>().enabled = false;
        }
        catch
        {
        }
    }
}

