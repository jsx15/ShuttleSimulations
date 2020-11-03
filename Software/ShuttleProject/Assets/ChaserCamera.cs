using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ChaserCamera : MonoBehaviour
{
    
    public float smoothness;
    public Transform targetObject;
    private Vector3 initalOffset;
    private Vector3 cameraPosition;
    private bool followMode ;
    
   
    // Start is called before the first frame update
    void Start()
    {
        targetObject = GameObject.Find("Avatar").transform;
        initalOffset = new Vector3(0,2,-2) - targetObject.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            followMode = !followMode;
        }
        
        if (followMode) {
            
        }
        else
        {
            cameraPosition = targetObject.position + initalOffset;
            transform.position = Vector3.Lerp(transform.position, cameraPosition, smoothness*Time.fixedDeltaTime);
        }
    }
}
