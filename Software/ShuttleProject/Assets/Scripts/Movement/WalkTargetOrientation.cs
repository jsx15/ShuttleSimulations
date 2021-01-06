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
        
        // Start is called once per start
        private void Start()
        {
            _parent = this.transform.parent;
            _direction = this.transform.position - _parent.transform.position;
        }

        // Update is called once per frame
        private void Update()
        {
            this.transform.LookAt(_parent, _direction);
        }
    }
}