using System.Collections.Generic;
using MMICoSimulation;
using MMICSharp.MMIStandard.Utils;
using MMIStandard;
using MMIUnity.TargetEngine;
using UnityEngine;

public class TestAvatarBehavior : AvatarBehavior
{
    
    /// <summary>
    ///     Event to signal finished queue
    /// </summary>
    public delegate void Notify();
    public event Notify QueueFinished;
    
    /// <summary>
    ///     Override GUI
    /// </summary>
    protected override void GUIBehaviorInput()
    {
        //base.GUIBehaviorInput();
    }
    
    /// <summary>
    ///     Abort all running instructions
    /// </summary>
    public void Abort()
    {
        CoSimulator.Abort();
        QueueFinished?.Invoke();
    }

    /// <summary>
    ///     Play list of instructions
    /// </summary>
    /// <param name="list">Instructions to play</param>
    public void RunInstruction(List<MInstruction> list)
    {
        //IdleInstruction to ensure a underlying instruction to fall back
        string idleInstructionID = MInstructionFactory.GenerateID();
        MInstruction idleInstruction = new MInstruction(idleInstructionID, "Idle", "Pose/Idle");

        //Add Event handler
        this.CoSimulator.MSimulationEventHandler += this.CoSimulator_MSimulationEventHandler;


        //Instruction for signal an finished queue
        MInstruction defaultInstruction = new MInstruction(MInstructionFactory.GenerateID(), "FinishedQueue", "Pose/Idle")
        {
                StartCondition = list[list.Count - 1].ID + ":" + mmiConstants.MSimulationEvent_End + "+ 0.1", 
                Properties = PropertiesCreator.Create( CoSimTopic.OnStart, 
                idleInstructionID + ":" + CoSimAction.EndInstruction)
                
        };
        list.Add(defaultInstruction);


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
                    list[i].StartCondition = list[i - 1].StartCondition;
                    //list[i].StartCondition = list[i - 1].ID + ":" + "FingersPositioned + 0.1";
                }
                else
                {
                    list[i].StartCondition = list[i - 1].ID + ":" + mmiConstants.MSimulationEvent_End + "+ 0.1";
                }
            }
        }

        //Assign default idle
        CoSimulator.AssignInstruction(idleInstruction,
            new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});

        //Assign instruction from list
        foreach (var t in list)
        {
            CoSimulator.AssignInstruction(t,
                new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});
        }
    }

    /// <summary>
    ///     Callback for the co-simulation event handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CoSimulator_MSimulationEventHandler(object sender, MSimulationEvent e)
    {
        Debug.Log(e.Reference + " " + e.Name + " " + e.Type);
        
        //If Queue is finished abort all instructions and raise event
        if (e.Name.Equals("FinishedQueue"))
        {
            CoSimulator.Abort();
            QueueFinished?.Invoke();
        }
    }


}
