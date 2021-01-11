using System;
using UnityEngine;

namespace Movement
{
    public class WalkTargetOrientation : MonoBehaviour
    {
        /// <summary>
        ///     The parent of the WalkTarget
        /// </summary>
        private Transform _parent;
        
        /// <summary>
        ///     The direction from the WalkTarget to the _parent object
        /// </summary>
        private Vector3 _direction;
        private Vector3 _pos;
        
        // Start is called once per start
        private void Start()
        {
            _parent = transform.parent;
            _direction = transform.position - _parent.transform.position;
        }

        // Update is called once per frame
        private void Update()
        {
            transform.LookAt(_parent, _direction);
            _pos = transform.position;
            transform.position = new Vector3(_pos.x, 0.03f, _pos.z);
        }
    }
}