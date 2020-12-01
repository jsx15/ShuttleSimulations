using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class HoldPos : MonoBehaviour
{
    private Vector3 _pos;

    private GameObject _go;

    // Start is called before the first frame update
    void Start()
    {
        _pos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _go = GameObject.Find("Main Camera").GetComponent<SelectObject>().GetObject();
        if (MoveTargetChecker.IsMoveTarget(_go))
        {
            _pos = this.transform.position;
        }
        else
        {
            this.transform.position = _pos;
        }
    }
}