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

            MInstruction walk5 = beh.WalkTo("WalkTargetSphere");


            List<MInstruction> list = new List<MInstruction> {walk5};


            List<MInstruction> reachList = new List<MInstruction>();
            //reachList.AddRange(beh.ReachObject(GameObject.Find("Sphere")));

            //reachList.AddRange(beh.MoveObject(GameObject.Find("Sphere"), GameObject.Find("SphereTarget")));

            // reachList.AddRange(beh.ReleaseObject());
            
            reachList.AddRange(beh.PickUp(GameObject.Find("Sphere")));

            //reachList.AddRange(beh.MoveObject(GameObject.Find("Sphere"), GameObject.Find("SphereTarget")));
            
            //reachList.Add(beh.WalkTo("WalkTarget"));
            
            reachList.AddRange(beh.ReleaseObject());
            
            list.AddRange(reachList);
            beh.RunInstruction(list);
            
            

            start = false;


        }
    }
}