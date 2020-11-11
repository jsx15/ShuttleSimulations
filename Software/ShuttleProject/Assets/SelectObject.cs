using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectObject : MonoBehaviour
{
    //object
    private GameObject go;
    
    //selector variables
    private RaycastHit hit;
    private Ray ray;
    private Color originalColor;
    private Color selectColor = Color.red;
    private MeshRenderer mRenderer;

    //drag variables
    private Vector3 mOffset;
    private float mZCoord;
    public bool lockY;
    private float yPos;
    
    //rotate variables
    private bool gravity;
    private bool collider;
    private float speed = 5.0F;
    private float x;
    private float y;
    private float z;
    private Vector3 pos;
    
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
                        mRenderer.material.color = originalColor;
                    }    
                    
                    //Mark the selected object as red
                    go = hit.transform.gameObject;
                    mRenderer = go.GetComponent<MeshRenderer>();
                    originalColor = mRenderer.material.color;
                    mRenderer.material.color = selectColor;
                    
                    //safe settings
                    safeGravityAndCollider();

                    //set Offset for Drag
                    setOffset();
                }
            }
            else
            {
                //If an object was already selected change it to it's original color and set the object to null
                if (go != null)
                {
                    mRenderer.material.color = originalColor;
                    go = null;
                }  
            }
        }
        try
        {
            //handle Rotate OR Drag
            if (!Input.GetKey(KeyCode.M)) handleRotate();
            if (!Input.GetKey(KeyCode.X) && !Input.GetKey(KeyCode.Y) && !Input.GetKey(KeyCode.Z)) handleDrag();
        }
        catch (Exception)
        {
        }
    }
    
    //-------------------rotate-------------------

    void handleRotate()
    {
        if (Input.GetKey(KeyCode.X))
        {
            //disable gravity and collider
            disableGravityAndCollider();

            //get mouse position on x axis
            x = speed * Input.GetAxis("Mouse X");

            //rotate object
            go.transform.Rotate(x, 0, 0);
        }

        else if (Input.GetKey(KeyCode.Y))
        {
            //disable gravity and collider
            disableGravityAndCollider();

            //get mouse position on x axis
            y = -1 * speed * Input.GetAxis("Mouse X");

            //rotate object
            go.transform.Rotate(0, y, 0);
        }

        else if (Input.GetKey(KeyCode.Z))
        {
            //disable gravity and collider
            disableGravityAndCollider();

            //get mouse position on x axis
            z = speed * Input.GetAxis("Mouse X");

            //rotate object
            go.transform.Rotate(0, 0, z);
        }
        else
        {
            restoreGravityAndCollider();
        }
    }

    //-------------------drag-------------------

    void setOffset()
    {
        mZCoord = Camera.main.WorldToScreenPoint(go.transform.position).z;
        yPos = go.transform.position.y;

        // Store offset = gameobject world pos - mouse world pos
        mOffset = go.transform.position - GetMouseAsWorldPoint();
    }


    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen
        mousePoint.z = mZCoord;

        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }


    void handleDrag()
    {
        if (Input.GetKey(KeyCode.M))
        {
            disableGravityAndCollider();

            //stay at this height
            if (lockY)
            {
                go.transform.position = new Vector3(GetMouseAsWorldPoint().x + mOffset.x, yPos,
                    GetMouseAsWorldPoint().z + mOffset.z);
            }
            //change height with mouse
            else
            {
                go.transform.position = GetMouseAsWorldPoint() + mOffset;
                if (go.transform.position.y < 0)
                {
                    go.transform.position = new Vector3(go.transform.position.x, 0, go.transform.position.z);
                }
            }
        }
        else
        {
            restoreGravityAndCollider();
        }
    }

    //-------------------safe, restore and disable settings-------------------

    //safe gravity and collider settings
    void safeGravityAndCollider()
    {
        try
        {
            gravity = go.GetComponent<Rigidbody>().useGravity;
            collider = go.GetComponent<Collider>().enabled;
        }
        catch
        {
        }
    }

    //restore gravity and collider settings
    void restoreGravityAndCollider()
    {
        try
        {
            go.GetComponent<Rigidbody>().useGravity = gravity;
            go.GetComponent<Collider>().enabled = collider;
        }
        catch
        {
        }
    }

    //disable gravity and collider settings
    void disableGravityAndCollider()
    {
        try
        {
            go.GetComponent<Rigidbody>().useGravity = false;
            go.GetComponent<Collider>().enabled = false;
        }
        catch
        {
        }
    }

    //Change color back to the original one
    public void resetColor() => mRenderer.material.color = originalColor;
    
    //Return the selected object
    public GameObject getObject() => go;
}
