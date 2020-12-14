using System.Collections.Generic;
using MMIStandard;
using UnityEngine;

public class moveSeq : MonoBehaviour
{
    public bool start = false;


    void Update()
    {
        TestAvatarBehavior beh = GetComponent<TestAvatarBehavior>();
        if (start)
        {
            
           // MInstruction walk5 = beh.WalkTo();

            List<MInstruction> list = new List<MInstruction> ();
            
            
           // list.Add(walk5);

            list.AddRange(beh.PickUp(GameObject.Find("Sphere")));


           //list.Add(beh.WalkTo();
            
            list.AddRange(beh.ReleaseObject());
            
            //list.Add(beh.WalkTo());
            
            
            beh.RunInstruction(list);

            start = false;
            
        }
    }
}