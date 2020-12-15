using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class WalkTargetManager
    {
        private static WalkTargetManager _instance = new WalkTargetManager();
        
        private List<GameObject> _walkTargetList = new List<GameObject>();
        private WalkTargetManager() {}
        
        public static WalkTargetManager getInstance()
        {
            return _instance;
        }

        public void AddWalkTarget(GameObject target)
        {
            _walkTargetList.Add(target);
        }
        
        public List<GameObject> GetWalkTarget()
        {
            return _walkTargetList;
        }
    }
}