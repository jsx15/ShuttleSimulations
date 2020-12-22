using UnityEngine;

namespace Scripts
{
    public static class MoveTargetChecker
    {
        /// <summary>
        ///     Check if object is a move target
        /// </summary>
        /// <param name="go">object to be checked</param>
        /// <returns>True if object is move target</returns>
        public static bool IsMoveTarget(GameObject go)
        {
            return go != null && go.name == "moveTarget";
        }
    }
}