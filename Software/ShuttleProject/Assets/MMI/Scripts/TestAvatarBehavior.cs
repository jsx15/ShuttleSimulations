using MMICoSimulation;
using MMICSharp.MMIStandard.Utils;
using MMIStandard;
using MMIUnity.TargetEngine;
using MMIUnity.TargetEngine.Scene;
using UnityEngine;


public class TestAvatarBehavior : AvatarBehavior
{

    private string carryID;

    protected override void GUIBehaviorInput()
    {
        if (GUI.Button(new Rect(10, 10, 120, 50), "Idle"))
        {
            //Create a new idle instruction of type idle
            MInstruction idleInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Idle", "idle");

            //Abort all instructions
            this.CoSimulator.Abort();

            //Assign the instruction to the co-simulator
            this.CoSimulator.AssignInstruction(idleInstruction, null);
        }


        //Walkin to a specific point
        if (GUI.Button(new Rect(140, 10, 120, 50), "Walk to"))
        {
            //First create the walk instruction to walk to the specific object
            MInstruction walkInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Walk", "walk")
            {
                //Write the target id to the properties (the target id is gathered from the scene).
                //An alternative way to get a target would be to directly use the MMISceneObject as editor variable
                Properties = PropertiesCreator.Create("TargetID", UnitySceneAccess.Instance.GetSceneObjectByName("WalkTarget").ID)
            };

            //Furthermore create an idle instruction
            MInstruction idleInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Idle", "idle")
            {
                //Start idle after walk has been finished
                StartCondition = walkInstruction.ID + ":" + mmiConstants.MSimulationEvent_End 
            };

            //Abort all current tasks
            this.CoSimulator.Abort();

            //Assign walk and idle instruction
            this.CoSimulator.AssignInstruction(walkInstruction, null);
            this.CoSimulator.AssignInstruction(idleInstruction, null);
            this.CoSimulator.MSimulationEventHandler += this.CoSimulator_MSimulationEventHandler;
        }


        //Creates a reach instruction to reach a particular object
        if (GUI.Button(new Rect(270, 10, 120, 50), "Reach Object"))
        {
            //As always create an idle instruction first
            MInstruction idleInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Idle", "idle");

            //Now create a specific instruction to reach with the right hand
            MInstruction reachRight = new MInstruction(MInstructionFactory.GenerateID(), "reach right", "Pose/Reach")
            {
                Properties = new System.Collections.Generic.Dictionary<string, string>()
                {
                    //The object that descibres the reach posture
                    { "TargetID", UnitySceneAccess.Instance["GraspTargetR"].ID},
                    //The hand that is used
                    { "Hand","Right"}
                }
            };

            //this.CoSimulator.Abort();
            this.CoSimulator.AssignInstruction(idleInstruction, null);
            this.CoSimulator.AssignInstruction(reachRight, null);
        }




        if (GUI.Button(new Rect(530, 10, 120, 50), "Release Object"))
        {
            MInstruction releaseRight = new MInstruction(MInstructionFactory.GenerateID(), "release object", "Object/Release")
            {
                Properties = PropertiesCreator.Create("Hand", "Right", CoSimTopic.OnStart, carryID + ":" + CoSimAction.EndInstruction),
            };
            MInstruction releaseLeft = new MInstruction(MInstructionFactory.GenerateID(), "release object", "Object/Release")
            {
                Properties = PropertiesCreator.Create("Hand", "Left", CoSimTopic.OnStart, carryID + ":" + CoSimAction.EndInstruction),
            };


            this.CoSimulator.AssignInstruction(releaseRight, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
            this.CoSimulator.AssignInstruction(releaseLeft, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });

        }


        if (GUI.Button(new Rect(400, 10, 120, 50), "Move Object"))
        {
            MInstruction idleInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Idle", "idle");

            MInstruction moveObject = new MInstruction(MInstructionFactory.GenerateID(), "move object", "Object/Move")
            {
                Properties = PropertiesCreator.Create("SubjectID", UnitySceneAccess.Instance["GraspObject"].ID, "Hand", "Right", "TargetID", UnitySceneAccess.Instance["PositioningTarget"].ID),
            };

            this.CoSimulator.Abort();
            this.CoSimulator.AssignInstruction(idleInstruction, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
            this.CoSimulator.AssignInstruction(moveObject, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
        }


        if (GUI.Button(new Rect(790, 10, 120, 50), "Pick-up"))
        {
            MInstruction idleInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Idle", "idle");

            MInstruction reachInstruction = new MInstruction(MInstructionFactory.GenerateID(), "reach", "Pose/Reach")
            {
                Properties = PropertiesCreator.Create("TargetID", UnitySceneAccess.Instance["GraspTargetR"].ID, "Hand", "Right"),
            };

            carryID = MInstructionFactory.GenerateID();
            MInstruction carryInstruction = new MInstruction(carryID, "carry object", "Object/Carry")
            {
                Properties = PropertiesCreator.Create("TargetID", UnitySceneAccess.Instance["GraspObject"].ID, "Hand", "Right"),
                StartCondition = reachInstruction.ID +":"+ mmiConstants.MSimulationEvent_End + "+ 0.01"
            };

           
            this.CoSimulator.AssignInstruction(idleInstruction, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
            this.CoSimulator.AssignInstruction(reachInstruction, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
            this.CoSimulator.AssignInstruction(carryInstruction, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
            this.CoSimulator.MSimulationEventHandler += this.CoSimulator_MSimulationEventHandler;

        }

        if (GUI.Button(new Rect(660, 10, 120, 50), "Carry Object"))
        {
            carryID = MInstructionFactory.GenerateID();
            MInstruction carryInstruction = new MInstruction(carryID, "carry object", "Object/Carry")
            {
                Properties = PropertiesCreator.Create("TargetID", UnitySceneAccess.Instance["GraspObject"].ID, "Hand", "Right")//, "CarryTarget", UnitySceneAccess.Instance["CarryTarget"].ID),
            };

            this.CoSimulator.AssignInstruction(carryInstruction, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
        }






        if (GUI.Button(new Rect(10, 70, 220, 50), "Pickup large object"))
        {
            MInstruction walkInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Walk", "walk")
            {
                Properties = PropertiesCreator.Create("TargetID", UnitySceneAccess.Instance.GetSceneObjectByName("WalkTargetLargeObject").ID)
            };

            MInstruction idleInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Idle", "idle")
            {
                StartCondition = walkInstruction.ID + ":" + mmiConstants.MSimulationEvent_End
            };

            MInstruction reachLeft = new MInstruction(MInstructionFactory.GenerateID(), "reachLeft", "Pose/Reach")
            {
                Properties = PropertiesCreator.Create("TargetID", UnitySceneAccess.Instance["LargeObjectGraspL"].ID, "Hand", "Left"),
                StartCondition = walkInstruction.ID + ":" + mmiConstants.MSimulationEvent_End
            };

            MInstruction reachRight = new MInstruction(MInstructionFactory.GenerateID(), "reachRight", "Pose/Reach")
            {
                Properties = PropertiesCreator.Create("TargetID", UnitySceneAccess.Instance["LargeObjectGraspR"].ID, "Hand", "Right"),
                StartCondition = walkInstruction.ID + ":" + mmiConstants.MSimulationEvent_End
            };

            carryID = MInstructionFactory.GenerateID();
            MInstruction carryInstruction = new MInstruction(carryID, "carry object", "Object/Carry")
            {
                Properties = PropertiesCreator.Create("TargetID", UnitySceneAccess.Instance["LargeObject"].ID, "Hand", "Both", "CarryDistance", 0.4f.ToString(), "CarryTarget", UnitySceneAccess.Instance["CarryTarget"].ID),
                StartCondition = reachLeft.ID + ":" + mmiConstants.MSimulationEvent_End + " && " + reachRight.ID + ":" + mmiConstants.MSimulationEvent_End
            };

            this.CoSimulator.AssignInstruction(walkInstruction, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
            this.CoSimulator.AssignInstruction(idleInstruction, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
            this.CoSimulator.AssignInstruction(reachLeft, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
            this.CoSimulator.AssignInstruction(reachRight, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });

            this.CoSimulator.AssignInstruction(carryInstruction, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
            this.CoSimulator.MSimulationEventHandler += this.CoSimulator_MSimulationEventHandler;
        }

        if (GUI.Button(new Rect(240, 70, 220, 50), "Move Large Object both handed"))
        {
            MInstruction moveInstruction = new MInstruction(MInstructionFactory.GenerateID(), "move object", "Object/Move")
            {
                Properties = PropertiesCreator.Create("SubjectID", UnitySceneAccess.Instance["LargeObject"].ID, "Hand", "Both", "TargetID", UnitySceneAccess.Instance["LargeObjectPositioningTarget"].ID, "HoldDuration", 1.0f.ToString()),

                //Terminate the carry
                Action = CoSimTopic.OnStart + "->" + carryID + ":" + CoSimAction.EndInstruction,
            };

            MInstruction releaseLeft = new MInstruction(MInstructionFactory.GenerateID(), "release object left", "Object/Release")
            {
                Properties = PropertiesCreator.Create("Hand", "Left"),
                StartCondition = moveInstruction.ID + ":" + mmiConstants.MSimulationEvent_End
            };

            MInstruction releaseRight = new MInstruction(MInstructionFactory.GenerateID(), "release object right", "Object/Release")
            {
                Properties = PropertiesCreator.Create("Hand", "Right"),
                StartCondition = moveInstruction.ID + ":" + mmiConstants.MSimulationEvent_End
            };


            this.CoSimulator.AssignInstruction(moveInstruction, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
            this.CoSimulator.AssignInstruction(releaseLeft, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
            this.CoSimulator.AssignInstruction(releaseRight, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });


        }


        if (GUI.Button(new Rect(470, 70, 210, 50), "Reach Object Single"))
        {

            MInstruction idleInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Idle", "idle");


            MInstruction reachRight = new MInstruction(MInstructionFactory.GenerateID(), "reach right", "Pose/Reach")
            {
                Properties = PropertiesCreator.Create("TargetID", UnitySceneAccess.Instance["GraspTargetR"].ID, "Hand", "Right"),
            };




            this.CoSimulator.Abort();
            this.CoSimulator.AssignInstruction(idleInstruction, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });
            this.CoSimulator.AssignInstruction(reachRight, new MSimulationState() { Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture() });


        }

        if (GUI.Button(new Rect(700, 70, 160, 50), "Abort"))
        {
            this.CoSimulator.Abort();
        }



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
