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

        /// <summary>
        ///     Checks if object has hands as children
        /// </summary>
        /// <param name="go">Object to be checked</param>
        /// <returns>True if object has ahnds</returns>
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
        
        /// <summary>
        ///     Check if object has hands
        /// </summary>
        /// <param name="go">Object to be checked</param>
        /// <returns>True if object has a hand</returns>
        public static bool HasHands(GameObject go)
        {
            return HasLeftHand(go) || HasRightHand(go);
        }

        /// <summary>
        ///     Returns the lefthand of an object
        /// </summary>
        /// <param name="go">Object to get the hand from</param>
        /// <returns>lefthand of go</returns>
        public static GameObject GetLeftHand(GameObject go)
        {
            if (go.transform.Find("LeftHand(Clone)") != null)
            {
                return go.transform.Find("LeftHand(Clone)").gameObject;
            }
            return go.transform.Find("LeftHandSmallObject(Clone)").gameObject;
        }
        
        /// <summary>
        ///     Returns the righthand of an object
        /// </summary>
        /// <param name="go">Object to get the hand from</param>
        /// <returns>righthand of go</returns>
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