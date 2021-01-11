using System.IO;
using UnityEngine;

namespace Scripts
{
    public class PlaceHands : MonoBehaviour
    {
        /// <summary>
        ///     SelectObject
        /// </summary>
        public SelectObject selectObject;
        
        /// <summary>
        ///    
        /// </summary>
        private bool _hasRigidBody = false;
        
        /// <summary>
        ///     Offset
        /// </summary>
        private const float OffSetValue = 0.02f;
        
        /// <summary>
        ///     Box collider
        /// </summary>
        private BoxCollider _boxColliderLeftHand, _boxColliderRightHand;

        /// <summary>
        ///     Place left hand on selected object
        /// </summary>
        public void PlaceLeftHand()
        {
            
            GameObject go = selectObject.GetObject();
            Vector3 hitPoint = selectObject.GetHitPoint();
            Vector3 hitPointNormal = selectObject.GetHitPointNormal();
            
            selectObject.ResetColor();
            

            if (go != null)
            {
                if (!HandChecker.HasLeftHand(go) && !HandChecker.IsHand(go))
                {
                    if (go.GetComponent<Rigidbody>() != null)
                    {
                        //Destroy the RigidBody
                        Destroy(go.GetComponent<Rigidbody>());
                        _hasRigidBody = true;
                    }

                    Vector3 rotationLeft = new Vector3() ;
                            
                    //load leftHandPrefab and instantiate it with the predetermined parameters
                    GameObject leftHandPrefab =
                        Resources.Load("HandPrefab" + Path.DirectorySeparatorChar + "LeftHand") as GameObject;
                    GameObject leftHand = Instantiate(leftHandPrefab,
                        hitPoint + selectObject.GetDragAndRotate().GetOffsetAfterDrag() + hitPointNormal * OffSetValue ,
                        Quaternion.Euler(rotationLeft));
                    leftHand.transform.SetParent(go.transform);
                    leftHand.transform.rotation = Quaternion.FromToRotation(-leftHand.transform.right, hitPointNormal);
                            
                    //Add a BoxCollider to the hand
                    _boxColliderLeftHand = leftHand.AddComponent<BoxCollider>();
                    adjustBoxCollider(_boxColliderLeftHand, 0);
                }
            }
        }

        /// <summary>
        ///      Place right hand on selected object
        /// </summary>
        public void PlaceRightHand()
        {
            GameObject go = selectObject.GetObject();
            Vector3 hitPoint = selectObject.GetHitPoint();
            Vector3 hitPointNormal = selectObject.GetHitPointNormal();
            selectObject.ResetColor();

            if (go != null)
            {
                if (!HandChecker.HasRightHand(go) && !HandChecker.IsHand(go))
                {
                    if (go.GetComponent<Rigidbody>() != null)
                    {
                        //Destroy the RigidBody
                        Destroy(go.GetComponent<Rigidbody>());
                        _hasRigidBody = true;
                    }

                    Vector3 rotationRight = new Vector3();

                    //load rightHandPrefab and instantiate it with the predetermined parameters
                    GameObject rightHandPrefab =
                        Resources.Load("HandPrefab" + Path.DirectorySeparatorChar + "RightHand") as
                            GameObject;
                    GameObject rightHand = Instantiate(rightHandPrefab,
                        hitPoint + selectObject.GetDragAndRotate().GetOffsetAfterDrag() + hitPointNormal * OffSetValue,
                        Quaternion.Euler(rotationRight));
                    rightHand.transform.SetParent(go.transform);
                    rightHand.transform.rotation =
                        Quaternion.FromToRotation(-rightHand.transform.right, hitPointNormal);

                    //Add a BoxCollider to the hand
                    _boxColliderRightHand = rightHand.AddComponent<BoxCollider>();
                    adjustBoxCollider(_boxColliderRightHand, 1);
                }
            }
        }

        /// <summary>
        ///     Place right tweezer hand on selected object
        /// </summary>
        public void PlaceRightTweezerHand()
        {
            GameObject go = selectObject.GetObject();
            Vector3 hitPoint = selectObject.GetHitPoint();
            Vector3 hitPointNormal = selectObject.GetHitPointNormal();
            selectObject.ResetColor();

            if (go != null)
            {
                if (!HandChecker.HasRightHand(go) && !HandChecker.IsHand(go))
                {
                    if (go.GetComponent<Rigidbody>() != null)
                    {
                        //Destroy the RigidBody
                        Destroy(go.GetComponent<Rigidbody>());
                        _hasRigidBody = true;
                    }

                    Vector3 rotationRight = new Vector3();

                    //load rightHandPrefab and instantiate it with the predetermined parameters
                    GameObject rightHandPrefab =
                        Resources.Load("HandPrefab" + Path.DirectorySeparatorChar + "RightHandSmallObject") as
                            GameObject;
                    GameObject rightHand = Instantiate(rightHandPrefab,
                        hitPoint + selectObject.GetDragAndRotate().GetOffsetAfterDrag() + hitPointNormal * OffSetValue,
                        Quaternion.Euler(rotationRight));
                    rightHand.transform.SetParent(go.transform);
                    rightHand.transform.rotation =
                        Quaternion.FromToRotation(-rightHand.transform.right, hitPointNormal);

                    //Add a BoxCollider to the hand
                    _boxColliderRightHand = rightHand.AddComponent<BoxCollider>();
                    adjustBoxCollider(_boxColliderRightHand, 1);
                }
            }
        }

        public void PlaceLeftTweezerHand()
        {
            GameObject go = selectObject.GetObject();
            Vector3 hitPoint = selectObject.GetHitPoint();
            Vector3 hitPointNormal = selectObject.GetHitPointNormal();
            
            selectObject.ResetColor();
            

            if (go != null)
            {
                if (!HandChecker.HasLeftHand(go) && !HandChecker.IsHand(go))
                {
                    if (go.GetComponent<Rigidbody>() != null)
                    {
                        //Destroy the RigidBody
                        Destroy(go.GetComponent<Rigidbody>());
                        _hasRigidBody = true;
                    }

                    Vector3 rotationLeft = new Vector3() ;
                            
                    //load leftHandPrefab and instantiate it with the predetermined parameters
                    GameObject leftHandPrefab =
                        Resources.Load("HandPrefab" + Path.DirectorySeparatorChar + "LeftHandSmallObject") as GameObject;
                    GameObject leftHand = Instantiate(leftHandPrefab,
                        hitPoint + selectObject.GetDragAndRotate().GetOffsetAfterDrag() + hitPointNormal * OffSetValue ,
                        Quaternion.Euler(rotationLeft));
                    leftHand.transform.SetParent(go.transform);
                    leftHand.transform.rotation = Quaternion.FromToRotation(-leftHand.transform.right, hitPointNormal);
                            
                    //Add a BoxCollider to the hand
                    _boxColliderLeftHand = leftHand.AddComponent<BoxCollider>();
                    adjustBoxCollider(_boxColliderLeftHand, 0);
                }
            }
        }

        /// <summary>
        ///     Adjust box collider
        /// </summary>
        /// <param name="boxCollider"></param>
        /// <param name="position"></param>
        private void adjustBoxCollider(BoxCollider boxCollider, int position)
        {
            switch (position)
            {
                case 0:
                    boxCollider.size = new Vector3(0.04f, 0.2f, 0.15f);
                    boxCollider.center = new Vector3(-0.008f, 0.1f, -0.025f);
                    break;
                case 1:
                    boxCollider.size = new Vector3(0.04f, 0.2f, 0.15f);
                    boxCollider.center = new Vector3(-0.008f, 0.1f, 0.025f);
                    break;
            }
        }
    }
}