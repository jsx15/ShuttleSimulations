using System.Collections.Generic;
using MMIStandard;
using MMIUnity.TargetEngine;
using UnityEngine;

public class TestAvatarBehavior : AvatarBehavior
{
    public void RunInstruction(List<MInstruction> list)
    {
        print(list.Count);
        MInstruction idleInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Idle", "Pose/Idle");
        this.CoSimulator.MSimulationEventHandler += this.CoSimulator_MSimulationEventHandler;

        for (int i = 0; i < list.Count; i++)
        {
            if (i > 0)
            {
                if (list[i - 1].Name.Equals("carry object"))
                {
                    list[i].StartCondition = list[i - 1].ID + ":" + "PositioningFinished + 0.1";
                }
                else if (list[i - 1].Name.Equals("Move Fingers"))
                {
                    list[i].StartCondition = list[i - 1].ID + ":" + "FingersPositioned + 0.1";
                }
                else if (list[i - 1].Name.Equals("Release Fingers"))
                {
                    list[i].StartCondition = list[i - 1].StartCondition ;
                    //list[i].StartCondition = list[i - 1].ID + ":" + "FingersPositioned + 0.1";
                }
                else
                {
                    list[i].StartCondition = list[i - 1].ID + ":" + mmiConstants.MSimulationEvent_End + "+ 0.1";
                }
            }
        }

        foreach (var t in list)
        {
            CoSimulator.AssignInstruction(t,
                new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});
            
            CoSimulator.AssignInstruction(idleInstruction,
                new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});
        
        }
        //CoSimulator.AssignInstruction(idleInstruction,new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});
    }
    
    /// <summary>
    /// Callback for the co-simulation event handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CoSimulator_MSimulationEventHandler(object sender, MSimulationEvent e)
    {
        Debug.Log(e.Reference + " " + e.Name + " " + e.Type);
    }


}
