using System;
using MMIUnity;
using UnityEngine;

namespace Scripts
{
    public static class HandChecker
    {
        //Check if object has a LeftHand
        public static Boolean HasLeftHand(GameObject obj)
        {
            if (obj.transform.GetChildRecursiveByName("LeftHand(Clone)"))
            {
                return true;
            }
            return false;
        }
    
        //Check if object has a RightHand
        public static Boolean HasRightHand(GameObject obj)
        {
            if (obj.transform.GetChildRecursiveByName("RightHand(Clone)"))
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
            if (obj.name == "RightHand(Clone)" || obj.name == "LeftHand(Clone)")
            {
                return true;
            }
            return false;
        }
    }
}