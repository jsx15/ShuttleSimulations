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
            if (go != null && go.transform.GetChildRecursiveByName("LeftHand(Clone)"))
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
            if (go != null && go.transform.GetChildRecursiveByName("RightHand(Clone)"))
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
            if (go != null && go.transform.GetChildRecursiveByName("RightHand(Clone)") && go.transform.GetChildRecursiveByName("LeftHand(Clone)"))
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
        ///     Check if object has hands
        /// </summary>
        /// <param name="go">Object to be checked</param>
        /// <returns>True if object has a hand</returns>
        public static bool HasHands(GameObject go)
        {
            return HasLeftHand(go) || HasRightHand(go);
        }
    }
}