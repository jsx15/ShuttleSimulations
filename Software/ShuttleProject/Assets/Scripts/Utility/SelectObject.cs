using System;
using System.Collections;
using System.Collections.Generic;
using MMIUnity;
using Scripts;
using UI.MenuMB;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SelectObject : MonoBehaviour
{
    public bool lockY;
    
    public event EventHandler ObjectSelected;
    
    //Button manager
    private AddObjectMenu _addObjectMenu;
    
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
    
    private WalkToMenu.WalkToHandler _walkToHandler = new WalkToMenu.WalkToHandler();
    
    
    private void Start()
    {
        _addObjectMenu = GameObject.Find("Scene").GetComponent<AddObjectMenu>();
    }
    
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
                        if (HandChecker.IsHand(go))
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
                    
                    _addObjectMenu.ObjectSelected(go);
                    
                    ObjectSelected += delegate(object sender, EventArgs args)
                    {
                        _walkToHandler.ObjectSelectedHandler(sender, args, go.name);
                    };
                    
                    // ObjectSelected(this, new EventArgs());

                    if (HandChecker.IsHand(go))
                    {
                        child = go.transform.GetChild(0);
                        mRendererChild = child.GetComponent<MeshRenderer>();
                        mRendererChild.material.color = selectColor;
                    }
                    mRenderer.material.color = selectColor;

                    dragAndRotate = new DragAndRotate(go, lockY);
                    if(HandChecker.IsHand(go)) handMovement = new HandMovement(go);
                }
            }
            else
            {
                //If an object was already selected change it to it's original color and set the object to null
                if (go != null)
                {
                    if (HandChecker.IsHand(go))
                    {
                        mRendererChild.material.color = originalColor;
                        child = null;
                    }    
                    mRenderer.material.color = originalColor;
                    go = null;
                    _addObjectMenu.ObjectSelected(go);
                }  
            }
        }
        try
        {
            if (HandChecker.IsHand(go))
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

    //Change color back to the original one
    public void resetColor()
    {
        if (child != null)
        {
            mRendererChild.material.color = originalColor;
        }
        mRenderer.material.color = originalColor;
        go = null;
        _addObjectMenu.ObjectSelected(go);
    }
    
    //Return the selected object
    public GameObject getObject() => go;

    //Return the selected point
    public Vector3 getHitPoint() => hitPoint;
    
}
