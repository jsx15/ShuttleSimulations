
using System.Collections.Generic;
using MMIUnity.TargetEngine.Scene;
using UnityEngine;
/*
 * This Class is derived by a Youtube tutorial on inverse kinematics that i have rewritten for our purposes.
 * Link to the tutorial:https://www.youtube.com/watch?v=VdJGouwViPs 
 */
namespace Movement
{
    public class IKManager : MonoBehaviour
    {
        private UnityBone _tip;
        private Vector3 _target;
        private Vector3 _initialTarget;
        private Vector3 _rayDirection;
        

        private List<UnityBone> _fingerBones;
        private List<Quaternion> _fingerBonesRotation;
        private SelectObject _selectObject; 
           

        private const float Threshold = 0.01f;
        private  float _rate;
        
        private void Start()
        {
            _fingerBones = new List<UnityBone>(transform.gameObject.GetComponentsInChildren<UnityBone>());
            _fingerBonesRotation = new List<Quaternion>();
            _tip = _fingerBones[3];
            _target = GetTarget(GetComponentInParent<Transform>());
            _initialTarget = _target;
            _selectObject = GameObject.Find("Main Camera").GetComponent<SelectObject>();
            foreach (UnityBone bone in _fingerBones)
            {
                _fingerBonesRotation.Add(bone.transform.localRotation);
            }

        }

        private void Update()
        {
            if (!_selectObject.IsMoving())
            {
                if (Vector3.Distance(_tip.transform.position, _target) > Threshold)
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
            else
            {
                UnityBone currentBone;
                for (int x = 0; x < _fingerBones.Count; x++)
                {
                    currentBone = _fingerBones[x];
                    currentBone.transform.localRotation = _fingerBonesRotation[x];
                }
            }

            
        }

        private float CalculateSlope(UnityBone bone)
        {
            float deltaTheta = 0.1f;
            _target = GetTarget(_tip.transform);

            float distance1 = Vector3.Distance(_tip.transform.position , _target);
            bone.transform.Rotate(Vector3.forward * deltaTheta);

            float distance2 = Vector3.Distance(_tip.transform.position, _target);
            bone.transform.Rotate(Vector3.forward * -deltaTheta);
            return (distance2 - distance1) / deltaTheta;
        }
        

        private Vector3 GetTarget(Transform transformTarget)
        {
            _rate = 500.0f;
            Ray ray = new Ray(transformTarget.position, transformTarget.right);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 0.2f))
            {
                return hitInfo.point;
            }
            _rate = 2000.0f;
            return _initialTarget;
        }

    }
}