using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DefaultNamespace;
using MMIStandard;
using MMIUnity.TargetEngine.Scene;
using UnityEngine;

public class HandMovement 
{
    private GameObject go;
    private bool collider;
    private GameObject parent;

    private List<UnityBone> bones; 
    

    public HandMovement(GameObject go)
    {
        this.go = go;
        parent = go.transform.parent.gameObject;
        safeCollider();
        bones = new List<UnityBone>(go.GetComponentsInChildren<UnityBone>());
        
    }
    
    //casts a Ray from Object in predetermined direction
    //colliders from originating object have to be disabled in order to let the rays not hit the objects own collider 
    public void castRayFromObject()
    {
        disableCollider();
        
        //cast ray from object in direction  
        Ray rayHand = new Ray(go.transform.position ,go.transform.right);
        RaycastHit hitInfoHand;
        
        //cast ray from camera position to mouse position
        Ray rayMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfoMouse;
        
        if (Physics.Raycast(rayHand, out hitInfoHand, 0.2f))
        {
            if (Input.GetKey(KeyCode.M) && Physics.Raycast(rayMouse, out hitInfoMouse))
            {
                // check if hitInfoMouse points to parent object of hand
                if (hitInfoMouse.collider.gameObject.GetInstanceID().Equals(parent.GetInstanceID()))
                {
                    go.transform.position = hitInfoMouse.point + hitInfoMouse.normal * 0.005f;
                    go.transform.rotation = Quaternion.FromToRotation(Vector3.left, hitInfoMouse.normal);
                    
                    /*foreach (UnityBone bone in bones.Skip(1))
                    {
                         Ray rayBone = new Ray(bone.transform.position ,bone.transform.right);
                        RaycastHit hitInfoBone;

                        if (Physics.Raycast(rayBone, out hitInfoBone, 0.2f))
                        {
                            bone.transform.rotation = Quaternion.FromToRotation(hitInfoBone.transform, hitInfoBone.normal);
                        }
                        else
                        {
                            bone.transform.rotation = Quaternion.Euler(0,0,-60);
                        }
                        

                    }*/
                }
            }
            else
            {
                go.transform.position = hitInfoHand.point + hitInfoHand.normal * 0.005f;
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

