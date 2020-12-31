using System;
using System.Collections.Generic;
using MMICoSimulation;
using MMICSharp.MMIStandard.Utils;
using MMIStandard;
using MMIUnity;
using MMIUnity.TargetEngine.Scene;
using Scripts;
using UnityEngine;

namespace Movement
{
    public class Instruction : MonoBehaviour
    {
        /// <summary>
        ///     Manager for carry instruction ids
        /// </summary>
        private CarryIDManager _carryIDManager = new CarryIDManager();
        
        /// <summary>
        ///     Manager for hand pose instruction ids
        /// </summary>
        private HandPoseIdManager _handPoseIdManager = new HandPoseIdManager();
        /// <summary>
        ///     SelectObject
        /// </summary>
        public SelectObject _selectObject;
        
        /// <summary>
        ///     TestAvatarBehavior
        /// </summary>
        public TestAvatarBehavior testAvatarBehavior;

        public GameObject ScrollView;
        
        /// <summary>
        ///     List of all instructions
        /// </summary>
        private List<MInstruction> _mInstructions = new List<MInstruction>();
        

        public void WalkTo()
        {
            GameObject go = _selectObject.GetObject();
            GameObject walkTarget = go.transform.GetChildRecursiveByName("WalkTarget").gameObject;

            String objectID = walkTarget.GetComponent<MMISceneObject>().MSceneObject.ID;

            MInstruction walkInstruction = new MInstruction(MInstructionFactory.GenerateID(), "Walk", "Locomotion/Walk")
            {
                Properties = PropertiesCreator.Create("TargetID", objectID, "ForcePath", "true")
            };
            
            _mInstructions.Add(walkInstruction);
        }

        public void ReachObject()
        {
            GameObject go = _selectObject.GetObject();
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
            
            _mInstructions.AddRange(list);
        }

        public void Release()
        {
            GameObject go = _selectObject.GetObject();
            
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
            
            _mInstructions.AddRange(list);
        }

        public void MoveObject()
        {
            GameObject obj = _selectObject.GetObject();
            GameObject positionTarget = obj.transform.GetChildRecursiveByName("moveTarget").gameObject;
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
            
            _mInstructions.AddRange(list);
        }

        public void PickUp()
        {
            GameObject obj = _selectObject.GetObject();
            List<MInstruction> list = new List<MInstruction>();

            String objID = obj.GetComponent<MMISceneObject>().MSceneObject.ID;
        
            if (HandChecker.HasBothHands(obj))
            {
                _carryIDManager.CurrentCarryIdBoth = MInstructionFactory.GenerateID();
                MInstruction carryInstruction =
                    new MInstruction(_carryIDManager.CurrentCarryIdBoth, "carry object", "Object/Carry")
                    {
                        Properties = PropertiesCreator.Create("TargetID", objID, "Hand",
                            "Both")
                    };
                list.Add(carryInstruction);
            }
            else if (HandChecker.HasRightHand(obj))
            {
                _carryIDManager.CurrentCarryIdRight = MInstructionFactory.GenerateID();
                MInstruction carryInstruction = 
                    new MInstruction(_carryIDManager.CurrentCarryIdRight, "carry object", "Object/Carry")
                    {
                        Properties = PropertiesCreator.Create("TargetID", objID, "Hand",
                            "Right"),
                    };
                list.Add(carryInstruction);
            }
            else if (HandChecker.HasLeftHand(obj))
            {
                _carryIDManager.CurrentCarryIdLeft = MInstructionFactory.GenerateID();
                MInstruction carryInstruction =
                    new MInstruction(_carryIDManager.CurrentCarryIdLeft, "carry object", "Object/Carry")
                    {
                        Properties = PropertiesCreator.Create("TargetID", objID, "Hand",
                            "Left"),
                    };
                list.Add(carryInstruction);
            }
            
            _mInstructions.AddRange(list);
        }
        
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
        
        
        public void Play()
        {
            testAvatarBehavior.RunInstruction(_mInstructions);
            WalkTargetManager.getInstance().GetWalkTarget();
            foreach (var target in WalkTargetManager.getInstance().GetWalkTarget())
            {
                target.transform.localScale = new Vector3(0,0,0);
            }
        }
    }
}