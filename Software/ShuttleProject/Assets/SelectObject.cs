using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObject : MonoBehaviour
{
    private GameObject go;
    private RaycastHit hit;
    private Ray ray;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform)
                {
                    go = hit.transform.gameObject;
                }
            }
        }
    }

    //return the clicked object
    public GameObject getObject() => go;
}
