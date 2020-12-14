using System;
using System.Collections.Generic;
using MMICoSimulation;
using MMICSharp.MMIStandard.Utils;
using MMIStandard;
using MMIUnity;
using MMIUnity.TargetEngine;
using MMIUnity.TargetEngine.Scene;
using Scripts;
using UnityEngine;


public class TestAvatarBehavior : AvatarBehavior
{
    public WalkTrajectory WalkTrajectory;
    public MMISceneObject WalkTrajectoryTarget;
    
    private Vector3 _hitPoint;
    private string _carryID;
    private BoxCollider _boxColliderLeftHand;
    private BoxCollider _boxColliderRightHand;
    private ObjectBounds _objectBounds;
    private float _offSetValue = 0.005f;
    private String _tempEvent;

    protected override void GUIBehaviorInput()
    {
        //base.GUIBehaviorInput();
    }

    /*protected override void GUIBehaviorInput()
    {
        if (GUI.Button(new Rect(10, 10, 120, 50), "Idle"))
        {
            //Create a new idle instruction of type idle
            MInstruction idleInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Idle", "Pose/Idle");

            //Abort all instructions
            this.CoSimulator.Abort();

            //Assign the instruction to the co-simulator
            this.CoSimulator.AssignInstruction(idleInstruction, null);
        }


        //Walkin to a specific point
        if (GUI.Button(new Rect(140, 10, 120, 50), "Walk to"))
        {
            //First create the walk instruction to walk to the specific object
            MInstruction walkInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Walk", "Locomotion/Walk")
            {
                //Write the target id to the properties (the target id is gathered from the scene).
                //An alternative way to get a target would be to directly use the MMISceneObject as editor variable
                //Force path means a straight line path is enfored if no path can be found
                Properties = PropertiesCreator.Create("TargetID",
                    UnitySceneAccess.Instance.GetSceneObjectByName("WalkTarget").ID, "ForcePath",
                    true.ToString()) //"ReplanningTime", 500.ToString())
            };



            //Furthermore create an idle instruction
            MInstruction idleInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Idle", "Pose/Idle")
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


        if (GUI.Button(new Rect(10, 130, 150, 50), "Walk to (trajectory)"))
        {
            //First create the walk instruction to walk to the specific object
            MInstruction walkInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Walk", "Locomotion/Walk")
            {
                //Use the walk trajectory as target
                Properties = PropertiesCreator.Create("TargetID", WalkTrajectoryTarget.MSceneObject.ID)
            };

            //Get the walk trajectory (if available)
            MConstraint constraint = this.WalkTrajectory.GetPathConstraint();

            //Create empty constraint list
            walkInstruction.Constraints = new System.Collections.Generic.List<MConstraint>();

            //Add constraint id as property
            walkInstruction.Properties.Add("Trajectory", constraint.ID);

            //Add the constraint
            walkInstruction.Constraints.Add(constraint);

            //Furthermore create an idle instruction
            MInstruction idleInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Idle", "Pose/Idle")
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
            MInstruction idleInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Idle", "Pose/Idle");

            //Now create a specific instruction to reach with the right hand
            MInstruction reachRight = new MInstruction(MInstructionFactory.GenerateID(), "reach right", "Pose/Reach")
            {
                Properties = new System.Collections.Generic.Dictionary<string, string>()
                {
                    //The object that descibres the reach posture
                    {"TargetID", UnitySceneAccess.Instance["GraspTargetR"].ID},
                    //The hand that is used
                    {"Hand", "Right"}
                }
            };

            //this.CoSimulator.Abort();
            this.CoSimulator.AssignInstruction(idleInstruction, null);
            this.CoSimulator.AssignInstruction(reachRight, null);
        }




        if (GUI.Button(new Rect(530, 10, 120, 50), "Release Object"))
        {
            MInstruction releaseRight =
                new MInstruction(MInstructionFactory.GenerateID(), "release object", "Object/Release")
                {
                    Properties = PropertiesCreator.Create("Hand", "Right", CoSimTopic.OnStart,
                        carryID + ":" + CoSimAction.EndInstruction),
                };
            MInstruction releaseLeft =
                new MInstruction(MInstructionFactory.GenerateID(), "release object", "Object/Release")
                {
                    Properties = PropertiesCreator.Create("Hand", "Left", CoSimTopic.OnStart,
                        carryID + ":" + CoSimAction.EndInstruction),
                };


            this.CoSimulator.AssignInstruction(releaseRight,
                new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});
            this.CoSimulator.AssignInstruction(releaseLeft,
                new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});

        }


        if (GUI.Button(new Rect(400, 10, 120, 50), "Move Object"))
        {
            MInstruction idleInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Idle", "Pose/Idle");

            MInstruction moveObject = new MInstruction(MInstructionFactory.GenerateID(), "move object", "Object/Move")
            {
                Properties = PropertiesCreator.Create("SubjectID", UnitySceneAccess.Instance["GraspObject"].ID, "Hand",
                    "Right", "TargetID", UnitySceneAccess.Instance["PositioningTarget"].ID),
            };

            this.CoSimulator.Abort();
            this.CoSimulator.AssignInstruction(idleInstruction,
                new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});
            this.CoSimulator.AssignInstruction(moveObject,
                new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});
        }


        if (GUI.Button(new Rect(790, 10, 120, 50), "Pick-up"))
        {
            MInstruction idleInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Idle", "Pose/Idle");

            MInstruction reachInstruction = new MInstruction(MInstructionFactory.GenerateID(), "reach", "Pose/Reach")
            {
                Properties = PropertiesCreator.Create("TargetID", UnitySceneAccess.Instance["GraspTargetR"].ID, "Hand",
                    "Right"),
            };

            carryID = MInstructionFactory.GenerateID();
            MInstruction carryInstruction = new MInstruction(carryID, "carry object", "Object/Carry")
            {
                Properties = PropertiesCreator.Create("TargetID", UnitySceneAccess.Instance["GraspObject"].ID, "Hand",
                    "Right"),
                StartCondition = reachInstruction.ID + ":" + mmiConstants.MSimulationEvent_End + "+ 0.01"
            };


            this.CoSimulator.AssignInstruction(idleInstruction,
                new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});
            this.CoSimulator.AssignInstruction(reachInstruction,
                new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});
            this.CoSimulator.AssignInstruction(carryInstruction,
                new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});
            this.CoSimulator.MSimulationEventHandler += this.CoSimulator_MSimulationEventHandler;

        }

        if (GUI.Button(new Rect(660, 10, 120, 50), "Carry Object"))
        {
            carryID = MInstructionFactory.GenerateID();
            MInstruction carryInstruction = new MInstruction(carryID, "carry object", "Object/Carry")
            {
                Properties = PropertiesCreator.Create("TargetID", UnitySceneAccess.Instance["GraspObject"].ID, "Hand",
                    "Right") //, "CarryTarget", UnitySceneAccess.Instance["CarryTarget"].ID),
            };

            this.CoSimulator.AssignInstruction(carryInstruction,
                new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});
        }






        if (GUI.Button(new Rect(10, 70, 220, 50), "Pickup large object"))
        {
            MInstruction walkInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Walk", "Locomotion/Walk")
            {
                Properties = PropertiesCreator.Create("TargetID",
                    UnitySceneAccess.Instance.GetSceneObjectByName("WalkTargetLargeObject").ID)
            };

            MInstruction idleInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Idle", "Pose/Idle")
            {
                StartCondition = walkInstruction.ID + ":" + mmiConstants.MSimulationEvent_End
            };

            MInstruction reachLeft = new MInstruction(MInstructionFactory.GenerateID(), "reachLeft", "Pose/Reach")
            {
                Properties = PropertiesCreator.Create("TargetID", UnitySceneAccess.Instance["LargeObjectGraspL"].ID,
                    "Hand", "Left"),
                StartCondition = walkInstruction.ID + ":" + mmiConstants.MSimulationEvent_End
            };

            MInstruction reachRight = new MInstruction(MInstructionFactory.GenerateID(), "reachRight", "Pose/Reach")
            {
                Properties = PropertiesCreator.Create("TargetID", UnitySceneAccess.Instance["LargeObjectGraspR"].ID,
                    "Hand", "Right"),
                StartCondition = walkInstruction.ID + ":" + mmiConstants.MSimulationEvent_End
            };

            carryID = MInstructionFactory.GenerateID();
            MInstruction carryInstruction = new MInstruction(carryID, "carry object", "Object/Carry")
            {
                Properties = PropertiesCreator.Create("TargetID", UnitySceneAccess.Instance["LargeObject"].ID, "Hand",
                    "Both", "CarryDistance", 0.4f.ToString(), "CarryTarget",
                    UnitySceneAccess.Instance["CarryTarget"].ID),
                StartCondition = reachLeft.ID + ":" + mmiConstants.MSimulationEvent_End + " && " + reachRight.ID + ":" +
                                 mmiConstants.MSimulationEvent_End
            };

            this.CoSimulator.AssignInstruction(walkInstruction,
                new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});
            this.CoSimulator.AssignInstruction(idleInstruction,
                new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});
            this.CoSimulator.AssignInstruction(reachLeft,
                new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});
            this.CoSimulator.AssignInstruction(reachRight,
                new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});

            this.CoSimulator.AssignInstruction(carryInstruction,
                new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});
            this.CoSimulator.MSimulationEventHandler += this.CoSimulator_MSimulationEventHandler;
        }

        if (GUI.Button(new Rect(240, 70, 220, 50), "Move Large Object both handed"))
        {
            MInstruction moveInstruction =
                new MInstruction(MInstructionFactory.GenerateID(), "move object", "Object/Move")
                {
                    Properties = PropertiesCreator.Create("SubjectID", UnitySceneAccess.Instance["LargeObject"].ID,
                        "Hand", "Both", "TargetID", UnitySceneAccess.Instance["LargeObjectPositioningTarget"].ID,
                        "HoldDuration", 1.0f.ToString()),

                    //Terminate the carry
                    Action = CoSimTopic.OnStart + "->" + carryID + ":" + CoSimAction.EndInstruction,
                };

            MInstruction releaseLeft =
                new MInstruction(MInstructionFactory.GenerateID(), "release object left", "Object/Release")
                {
                    Properties = PropertiesCreator.Create("Hand", "Left"),
                    StartCondition = moveInstruction.ID + ":" + mmiConstants.MSimulationEvent_End
                };

            MInstruction releaseRight =
                new MInstruction(MInstructionFactory.GenerateID(), "release object right", "Object/Release")
                {
                    Properties = PropertiesCreator.Create("Hand", "Right"),
                    StartCondition = moveInstruction.ID + ":" + mmiConstants.MSimulationEvent_End
                };


            this.CoSimulator.AssignInstruction(moveInstruction,
                new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});
            this.CoSimulator.AssignInstruction(releaseLeft,
                new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});
            this.CoSimulator.AssignInstruction(releaseRight,
                new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});


        }


        if (GUI.Button(new Rect(470, 70, 210, 50), "Reach Object Single"))
        {

            MInstruction idleInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Idle", "Pose/Idle");


            MInstruction reachRight = new MInstruction(MInstructionFactory.GenerateID(), "reach right", "Pose/Reach")
            {
                Properties = PropertiesCreator.Create("TargetID", UnitySceneAccess.Instance["GraspTargetR"].ID, "Hand",
                    "Right"),
            };




            this.CoSimulator.Abort();
            this.CoSimulator.AssignInstruction(idleInstruction,
                new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});
            this.CoSimulator.AssignInstruction(reachRight,
                new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});


        }

        if (GUI.Button(new Rect(700, 70, 160, 50), "Abort"))
        {
            this.CoSimulator.Abort();
        }
    }*/

    public MInstruction WalkTo(String s)
    {
        MInstruction walkInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Walk", "Locomotion/Walk")
        {
            Properties = PropertiesCreator.Create("TargetID", UnitySceneAccess.Instance.GetSceneObjectByName(s).ID)
        };
        
        return walkInstruction;
    }

    public List<MInstruction> ReachObject(GameObject go)
    {
        List<MInstruction> list = new List<MInstruction>();

        if (HandChecker.HasLeftHand(go))
        {
            //Get UnitySceneAccess ID of hand
            GameObject hand = go.transform.GetChildRecursiveByName("LeftHand(Clone)").gameObject;
            String objectID = hand.GetComponent<MMISceneObject>().MSceneObject.ID;

            //Now create a specific instruction to reach with the right hand
            MInstruction reachLeft = new MInstruction(MInstructionFactory.GenerateID(), "reach left", "Pose/Reach")
            {
                Properties = PropertiesCreator.Create("TargetID", objectID,
                    "Hand", "Left")
            };
            // this.CoSimulator.AssignInstruction(reachLeft, null);
            list.Add(reachLeft);
            
            /*
            //Indicates whether a release motion should be performed
            bool Release = false;
            //The desired Hand pose (rotations of the finger Joints)
            UnityHandPose leftHandPose = hand.GetComponent<UnityHandPose>();
            
            //Create the instruction to move the fingers
            MInstruction moveFingersInstructions = new MInstruction(System.Guid.NewGuid().ToString(), "Move fingers", "Pose/MoveFingers")
            {
                Properties = new Dictionary<string, string>()
                {
                    {"Release", Release.ToString() },
                    {"Hand", "Left" }
                },
                Constraints = new List<MConstraint>()
            };
            string constraintID = System.Guid.NewGuid().ToString();
            moveFingersInstructions.Properties.Add("HandPose", constraintID);
            moveFingersInstructions.Constraints.Add(new MConstraint()
            {
                ID = constraintID,
                PostureConstraint = leftHandPose.GetPostureConstraint()
            });
            // this.CoSimulator.AssignInstruction(moveFingersInstructions, null);
            list.Add(moveFingersInstructions);*/
        }

        if (HandChecker.HasRightHand(go))
        {
            //Get UnitySceneAccess ID of hand
            GameObject hand = go.transform.GetChildRecursiveByName("RightHand(Clone)").gameObject;
            String objectID = hand.GetComponent<MMISceneObject>().MSceneObject.ID;
            //Now create a specific instruction to reach with the right hand
            MInstruction reachRight = new MInstruction(MInstructionFactory.GenerateID(), "reach right", "Pose/Reach")
            {
                Properties = PropertiesCreator.Create("TargetID", objectID, "Hand", "Right")
            };
            // this.CoSimulator.AssignInstruction(reachRight, null);
            list.Add(reachRight);

            /*
            //Indicates whether a release motion should be performed
            bool Release = false;
            //The desired Hand pose (rotations of the finger Joints)
            UnityHandPose leftHandPose = hand.GetComponent<UnityHandPose>();
            
            //Create the instruction to move the fingers
            MInstruction moveFingersInstructions = new MInstruction(System.Guid.NewGuid().ToString(), "Move fingers", "Pose/MoveFingers")
            {
                Properties = new Dictionary<string, string>()
                {
                    {"Release", Release.ToString() },
                    {"Hand", "Right" }
                },
                Constraints = new List<MConstraint>()
            };
            string constraintID = System.Guid.NewGuid().ToString();
            moveFingersInstructions.Properties.Add("HandPose", constraintID);
            moveFingersInstructions.Constraints.Add(new MConstraint()
            {
                ID = constraintID,
                PostureConstraint = leftHandPose.GetPostureConstraint()
            });
            // this.CoSimulator.AssignInstruction(moveFingersInstructions, null);
            list.Add(moveFingersInstructions);*/
        }

        list.AddRange(MakeHandPose(go));
        
        return list;
    }

    public List<MInstruction> ReleaseObject()
    {
        List<MInstruction> list = new List<MInstruction>();
        list.AddRange(ReleaseHandPose());

        MInstruction releaseLeft =
            new MInstruction(MInstructionFactory.GenerateID(), "release object", "Object/Release")
            {
                Properties = PropertiesCreator.Create("Hand", "Left", CoSimTopic.OnStart,
                    _carryID + ":" + CoSimAction.EndInstruction),
            };

        list.Add(releaseLeft);

        MInstruction releaseRight =
            new MInstruction(MInstructionFactory.GenerateID(), "release object", "Object/Release")
            {
                Properties = PropertiesCreator.Create("Hand", "Right", CoSimTopic.OnStart,
                    _carryID + ":" + CoSimAction.EndInstruction),
            };

        list.Add(releaseRight);

        return list;
    }

    public List<MInstruction> MoveObject(GameObject obj, GameObject positionTarget)
    {
        List<MInstruction> list = new List<MInstruction>();

        String objectID = obj.GetComponent<MMISceneObject>().MSceneObject.ID;
        String targetPositionID = positionTarget.GetComponent<MMISceneObject>().MSceneObject.ID;

        MInstruction moveObject = new MInstruction(MInstructionFactory.GenerateID(), "move object", "Object/Move")
        {
            Properties = PropertiesCreator.Create("SubjectID", objectID, "Hand",
                "Left", "TargetID", targetPositionID)
        };

        list.Add(moveObject);
        return list;
    }

    public List<MInstruction> PickUp(GameObject obj)
    {
        List<MInstruction> list = new List<MInstruction>();
        list.AddRange(ReachObject(obj));

        String objID = obj.GetComponent<MMISceneObject>().MSceneObject.ID;

        _carryID = MInstructionFactory.GenerateID();
        if (list.Count == 2)
        {
            MInstruction carryInstruction =
                new MInstruction(_carryID, "carry object", "Object/Carry")
                {
                    Properties = PropertiesCreator.Create("TargetID", objID, "Hand",
                        "Both"),
                };
            list.Add(carryInstruction);
        }
        else if (list[0].Name.Equals("reach right"))
        {
            MInstruction carryInstruction = new MInstruction(_carryID, "carry object", "Object/Carry")
            {
                Properties = PropertiesCreator.Create("TargetID", objID, "Hand",
                    "Right"),
            };
            list.Add(carryInstruction);
        }
        else
        {
            MInstruction carryInstruction =
                new MInstruction(_carryID, "carry object", "Object/Carry")
                {
                    Properties = PropertiesCreator.Create("TargetID", objID, "Hand",
                        "Left"),
                };
            list.Add(carryInstruction);
        }
        
        return list;
    }

    //Define a HandPose for the given object
    private List<MInstruction> MakeHandPose(GameObject go)
    {
        List<MInstruction> list = new List<MInstruction>();
        if (HandChecker.HasLeftHand(go))
        {
            //Get UnitySceneAccess ID of hand
            GameObject hand = go.transform.GetChildRecursiveByName("LeftHand(Clone)").gameObject;
            String objectID = hand.GetComponent<MMISceneObject>().MSceneObject.ID;
            
            //The desired Hand pose (rotations of the finger Joints)
            UnityHandPose leftHandPose = hand.GetComponent<UnityHandPose>();
            
            //Create the instruction to move the fingers
            MInstruction moveFingersInstructionsLeft = new MInstruction(System.Guid.NewGuid().ToString(), "Move Fingers", "Pose/MoveFingers")
            {
                Properties = new Dictionary<string, string>()
                {
                    {"Release", "false" },
                    {"Hand", "Left" }
                },
                Constraints = new List<MConstraint>()
            };
            string constraintID = System.Guid.NewGuid().ToString();
            moveFingersInstructionsLeft.Properties.Add("HandPose", constraintID);
            moveFingersInstructionsLeft.Constraints.Add(new MConstraint()
            {
                ID = constraintID,
                PostureConstraint = leftHandPose.GetPostureConstraint()
            });
            
            list.Add(moveFingersInstructionsLeft);
        }

        if (HandChecker.HasRightHand(go))
        {
            //Get UnitySceneAccess ID of hand
            GameObject hand = go.transform.GetChildRecursiveByName("RightHand(Clone)").gameObject;
            String objectID = hand.GetComponent<MMISceneObject>().MSceneObject.ID;
            
            //The desired Hand pose (rotations of the finger Joints)
            UnityHandPose leftHandPose = hand.GetComponent<UnityHandPose>();
            
            //Create the instruction to move the fingers
            MInstruction moveFingersInstructionsRight = new MInstruction(System.Guid.NewGuid().ToString(), "Move Fingers", "Pose/MoveFingers")
            {
                Properties = new Dictionary<string, string>()
                {
                    {"Release", "false" },
                    {"Hand", "Right" }
                },
                Constraints = new List<MConstraint>()
            };
            string constraintID = System.Guid.NewGuid().ToString();
            moveFingersInstructionsRight.Properties.Add("HandPose", constraintID);
            moveFingersInstructionsRight.Constraints.Add(new MConstraint()
            {
                ID = constraintID,
                PostureConstraint = leftHandPose.GetPostureConstraint()
            });
            
            list.Add(moveFingersInstructionsRight);
        }

        return list;
    }

    //Release the Handpose
    private List<MInstruction> ReleaseHandPose()
    {
        List<MInstruction> list = new List<MInstruction>();

        //Create the instruction to move the fingers
        MInstruction moveFingersInstructionsLeft =
            new MInstruction(System.Guid.NewGuid().ToString(), "Release Fingers", "Pose/MoveFingers")
            {
                Properties = new Dictionary<string, string>()
                {
                    {"Release", "true"},
                    {"Hand", "Left"}
                }
            };

        list.Add(moveFingersInstructionsLeft);

        //Create the instruction to move the fingers
        MInstruction moveFingersInstructionsRight =
            new MInstruction(System.Guid.NewGuid().ToString(), "Release Fingers", "Pose/MoveFingers")
            {
                Properties = new Dictionary<string, string>()
                {
                    {"Release", "true"},
                    {"Hand", "Right"}
                },
            };

        list.Add(moveFingersInstructionsRight);
        
        return list;
    }    
    
    public void RunInstruction(List<MInstruction> list)
    {
        MInstruction idleInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Idle", "Pose/Idle");
        this.CoSimulator.MSimulationEventHandler += this.CoSimulator_MSimulationEventHandler;
        /*foreach (var x in list)
        {
            print(x.Name + "   StartBedingung: " + x.StartCondition.ToString());
        }*/
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
                }    
                else
                {
                    list[i].StartCondition = list[i - 1].ID + ":" + mmiConstants.MSimulationEvent_End + "+ 0.1";
                }
            }
        }

        foreach (var x in list)
        {
            CoSimulator.AssignInstruction(idleInstruction,
                new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});
            CoSimulator.AssignInstruction(x,
                new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});
        }
        CoSimulator.AssignInstruction(idleInstruction,new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});
    }




    /// <summary>
    /// Callback for the co-simulation event handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CoSimulator_MSimulationEventHandler(object sender, MSimulationEvent e)
    {
        _tempEvent = e.Type;
        Debug.Log(e.Reference + " " + e.Name + " " + e.Type);
    }


}
