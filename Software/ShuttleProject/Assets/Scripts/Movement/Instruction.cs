using System;
using System.Collections.Generic;
using MMICoSimulation;
using MMICSharp.MMIStandard.Utils;
using MMIStandard;
using MMIUnity;
using MMIUnity.TargetEngine.Scene;
using Scripts;
using TMPro;
using UnityEngine;

namespace Movement
{
    public class Instruction : MonoBehaviour
    {
        /// <summary>
        ///     Manager for carry instruction ids
        /// </summary>
        private readonly CarryIDManager _carryIDManager = new CarryIDManager();
        
        /// <summary>
        ///     Manager for hand pose instruction ids
        /// </summary>
        private readonly HandPoseIdManager _handPoseIdManager = new HandPoseIdManager();
        /// <summary>
        ///     SelectObject
        /// </summary>
        public SelectObject selectObject;
        
        /// <summary>
        ///     TestAvatarBehavior
        /// </summary>
        public TestAvatarBehavior testAvatarBehavior;
        
        /// <summary>
        ///     Controller for queue and scrollView
        /// </summary>
        public QueueController queueController;
        

        /// <summary>
        ///     Walk to selected object
        /// </summary>
        public void WalkTo()
        {
            GameObject go = selectObject.GetObject();
            try
            {
                GameObject walkTarget = go.transform.GetChildRecursiveByName("WalkTarget").gameObject;

                String objectID = walkTarget.GetComponent<MMISceneObject>().MSceneObject.ID;

                MInstruction walkInstruction =
                    new MInstruction(MInstructionFactory.GenerateID(), "Walk", "Locomotion/Walk")
                    {
                        Properties = PropertiesCreator.Create("TargetID", objectID, "ForcePath", "true")
                    };
                
                queueController.AddItem(walkInstruction, "Walk to " + go.name);
            }
            catch (NullReferenceException e)
            {
                SSTools.ShowMessage("No walk target found", SSTools.Position.bottom, SSTools.Time.twoSecond);
            }
        }

        /// <summary>
        ///     Reach selected object
        /// </summary>
        public void ReachObject()
        {
            GameObject go;
            try
            {
                go = selectObject.GetObject();
            }
            catch (NullReferenceException ex)
            {
                SSTools.ShowMessage("No object selected", SSTools.Position.bottom, SSTools.Time.twoSecond);
                return;
            }

            if (!HandChecker.HasHands(go))
            {
                SSTools.ShowMessage("No hands placed", SSTools.Position.bottom, SSTools.Time.twoSecond);
                return;
            }
            
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
            
            queueController.AddItem(list, "Reach " + go.name);
        }

        /// <summary>
        ///     Release selected object
        /// </summary>
        public void Release()
        {
            GameObject go = selectObject.GetObject();
            
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
            
            queueController.AddItem(list, "Release "+ go.name);
        }

        /// <summary>
        ///     Place selected object
        /// </summary>
        public void MoveObject()
        {
            GameObject obj = selectObject.GetObject();
            
            if (!HandChecker.HasHands(obj))
            {
                SSTools.ShowMessage("No hands placed", SSTools.Position.bottom, SSTools.Time.twoSecond);
                return;
            }

            GameObject positionTarget;
            try
            {
                positionTarget = obj.transform.GetChildRecursiveByName("moveTarget").gameObject;
            }
            catch (NullReferenceException ex)
            {
                SSTools.ShowMessage("No move target defined", SSTools.Position.bottom, SSTools.Time.twoSecond);
                return;
            }

            List<MInstruction> list = new List<MInstruction>();
            // List<GameObject> hands = new List<GameObject>();
        
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
            
            queueController.AddItem(list, "Place " + obj.name);
        }
        
        /// <summary>
        ///     Pick up selected object
        /// </summary>
        public void PickUp()
        {
            GameObject obj = selectObject.GetObject();
            
            if (!HandChecker.HasHands(obj))
            {
                SSTools.ShowMessage("No hands placed", SSTools.Position.bottom, SSTools.Time.twoSecond);
                return;
            }
            
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
            
            queueController.AddItem(list, "Pick up " + obj.name);
        }
        
        /// <summary>
        ///     Position fingers on hand pose
        /// </summary>
        private IEnumerable<MInstruction> MakeHandPose(GameObject go, String side)
    {
        List<MInstruction> list = new List<MInstruction>();
        if (side.Equals("Left"))
        {
            //Get UnitySceneAccess ID of hand
            GameObject hand = go.transform.GetChildRecursiveByName("LeftHand(Clone)").gameObject;

            //The desired Hand pose (rotations of the finger Joints)
            UnityHandPose leftHandPose = hand.GetComponent<UnityHandPose>();
            
            _handPoseIdManager.CurrentHandIdLeft = Guid.NewGuid().ToString();
            
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
            var constraintID = Guid.NewGuid().ToString();
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
            
            _handPoseIdManager.CurrentHandIdRight = Guid.NewGuid().ToString();
            
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
            string constraintID = Guid.NewGuid().ToString();
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
        
        /// <summary>
        ///     Release fingers from hand pose
        /// </summary>
        private IEnumerable<MInstruction> ReleaseHandPose(string side)
        {
            List<MInstruction> list = new List<MInstruction>();

            //Create the instruction to move the fingers
        
            MInstruction moveFingersInstructionsLeft =
                new MInstruction(Guid.NewGuid().ToString(), "Release Fingers", "Pose/MoveFingers")
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

        /// <summary>
        ///     Remove last added instruction
        /// </summary>
        public void Undo()
        {
            queueController.RemoveLastItem();
        }

        /// <summary>
        ///     Abort running instructions
        /// </summary>
        public void Abort()
        {    
            testAvatarBehavior.Abort();
            queueController.Clear();
        }
        
        /// <summary>
        ///     Play stored queue
        /// </summary>
        public void Play()
        {
            testAvatarBehavior.RunInstruction(queueController.GETQueue());
            WalkTargetManager.getInstance().GetWalkTarget();
            foreach (var target in WalkTargetManager.getInstance().GetWalkTarget())
            {
                target.transform.localScale = new Vector3(0,0,0);
            }
        }
    }
}