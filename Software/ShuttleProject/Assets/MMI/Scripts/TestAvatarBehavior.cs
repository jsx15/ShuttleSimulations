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
    private GameObject go;
    private Vector3 hitPoint;
    private string carryID;
    private BoxCollider boxColliderLeftHand;
    private BoxCollider boxColliderRightHand;
    private ObjectBounds objectBounds;
    private float OffSetValue = 0.005f;

/*
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
                Properties = PropertiesCreator.Create("TargetID",
                    UnitySceneAccess.Instance.GetSceneObjectByName("WalkTarget").ID)
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
                Properties = new Dictionary<string, string>()
                {
                    //The object that descibres the reach posture
                    {"TargetID", UnitySceneAccess.Instance["GraspTargetR"].ID},
                    //The hand that is used
                    {"Hand", "Left"}
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
            MInstruction idleInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Idle", "idle");

            MInstruction moveObject = new MInstruction(MInstructionFactory.GenerateID(), "move object", "Object/Move")
            {
                Properties = PropertiesCreator.Create("SubjectID", UnitySceneAccess.Instance["GraspObject"].ID, "Hand",
                    "Left", "TargetID", UnitySceneAccess.Instance["PositioningTarget"].ID),
            };

            this.CoSimulator.Abort();
            this.CoSimulator.AssignInstruction(idleInstruction,
                new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});
            this.CoSimulator.AssignInstruction(moveObject,
                new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});
        }


        if (GUI.Button(new Rect(790, 10, 120, 50), "Pick-up"))
        {
            MInstruction idleInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Idle", "idle");

            MInstruction reachInstruction = new MInstruction(MInstructionFactory.GenerateID(), "reach", "Pose/Reach")
            {
                Properties = PropertiesCreator.Create("TargetID", UnitySceneAccess.Instance["RightHand(Clone)"].ID,
                    "Hand",
                    "Right"),
            };

            carryID = MInstructionFactory.GenerateID();
            MInstruction carryInstruction = new MInstruction(carryID, "carry object", "Object/Carry")
            {
                Properties = PropertiesCreator.Create("TargetID", UnitySceneAccess.Instance["Sphere"].ID, "Hand",
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
                    "Left") //, "CarryTarget", UnitySceneAccess.Instance["CarryTarget"].ID),
            };

            this.CoSimulator.AssignInstruction(carryInstruction,
                new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});
        }


        if (GUI.Button(new Rect(10, 70, 220, 50), "Pickup large object"))
        {
            MInstruction walkInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Walk", "walk")
            {
                Properties = PropertiesCreator.Create("TargetID",
                    UnitySceneAccess.Instance.GetSceneObjectByName("WalkTargetLargeObject").ID)
            };

            MInstruction idleInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Idle", "idle")
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
            MInstruction idleInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Idle", "idle");


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

        //Spawn a left hand at the clicked position and set its parent
        if (GUI.Button(new Rect(950, 10, 180, 50), "Place LeftHand"))
        {
            //Acces data from the SelectObject script and change the GameObject's color back to normal 
            try
            {
                go = GameObject.Find("Main Camera").GetComponent<SelectObject>().getObject();
                hitPoint = GameObject.Find("Main Camera").GetComponent<SelectObject>().getHitPoint();
                GameObject.Find("Main Camera").GetComponent<SelectObject>().resetColor();
            }
            catch (Exception)
            {
                SSTools.ShowMessage("No object selected", SSTools.Position.bottom, SSTools.Time.threeSecond);
            }

            if (go != null)
            {
                if (!hasLeftHand(go))
                {
                    //Get max and min values of the selected GameObject
                    objectBounds = new ObjectBounds(go);
                    Vector3 max = objectBounds.getMaxBounds();
                    Vector3 min = objectBounds.getMinBounds();

                    Vector3 offsetLeft = new Vector3();
                    Vector3 rotationLeft = new Vector3();


                    //determine Offset vector and needed rotation in order to let the palm face the object
                    if (hitPoint.x == max.x)
                    {
                        offsetLeft = new Vector3(OffSetValue, 0, 0);
                        rotationLeft = new Vector3(0, 180, 0);
                    }
                    else if (hitPoint.x == min.x)
                    {
                        offsetLeft = new Vector3(-OffSetValue, 0, 0);
                        rotationLeft = new Vector3(0, 0, 0);
                    }
                    else if (hitPoint.y == max.y)
                    {
                        offsetLeft = new Vector3(0, OffSetValue, 0);
                        rotationLeft = new Vector3(0, 0, -90);
                    }
                    else if (hitPoint.y == min.y)
                    {
                        offsetLeft = new Vector3(0, -OffSetValue, 0);
                        rotationLeft = new Vector3(0, 0, 90);
                    }
                    else if (hitPoint.z == max.z)
                    {
                        offsetLeft = new Vector3(0, 0, OffSetValue);
                        rotationLeft = new Vector3(0, 90, 0);
                    }
                    else
                    {
                        offsetLeft = new Vector3(0, 0, -OffSetValue);
                        rotationLeft = new Vector3(0, -90, 0);
                    }


                    //load leftHandPrefab and instantiate it with the predetermined parameters
                    GameObject leftHandPrefab = Resources.Load("LeftHand") as GameObject;
                    GameObject leftHand = Instantiate(leftHandPrefab,
                        new Vector3(hitPoint.x, hitPoint.y, hitPoint.z) + offsetLeft,
                        Quaternion.Euler(rotationLeft)) as GameObject;
                    leftHand.transform.SetParent(go.transform);

                    //Add a BoxCollider to the hand
                    boxColliderLeftHand = leftHand.AddComponent<BoxCollider>();
                    adjustBoxCollider(boxColliderLeftHand, 0);
                }
                else
                {
                    SSTools.ShowMessage("LeftHand already placed", SSTools.Position.bottom, SSTools.Time.threeSecond);
                }
            }
        }

        //Spawn a RightHand at the clicked position and set its parents
        if (GUI.Button(new Rect(920, 70, 180, 50), "Place RightHand"))
        {
            //Acces data from the SelectObject script and change the GameObject's color back to normal
            try
            {
                go = GameObject.Find("Main Camera").GetComponent<SelectObject>().getObject();
                hitPoint = GameObject.Find("Main Camera").GetComponent<SelectObject>().getHitPoint();
                GameObject.Find("Main Camera").GetComponent<SelectObject>().resetColor();
            }
            catch (Exception)
            {
                SSTools.ShowMessage("No object selected", SSTools.Position.bottom, SSTools.Time.threeSecond);
            }

            if (go != null)
            {
                if (!hasRightHand(go))
                {
                    //Get max and min values of the selected GameObject
                    objectBounds = new ObjectBounds(go);
                    Vector3 max = objectBounds.getMaxBounds();
                    Vector3 min = objectBounds.getMinBounds();

                    Vector3 offsetRight = new Vector3();
                    Vector3 rotationRight = new Vector3();

                    //determine Offset vector and needed rotation in order to let the palm face the object
                    if (hitPoint.x == max.x)
                    {
                        offsetRight = new Vector3(OffSetValue, 0, 0);
                        rotationRight = new Vector3(0, 180, 0);
                    }
                    else if (hitPoint.x == min.x)
                    {
                        offsetRight = new Vector3(-OffSetValue, 0, 0);
                        rotationRight = new Vector3(0, 0, 0);
                    }
                    else if (hitPoint.y == max.y)
                    {
                        offsetRight = new Vector3(0, OffSetValue, 0);
                        rotationRight = new Vector3(0, 0, -90);
                    }
                    else if (hitPoint.y == min.y)
                    {
                        offsetRight = new Vector3(0, -OffSetValue, 0);
                        rotationRight = new Vector3(0, 0, 90);
                    }
                    else if (hitPoint.z == max.z)
                    {
                        offsetRight = new Vector3(0, 0, OffSetValue);
                        rotationRight = new Vector3(0, 90, 0);
                    }
                    else
                    {
                        offsetRight = new Vector3(0, 0, -OffSetValue);
                        rotationRight = new Vector3(0, -90, 0);
                    }

                    //load rightHandPrefab and instantiate it with the predetermined parameters
                    GameObject rightHandPrefab = Resources.Load("RightHand") as GameObject;
                    GameObject rightHand = Instantiate(rightHandPrefab,
                        new Vector3(hitPoint.x, hitPoint.y, hitPoint.z) + offsetRight,
                        Quaternion.Euler(rotationRight)) as GameObject;
                    rightHand.transform.SetParent(go.transform);

                    //Add a BoxCollider to the hand
                    boxColliderRightHand = rightHand.AddComponent<BoxCollider>();
                    adjustBoxCollider(boxColliderRightHand, 1);
                }
                else
                {
                    SSTools.ShowMessage("RightHand already placed", SSTools.Position.bottom, SSTools.Time.threeSecond);
                }
            }
        }
    }

    //Give an object a RigidBody
    private void addRigidBody(GameObject obj)
    {
        Rigidbody rigidbody = obj.AddComponent<Rigidbody>();
    }

    //Position the BoxCollider of the hand
    //If position = 0 : LeftHand
    //If position = 1 : RightHand
    private void adjustBoxCollider(BoxCollider boxCollider, int position)
    {
        switch (position)
        {
            case 0:
                boxCollider.size = new Vector3(0.04f, 0.2f, 0.15f);
                boxCollider.center = new Vector3(-0.008f, 0.1f, -0.025f);
                break;
            case 1:
                boxCollider.size = new Vector3(0.04f, 0.2f, 0.15f);
                boxCollider.center = new Vector3(-0.008f, 0.1f, -0.025f);
                break;
        }
    }

    //Check if object has a LeftHand
    private Boolean hasLeftHand(GameObject obj)
    {
        if (obj.transform.GetChildRecursiveByName("LeftHand(Clone)"))
        {
            return true;
        }

        return false;
    }

    //Check if object has a RightHand
    private Boolean hasRightHand(GameObject obj)
    {
        if (obj.transform.GetChildRecursiveByName("RightHand(Clone)"))
        {
            return true;
        }

        return false;
    }
*/
    public MInstruction WalkTo(String s)
    {
        MInstruction walkInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Walk", "walk")
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
        }

        return list;
    }

    public List<MInstruction> ReleaseObject()
    {
        List<MInstruction> list = new List<MInstruction>();

        MInstruction releaseLeft =
            new MInstruction(MInstructionFactory.GenerateID(), "release object", "Object/Release")
            {
                Properties = PropertiesCreator.Create("Hand", "Left", CoSimTopic.OnStart,
                    carryID + ":" + CoSimAction.EndInstruction),
            };

        list.Add(releaseLeft);
        MInstruction releaseRight =
            new MInstruction(MInstructionFactory.GenerateID(), "release object", "Object/Release")
            {
                Properties = PropertiesCreator.Create("Hand", "Right", CoSimTopic.OnStart,
                    carryID + ":" + CoSimAction.EndInstruction),
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
                "Right", "TargetID", targetPositionID)
        };

        list.Add(moveObject);
        return list;
    }

    public List<MInstruction> PickUp(GameObject obj)
    {
        List<MInstruction> list = new List<MInstruction>();
        list.AddRange(ReachObject(obj));

        String objID = obj.GetComponent<MMISceneObject>().MSceneObject.ID;

        carryID = MInstructionFactory.GenerateID();

        if (list.Count == 2)
        {
            MInstruction carryInstruction =
                new MInstruction(MInstructionFactory.GenerateID(), "carry object", "Object/Carry")
                {
                    Properties = PropertiesCreator.Create("TargetID", objID, "Hand",
                        "Both")
                    // StartCondition = inst.ID + ":" + mmiConstants.MSimulationEvent_End + "+ 0.01"
                };
            list.Add(carryInstruction);
        }
        else if (list[0].Name.Equals("reach right"))
        {
            MInstruction carryInstruction =
                new MInstruction(MInstructionFactory.GenerateID(), "carry object", "Object/Carry")
                {
                    Properties = PropertiesCreator.Create("TargetID", objID, "Hand",
                        "Right"),
                    //StartCondition = list[0].ID + ":" + mmiConstants.MSimulationEvent_End + "+ 0.01"
                };
            list.Add(carryInstruction);
        }
        else
        {
            MInstruction carryInstruction =
                new MInstruction(MInstructionFactory.GenerateID(), "carry object", "Object/Carry")
                {
                    Properties = PropertiesCreator.Create("TargetID", objID, "Hand",
                        "Left"),
                    // StartCondition = inst.ID + ":" + mmiConstants.MSimulationEvent_End + "+ 0.01"
                };
            list.Add(carryInstruction);
        }

        return list;
    }

    public void RunInstruction(List<MInstruction> list)
    {
        MInstruction idleInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Idle", "idle");

        for (int i = 0; i < list.Count; i++)
        {
            if (i > 0)
            {
                if (list[i - 1].Name.Equals("carry object"))
                {
                    list[i].StartCondition = list[i - 1].StartCondition + ":" + mmiConstants.MSimulationEvent_End + "+ 0.1";
                }
                else
                {
                    list[i].StartCondition = list[i - 1].ID + ":" + mmiConstants.MSimulationEvent_End + "+ 0.1";
                }

                print(list[i].Name + " hat die StartCondition: " + list[i].StartCondition);
            }

            CoSimulator.AssignInstruction(idleInstruction,
                new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});
            CoSimulator.AssignInstruction(list[i],
                new MSimulationState() {Initial = this.avatar.GetPosture(), Current = this.avatar.GetPosture()});
        }


        CoSimulator.AssignInstruction(idleInstruction, null);
        //this.CoSimulator.MSimulationEventHandler += this.CoSimulator_MSimulationEventHandler;
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