using System;
using System.Collections;
using System.Collections.Generic;
using MMIUnity;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SelectObject : MonoBehaviour
{
    public bool lockY;
    
    //object ,child and hitpoint
    private GameObject go;
    private Transform child;
    private Vector3 hitPoint;
    
    //selector variables
    private RaycastHit hit;
    private Ray ray;
    private Color originalColor;
    private Color selectColor = Color.red;
    private MeshRenderer mRenderer;
    private MeshRenderer mRendererChild;
    
    //Class objects
    private ObjectBounds objectBounds;
    private DragAndRotate dragAndRotate;

    private HandMovement handMovement;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform)
                {
                    //If an object was already selected change it to it's original color
                    if (go != null)
                    {
                        if (isHand(go))
                        {
                            mRendererChild.material.color = originalColor;
                            child = null;
                        }    
                        mRenderer.material.color = originalColor;
                    }
                    
                    //Get object and Hit Point
                    hitPoint = hit.point;
                    go = hit.transform.gameObject;

                    //Mark the selected object as red
                    mRenderer = go.GetComponent<MeshRenderer>();
                    originalColor = mRenderer.material.color;

                    if (isHand(go))
                    {
                        child = go.transform.GetChild(0);
                        mRendererChild = child.GetComponent<MeshRenderer>();
                        mRendererChild.material.color = selectColor;
                    }
                    mRenderer.material.color = selectColor;

                    dragAndRotate = new DragAndRotate(go, lockY);
                    if(isHand(go)) handMovement = new HandMovement(go);
                }
            }
            else
            {
                //If an object was already selected change it to it's original color and set the object to null
                if (go != null)
                {
                    if (isHand(go))
                    {
                        mRendererChild.material.color = originalColor;
                        child = null;
                    }    
                    mRenderer.material.color = originalColor;
                    go = null;
                }  
            }
        }
        try
        {
            if (isHand(go))
            {
                handMovement.castRayFromObject();
            }
            else
            {
                //handle Rotate OR Drag
                if (!Input.GetKey(KeyCode.M)) dragAndRotate.handleRotate();
                if (!Input.GetKey(KeyCode.X) && !Input.GetKey(KeyCode.Y) && !Input.GetKey(KeyCode.Z)) dragAndRotate.handleDrag();
            }

            

        }
        catch (Exception)
        {
        }
    }
    
    //-------------------often used/helpful methods-------------------
    
    //Check if object is a hand
    private Boolean isHand(GameObject obj)
    {
        if (obj.name == "RightHand(Clone)" || obj.name == "LeftHand(Clone)")
        {
            return true;
        }
        return false;
    }
    
    //Change color back to the original one
    public void resetColor()
    {
        if (child != null)
        {
            mRendererChild.material.color = originalColor;
        }
        mRenderer.material.color = originalColor;
    }
    
    //Return the selected object
    public GameObject getObject() => go;

    //Return the selected point
    public Vector3 getHitPoint() => hitPoint;
    
}
