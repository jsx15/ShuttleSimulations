using System;
using UnityEngine;

namespace Movement
{
    public class WalkTargetOrientation : MonoBehaviour
    {
        private Transform _parent;
        private Vector3 _direction;
        private Vector3 _pos;
        
        private void Start()
        {
            _parent = transform.parent;
            _direction = transform.position - _parent.transform.position;
        }

        private void Update()
        {
            transform.LookAt(_parent, _direction);
            _pos = transform.position;
            transform.position = new Vector3(_pos.x, 0.03f, _pos.z);
        }
    }
}