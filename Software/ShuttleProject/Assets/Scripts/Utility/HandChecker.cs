using MMIUnity;
using UnityEngine;

namespace Scripts
{
    public static class HandChecker
    {
        //Check if object has a LeftHand
        public static bool HasLeftHand(GameObject go)
        {
            if (go.transform.GetChildRecursiveByName("LeftHand(Clone)"))
            {
                return true;
            }
            return false;
        }
    
        //Check if object has a RightHand
        public static bool HasRightHand(GameObject go)
        {
            if (go.transform.GetChildRecursiveByName("RightHand(Clone)"))
            {
                return true;
            }
            return false;
        }
        
        //Check if object has both hands
        public static Boolean HasBothHands(GameObject obj)
        {
            if (obj.transform.GetChildRecursiveByName("RightHand(Clone)") && obj.transform.GetChildRecursiveByName("LeftHand(Clone)"))
            {
                return true;
            }
            return false;
        }
        
        public static Boolean IsHand(GameObject obj)
        {
            if (obj.name == "RightHand(Clone)" || obj.name == "LeftHand(Clone)" )
        //Check if object is a Hand
        public static bool IsHand(GameObject go)
        {
            if (go.name == "RightHand(Clone)" || go.name == "LeftHand(Clone)")
            {
                return true;
            }
            return false;
        }
    }
}