using UnityEngine;

namespace Scripts
{
    public static class MoveTargetChecker
    {
        public static bool IsMoveTarget(GameObject go)
        {
            if (go.name == "moveTarget")
            {
                return true;
            }

            return false;
        }
    }
}