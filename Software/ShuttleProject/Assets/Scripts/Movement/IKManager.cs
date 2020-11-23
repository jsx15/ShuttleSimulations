using System;
using System.Collections.Generic;
using MMIUnity.TargetEngine.Scene;
using UnityEngine;

namespace Movement
{
    public class IKManager : MonoBehaviour
    {
        private UnityBone _root;
        private UnityBone _tip;
        private Transform _target;
        private Vector3 _rayDirection;

        private List<UnityBone> _fingerBones;

        private float _threshold = 0.2f;
        private float _rate = 500.0f;
        private int _steps = 20;

        private void Start()
        {
            _fingerBones = new List<UnityBone>(transform.gameObject.GetComponentsInChildren<UnityBone>());
            _root = _fingerBones[0];
            _tip = _fingerBones[3];
            _target = GetTarget(_tip.transform);
        }

        private void Update()
        {
            if (Vector3.Distance(_tip.transform.position, _target.position) > _threshold)
            {
                UnityBone currentBone;
                for (int x = 0; x < _fingerBones.Count; x++)
                {
                    currentBone = _fingerBones[x];
                    float slope = CalculateSlope(currentBone);
                    currentBone.transform.Rotate(Vector3.forward * (-slope * _rate));
                }
            }
        }

        public float CalculateSlope(UnityBone _bone)
        {
            float deltaTheta = 0.1f;
            _target = GetTarget(_tip.transform);

            float distance1 = Vector3.Distance(_tip.transform.position , _target.position);
            _bone.transform.Rotate(Vector3.forward * deltaTheta);

            float distance2 = Vector3.Distance(_tip.transform.position, _target.position);
            _bone.transform.Rotate(Vector3.forward * -deltaTheta);
            return (distance2 - distance1) / deltaTheta;
        }

        private Transform GetTarget(Transform transformTarget)
        {
            Ray ray = new Ray(transformTarget.position, transformTarget.right);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 0.2f))
            { 
                return hitInfo.transform;
            }
            return transformTarget;
        }

    }
}