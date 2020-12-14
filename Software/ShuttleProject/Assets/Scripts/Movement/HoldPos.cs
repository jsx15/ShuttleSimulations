using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class HoldPos : MonoBehaviour
{
    private Vector3 _pos;
    private Quaternion _rotation;

    private GameObject _go;
    private GameObject camera;

    // Start is called before the first frame update
    void Start()
    {
        _pos = this.transform.position;
        _rotation = this.transform.rotation;
        camera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        _go = camera.GetComponent<SelectObject>().GetObject();
        if (MoveTargetChecker.IsMoveTarget(_go))
        {
            _pos = this.transform.position;
            _rotation = this.transform.rotation;
        }
        else
        {
            this.transform.position = _pos;
            this.transform.rotation = _rotation;
        }
    }
}