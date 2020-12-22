using System;
using System.Collections.Generic;
using MMICoSimulation;
using MMICSharp.MMIStandard.Utils;
using MMIStandard;
using MMIUnity;
using MMIUnity.TargetEngine;
using MMIUnity.TargetEngine.Scene;
using Scripts;
using TMPro;
using UnityEngine;


public class TestAvatarBehavior : AvatarBehavior
{
    private Vector3 hitPoint;
    private string carryIDR, carryIDL, carryIDB;
    private BoxCollider boxColliderLeftHand, boxColliderRightHand;
    private ObjectBounds objectBounds;
    private float OffSetValue = 0.005f;
    private CarryIDManager _carryIDManager = new CarryIDManager();
    private string tmp;
    private HandPoseIdManager _handPoseIdManager = new HandPoseIdManager();
    
    
    
    
    


    protected override void GUIBehaviorInput()
    {
        //base.GUIBehaviorInput();
    }

    
    public MInstruction WalkTo(GameObject go)

    {
        GameObject walkTarget = go.transform.GetChildRecursiveByName("WalkTarget").gameObject;

        String objectID = walkTarget.GetComponent<MMISceneObject>().MSceneObject.ID;

        MInstruction walkInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Walk", "Locomotion/Walk")
        {
            Properties = PropertiesCreator.Create("TargetID", objectID, "ForcePath", "true")
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
          
            list.Add(reachLeft);
            list.AddRange(MakeHandPose(go, "Left"));
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
            
            list.Add(reachRight);
            list.AddRange(MakeHandPose(go, "Right"));
        }
        //list.AddRange(MakeHandPose(go));
        return list;
    }

    public List<MInstruction> ReleaseObject(GameObject go)
    {
        List<MInstruction> list = new List<MInstruction>();
        List<GameObject> hands = new List<GameObject>();

        if (HandChecker.HasLeftHand(go))
        {
            MInstruction releaseLeft =
                new MInstruction(MInstructionFactory.GenerateID(), "release object", "Object/Release")
                {
                    Properties = PropertiesCreator.Create("Hand", "Left", CoSimTopic.OnStart,
                     _handPoseIdManager.CurrentHandIdLeft + ":" + CoSimAction.EndInstruction)
                    //Properties = PropertiesCreator.Create("Hand", "Left")
                };
            list.AddRange(ReleaseHandPose("Left"));
            list.Add(releaseLeft);
            hands.Add(go.transform.GetChildRecursiveByName("LeftHand(Clone)").gameObject);
        }


        if (HandChecker.HasRightHand(go))
        {
            MInstruction releaseRight =
                new MInstruction(MInstructionFactory.GenerateID(), "release object", "Object/Release")
                {
                    Properties = PropertiesCreator.Create("Hand", "Right", CoSimTopic.OnStart,
                     _handPoseIdManager.CurrentHandIdRight + ":" + CoSimAction.EndInstruction)
                    //Properties = PropertiesCreator.Create("Hand", "Right")
                };
            list.AddRange(ReleaseHandPose("Right"));
            list.Add(releaseRight);
            hands.Add(go.transform.GetChildRecursiveByName("RightHand(Clone)").gameObject);
        }
        
        //Destroy the hands after moving an object
        foreach (var hand in hands)
        {
            Destroy(hand);
        }
        
        return list;
    }

    public List<MInstruction> MoveObject(GameObject obj, GameObject positionTarget)
    {
        List<MInstruction> list = new List<MInstruction>();
        List<GameObject> hands = new List<GameObject>();
        
        String objectID = obj.GetComponent<MMISceneObject>().MSceneObject.ID;
        String targetPositionID = positionTarget.GetComponent<MMISceneObject>().MSceneObject.ID;
        if (HandChecker.HasBothHands(obj))
        {
            MInstruction moveObject = new MInstruction(MInstructionFactory.GenerateID(), "move object", "Object/Move")
            {
                Properties = PropertiesCreator.Create("SubjectID", objectID, "Hand",
                    "Both" , "TargetID", targetPositionID, CoSimTopic.OnStart, 
                    _carryIDManager.CurrentCarryIdBoth + ":" + CoSimAction.EndInstruction)
            };
            list.Add(moveObject);
        }
        else if (HandChecker.HasRightHand(obj))
        {
            MInstruction moveObject = new MInstruction(MInstructionFactory.GenerateID(), "move object", "Object/Move")
            {
                Properties = PropertiesCreator.Create("SubjectID", objectID, "Hand",
                    "Right" , "TargetID", targetPositionID, CoSimTopic.OnStart, 
                    _carryIDManager.CurrentCarryIdRight + ":" + CoSimAction.EndInstruction)
            };
            list.Add(moveObject);
        }
        else if (HandChecker.HasLeftHand(obj))
        {
            MInstruction moveObject = new MInstruction(MInstructionFactory.GenerateID(), "move object", "Object/Move")
            {
                Properties = PropertiesCreator.Create("SubjectID", objectID, "Hand",
                    "Left", "TargetID", targetPositionID, CoSimTopic.OnStart, 
                    _carryIDManager.CurrentCarryIdLeft + ":" + CoSimAction.EndInstruction)
            };
            list.Add(moveObject);
        }
        
        return list;
    }
    
    public List<MInstruction> PickUp(GameObject obj)
    {
        List<MInstruction> list = new List<MInstruction>();

        String objID = obj.GetComponent<MMISceneObject>().MSceneObject.ID;
        
        if (HandChecker.HasBothHands(obj))
        {
            carryIDB = MInstructionFactory.GenerateID();
            _carryIDManager.CurrentCarryIdBoth = carryIDB;
            MInstruction carryInstruction =
                new MInstruction(carryIDB, "carry object", "Object/Carry")
                {
                    Properties = PropertiesCreator.Create("TargetID", objID, "Hand",
                        "Both")
                };
            list.Add(carryInstruction);
        }
        else if (HandChecker.HasRightHand(obj))
        {
            carryIDR = MInstructionFactory.GenerateID();
            _carryIDManager.CurrentCarryIdRight = carryIDR;
            MInstruction carryInstruction = 
                new MInstruction(carryIDR, "carry object", "Object/Carry")
            {
                Properties = PropertiesCreator.Create("TargetID", objID, "Hand",
                    "Right"),
            };
            list.Add(carryInstruction);
        }
        else if (HandChecker.HasLeftHand(obj))
        {
            carryIDL = MInstructionFactory.GenerateID();
            _carryIDManager.CurrentCarryIdLeft = carryIDL;
            MInstruction carryInstruction =
                new MInstruction(carryIDL, "carry object", "Object/Carry")
                {
                    Properties = PropertiesCreator.Create("TargetID", objID, "Hand",
                        "Left"),
                };
            list.Add(carryInstruction);
        }
        
        return list;
    }

    //Define a HandPose for the given object
    private List<MInstruction> MakeHandPose(GameObject go, String side)
    {
        List<MInstruction> list = new List<MInstruction>();
        if (side.Equals("Left"))
        {
            //Get UnitySceneAccess ID of hand
            GameObject hand = go.transform.GetChildRecursiveByName("LeftHand(Clone)").gameObject;
            //String objectID = hand.GetComponent<MMISceneObject>().MSceneObject.ID;
            
            //The desired Hand pose (rotations of the finger Joints)
            UnityHandPose leftHandPose = hand.GetComponent<UnityHandPose>();
            
            _handPoseIdManager.CurrentHandIdLeft = System.Guid.NewGuid().ToString();
            
            //Create the instruction to move the fingers

            MInstruction moveFingersInstructionsLeft = new MInstruction(_handPoseIdManager.CurrentHandIdLeft, "Move Fingers", "Pose/MoveFingers")
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

        if (side.Equals("Right"))
        {
            //Get UnitySceneAccess ID of hand
            GameObject hand = go.transform.GetChildRecursiveByName("RightHand(Clone)").gameObject;
            //String objectID = hand.GetComponent<MMISceneObject>().MSceneObject.ID;
            
            //The desired Hand pose (rotations of the finger Joints)
            UnityHandPose leftHandPose = hand.GetComponent<UnityHandPose>();
            
            _handPoseIdManager.CurrentHandIdRight = System.Guid.NewGuid().ToString();
            
            //Create the instruction to move the fingers

            MInstruction moveFingersInstructionsRight = new MInstruction(_handPoseIdManager.CurrentHandIdRight, "Move Fingers", "Pose/MoveFingers")
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
    private List<MInstruction> ReleaseHandPose(string side)
    {
        List<MInstruction> list = new List<MInstruction>();

        //Create the instruction to move the fingers
        
        MInstruction moveFingersInstructionsLeft =
            new MInstruction(System.Guid.NewGuid().ToString(), "Release Fingers", "Pose/MoveFingers")
            {
                Properties = new Dictionary<string, string>()
                {
                    {"Release", "true"},
                    {"Hand", side}
                }
            };

        list.Add(moveFingersInstructionsLeft);
        return list;
    }    
    
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
        // Debug.Log(sender.ToString());
        if (e.Reference.Equals(tmp))
        {
            foreach (var x in WalkTargetManager.getInstance().GetWalkTarget())
            {
                x.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }
        }
    }


}
