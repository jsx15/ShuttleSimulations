using System.Collections;
using System.Collections.Generic;
using MMIStandard;
using UnityEngine;

public class moveSeq : MonoBehaviour
{
    public bool start = false;

    void Update()
    {
        if (start)
        {
            TestAvatarBehavior beh = GetComponent<TestAvatarBehavior>();
            
            MInstruction walk = beh.WalkTo("WalkTargetLargeObject");
            MInstruction walk2 = beh.WalkTo("WalkTarget2");
            MInstruction walk3 = beh.WalkTo("WalkTarget");
            MInstruction walk4 = beh.WalkTo("WalkTarget2");
            
            List<MInstruction> reachList = new List<MInstruction>();
            
            reachList.AddRange(beh.ReachObject(GameObject.Find("Obstacle")));
            
            List<MInstruction> list = new List<MInstruction>();
            
            list.Add(walk);
            list.Add(walk2);
            list.Add(walk3);
            list.Add(walk4);
            list.AddRange(reachList);

            beh.runInstruction(list);
            start = false;
        }
    }
}