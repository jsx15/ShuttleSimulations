using MMIUnity;
using UnityEngine;

namespace Scripts
{
    public static class HandChecker
    {
        /// <summary>
        ///     Check if object has left hand
        /// </summary>
        /// <param name="go">Object to be checked</param>
        /// <returns>True if object has left hand</returns>
        public static bool HasLeftHand(GameObject go)
        {
            if (go != null && (go.transform.GetChildRecursiveByName("LeftHand(Clone)") || go.transform.GetChildRecursiveByName("LeftHandSmallObject(Clone)")))
            {
                return true;
            }
            return false;
        }
    
        /// <summary>
        ///     Check if object has right hand
        /// </summary>
        /// <param name="go">Object to be checked</param>
        /// <returns>True if object has right hand</returns>
        public static bool HasRightHand(GameObject go)
        {
            if (go != null && (go.transform.GetChildRecursiveByName("RightHand(Clone)") || go.transform.GetChildRecursiveByName("RightHandSmallObject(Clone)")))
            {
                return true;
            }
            return false;
        }
        
        /// <summary>
        ///     Check if object has both hand
        /// </summary>
        /// <param name="go">Object to be checked</param>
        /// <returns>True if object has both hand</returns>
        public static bool HasBothHands(GameObject go)
        {
            if (HasLeftHand(go) && HasRightHand(go))
            {
                return true;
            }
            return false;
        }
        
        /// <summary>
        ///     Check if object is a hand
        /// </summary>
        /// <param name="go">Object to be checked</param>
        /// <returns>True if object is hand</returns>
        public static bool IsHand(GameObject go)
        {
            if (go != null && (go.name == "RightHand(Clone)" || go.name == "LeftHand(Clone)"))
            {
                return true;
            }
            return false;
        }

        public static bool IsTweezerHand(GameObject go)
        {
            if (go != null && (go.name == "RightHandSmallObject(Clone)" || go.name == "LeftHandSmallObject(Clone)"))
            {
                return true;
            }
            return false;
        }

        public static bool IsAnyHand(GameObject go)
        {
            return IsTweezerHand(go) || IsHand(go);
        }

        public static bool HasHands(GameObject go)
        {
            return HasLeftHand(go) || HasRightHand(go);
        }

        public static GameObject GetLeftHand(GameObject go)
        {
            if (go.transform.Find("LeftHand(Clone)") != null)
            {
                return go.transform.Find("LeftHand(Clone)").gameObject;
            }
            return go.transform.Find("LeftHandSmallObject(Clone)").gameObject;
        }
        
        public static GameObject GetRightHand(GameObject go)
        {
            if (go.transform.Find("RightHand(Clone)") != null)
            {
                return go.transform.Find("RightHand(Clone)").gameObject;
            }
            return go.transform.Find("RightHandSmallObject(Clone)").gameObject;
        }
    }
}