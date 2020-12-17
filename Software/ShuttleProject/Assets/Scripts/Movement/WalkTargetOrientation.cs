using System;
using UnityEngine;

namespace Movement
{
    public class WalkTargetOrientation : MonoBehaviour
    {
        private Transform _parent;
        private Vector3 _direction;
        
        private void Start()
        {
            _parent = this.transform.parent;
            _direction = this.transform.position - _parent.transform.position;
        }

        private void Update()
        {
            this.transform.LookAt(_parent, _direction);
        }
    }
}