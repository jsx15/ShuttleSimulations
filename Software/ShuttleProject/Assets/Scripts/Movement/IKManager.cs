
using System.Collections.Generic;
using MMIUnity.TargetEngine.Scene;
using UnityEngine;
/*
 * This Class is derived by a Youtube tutorial on inverse kinematics that i have rewritten for our purposes.
 * Link to the tutorial:https://www.youtube.com/watch?v=VdJGouwViPs 
 */
namespace Movement
{
    /// <summary>
    /// Inverse Kinematic Manager for finger movement.
    /// </summary>
    public class IKManager : MonoBehaviour
    {
        /// <summary>
        /// UnityBone of the fingertip.
        /// </summary>
        private UnityBone _tip;
        
        /// <summary>
        /// Target for the inverse kinematic.
        /// </summary>
        private Vector3 _target;
        
        /// <summary>
        /// Parent object of the hand corresponding to the finger.
        /// </summary>
        private Vector3 _initialTarget;
        
        /// <summary>
        /// List of every UnityBone in  a finger.
        /// </summary>
        private List<UnityBone> _fingerBones;
        
        /// <summary>
        /// List containing Rotation of each fingers bone.
        /// </summary>
        private List<Quaternion> _fingerBonesRotation;
        
        /// <summary>
        /// Reference to the SelectObject class since the IKManager will be added on Runtime.
        /// </summary>
        private SelectObject _selectObject; 
        
        /// <summary>
        /// Threshold which sets the distance between fingertip and object surface.
        /// </summary>
        private const float Threshold = 0.01f;
        
        /// <summary>
        /// The rate at which the rotation will rotate. A higher value equals a quicker, but less accurate rotation.
        /// </summary>
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
            //make sure that only the script running on the selected hand's fingers is responding
            if (_selectObject.GetObject().GetInstanceID().Equals(gameObject.transform.parent.gameObject.GetInstanceID()))
            {
                 // make sure that the hand isn't moving 
                 if (!_selectObject.IsMoving())
                 {
                     //Remain a certain distance to the target
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
                 //hand is moving 
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
        }
        
        /// <summary>
        /// Calculates the Slope for movement
        /// </summary>
        /// <param name="bone"></param>
        /// <returns>returns calculated slope</returns>
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
        
        /// <summary>
        /// Determines the target of the fingertip via Raycast. If the Raycast returns null the target is defaulted to the transform of the hands parent object
        /// </summary>
        /// <param name="transformTarget">transform of the object from which the Raycast will originate</param>
        /// <returns></returns>
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