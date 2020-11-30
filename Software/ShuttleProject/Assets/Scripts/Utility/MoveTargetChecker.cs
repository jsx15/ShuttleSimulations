using UnityEngine;

namespace Scripts
{
    public static class MoveTargetChecker
    {
        public static bool IsMoveTarget(GameObject go)
        {
            if ( go != null && go.name == "moveTarget")
            {
                return true;
            }

            return false;
        }
    }
}