using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class WalkTargetManager
    {
        /// <summary>
        ///     The variable for the instance
        /// </summary>
        private static WalkTargetManager _instance = new WalkTargetManager();
        
        /// <summary>
        ///     List of all walk targets
        /// </summary>
        private List<GameObject> _walkTargetList = new List<GameObject>();
        
        /// <summary>
        ///      Constructor
        /// </summary>
        private WalkTargetManager() {}
        
        /// <summary>
        ///     Get instance
        /// </summary>
        /// <returns>Instance</returns>
        public static WalkTargetManager getInstance()
        {
            return _instance;
        }

        /// <summary>
        ///     Add walk target
        /// </summary>
        /// <param name="target">walk target to add</param>
        public void AddWalkTarget(GameObject target)
        {
            _walkTargetList.Add(target);
        }
        
        /// <summary>
        ///     Get list of all walk targets
        /// </summary>
        /// <returns>List with all walk targets</returns>
        public List<GameObject> GetWalkTarget()
        {
            return _walkTargetList;
        }
    }
}