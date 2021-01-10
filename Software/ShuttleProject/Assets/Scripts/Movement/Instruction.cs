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
        ///     Event to signal start of queue
        /// </summary>
        public delegate void Notify();
        public event Notify QueueStart;

        private void Start()
        {
            testAvatarBehavior.QueueFinished += QueueFinishedHandler;
        }

        /// <summary>
        ///     Walk to selected object
        /// </summary>
        public void WalkTo()
        {
            GameObject go;
            GameObject walkTarget;
            try
            {
                //Get selected object
                go = selectObject.GetObject();
            }
            catch (NullReferenceException ex)
            {
                SSTools.ShowMessage("No object selected", SSTools.Position.bottom, SSTools.Time.twoSecond);
                return;
            }

            //If walk target is selected, switch to parent
            if (go.transform.name.Equals("WalkTarget"))
            {
                go = go.transform.parent.gameObject;
            }
            
            try
            {
                //Get walk target of selected object
                //walkTarget = go.transform.GetChildRecursiveByName("WalkTarget").gameObject;
                walkTarget = go.transform.Find("WalkTarget").gameObject;
            }
            catch (NullReferenceException e)
            {
                SSTools.ShowMessage("No walk target found", SSTools.Position.bottom, SSTools.Time.twoSecond);
                return;
            }
            String objectID = walkTarget.GetComponent<MMISceneObject>().MSceneObject.ID;

            //Create instruction
            MInstruction walkInstruction =
                new MInstruction(MInstructionFactory.GenerateID(), "Walk", "Locomotion/Walk")
                {
                    Properties = PropertiesCreator.Create("TargetID", objectID, "ForcePath", "true")
                };
            
            //Add instruction to queue
            queueController.AddItem(walkInstruction, "Walk to " + go.name);

        }

        /// <summary>
        ///     Reach selected object
        /// </summary>
        public void ReachObject()
        {
            GameObject go;
            try
            {
                //Get selected object
                go = selectObject.GetObject();
            }
            catch (NullReferenceException ex)
            {
                SSTools.ShowMessage("No object selected", SSTools.Position.bottom, SSTools.Time.twoSecond);
                return;
            }

            //If move target is selected, switch to parent
            if (MoveTargetChecker.IsMoveTarget(go))
            {
                go = go.transform.parent.gameObject;
            }

            //Check for hands
            if (!HandChecker.HasHands(go))
            {
                SSTools.ShowMessage("No hands placed", SSTools.Position.bottom, SSTools.Time.twoSecond);
                return;
            }
            
            //List of instructions
            List<MInstruction> list = new List<MInstruction>();

            if (HandChecker.HasLeftHand(go))
            {
                //Get UnitySceneAccess ID of hand
                //GameObject hand = go.transform.Find("LeftHand(Clone)").gameObject;
                GameObject hand = HandChecker.GetLeftHand(go);
                String objectID = hand.GetComponent<MMISceneObject>().MSceneObject.ID;

                //Now create a specific instruction to reach with the right hand
                MInstruction reachLeft = new MInstruction(MInstructionFactory.GenerateID(), "reach left", "Pose/Reach")
                {
                    Properties = PropertiesCreator.Create("TargetID", objectID,
                        "Hand", "Left")
                };
          
                //Add instructions to list
                list.Add(reachLeft);
                list.AddRange(MakeHandPose(go, "Left"));
            }

            if (HandChecker.HasRightHand(go))
            {
                //Get UnitySceneAccess ID of hand
                //GameObject hand = go.transform.Find("RightHand(Clone)").gameObject;
                GameObject hand = HandChecker.GetRightHand(go);
                String objectID = hand.GetComponent<MMISceneObject>().MSceneObject.ID;
                //Now create a specific instruction to reach with the right hand
                MInstruction reachRight = new MInstruction(MInstructionFactory.GenerateID(), "reach right", "Pose/Reach")
                {
                    Properties = PropertiesCreator.Create("TargetID", objectID, "Hand", "Right")
                };
            
                //Add instructions to list
                list.Add(reachRight);
                list.AddRange(MakeHandPose(go, "Right"));
            }
            
            //Add instruction to queue
            queueController.AddItem(list, "Reach " + go.name);
        }

        /// <summary>
        ///     Release selected object
        /// </summary>
        public void Release()
        {
            GameObject go;
            try
            {
                //Get selected object
                go = selectObject.GetObject();
            }
            catch (NullReferenceException ex)
            {
                SSTools.ShowMessage("No object selected", SSTools.Position.bottom, SSTools.Time.twoSecond);
                return;
            }
            
            //If move target is selected, switch to parent
            if (MoveTargetChecker.IsMoveTarget(go))
            {
                go = go.transform.parent.gameObject;
            }

            //List for instructions
            List<MInstruction> list = new List<MInstruction>();
            
            //List for hands on object
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
                
                //Add instructions to position fingers
                list.AddRange(ReleaseHandPose("Left"));
                
                //Add instruction to release left hand
                list.Add(releaseLeft);
                
                //Remove left hand game object from object
                //hands.Add(go.transform.Find("LeftHand(Clone)").gameObject);
                hands.Add(HandChecker.GetLeftHand(go));
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
                
                //Add instructions to position fingers
                list.AddRange(ReleaseHandPose("Right"));
                
                //Add instruction to release right hand
                list.Add(releaseRight);
                
                //Remove right hand game object from object
                //hands.Add(go.transform.Find("RightHand(Clone)").gameObject);
                hands.Add(HandChecker.GetRightHand(go));
            }
        
            //Destroy the hands after moving an object
            foreach (var hand in hands)
            {
                Destroy(hand);
            }
            
            //Add instructions to queue
            queueController.AddItem(list, "Release "+ go.name);
        }

        /// <summary>
        ///     Place selected object
        /// </summary>
        public void MoveObject()
        {
            GameObject obj;
            try
            {
                //Get selected object
                obj = selectObject.GetObject();
            }
            catch (NullReferenceException ex)
            {
                SSTools.ShowMessage("No object selected", SSTools.Position.bottom, SSTools.Time.twoSecond);
                return;
            }
            
            //If move target is selected, switch to parent
            if (MoveTargetChecker.IsMoveTarget(obj))
            {
                obj = obj.transform.parent.gameObject;
            }
            
            if (!HandChecker.HasHands(obj))
            {
                SSTools.ShowMessage("No hands placed", SSTools.Position.bottom, SSTools.Time.twoSecond);
                return;
            }

            GameObject positionTarget;
            try
            {
                //Get move target
                positionTarget = obj.transform.Find("moveTarget").gameObject;
            }
            catch (NullReferenceException ex)
            {
                SSTools.ShowMessage("No move target defined", SSTools.Position.bottom, SSTools.Time.twoSecond);
                return;
            }

            //List for instructions
            List<MInstruction> list = new List<MInstruction>();

            //Get IDs of objects
            String objectID = obj.GetComponent<MMISceneObject>().MSceneObject.ID;
            String targetPositionID = positionTarget.GetComponent<MMISceneObject>().MSceneObject.ID;
            
            //Instruction if both hands are present on object
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
            
            //Instruction if only right hand is present on object
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
            
            //Instruction if only left hand is present on object
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
            
            //Add instructions to queue
            queueController.AddItem(list, "Place " + obj.name);
        }
        
        /// <summary>
        ///     Pick up selected object
        /// </summary>
        public void PickUp()
        {
            GameObject obj;
            try
            {
                //Get selected object
                obj = selectObject.GetObject();
            }
            catch (NullReferenceException ex)
            {
                SSTools.ShowMessage("No object selected", SSTools.Position.bottom, SSTools.Time.twoSecond);
                return;
            }
            
            //If move target is selected, switch to parent
            if (MoveTargetChecker.IsMoveTarget(obj))
            {
                obj = obj.transform.parent.gameObject;
            }
            
            if (!HandChecker.HasHands(obj))
            {
                SSTools.ShowMessage("No hands placed", SSTools.Position.bottom, SSTools.Time.twoSecond);
                return;
            }
            
            List<MInstruction> list = new List<MInstruction>();

            //Get IDs of object
            String objID = obj.GetComponent<MMISceneObject>().MSceneObject.ID;
        
            //Instructions if both hands are present on object
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
            
            //Instructions if only right hand is present on object
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
            
            //Instructions if only left hand is present on object
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
            
            //Add instructions to queue
            queueController.AddItem(list, "Pick up " + obj.name);
        }
        
        /// <summary>
        ///     Position fingers on hand pose
        /// </summary>
        private IEnumerable<MInstruction> MakeHandPose(GameObject go, string side)
        {
            List<MInstruction> list = new List<MInstruction>();
            if (side.Equals("Left"))
            {
                //Get UnitySceneAccess ID of hand
                //GameObject hand = go.transform.Find("LeftHand(Clone)").gameObject;
                GameObject hand = HandChecker.GetLeftHand(go);

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
                
                //Add properties to the instruction
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
                //GameObject hand = go.transform.Find("RightHand(Clone)").gameObject;
                GameObject hand = HandChecker.GetRightHand(go);
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
                
                //Add properties to the instruction
                var constraintID = Guid.NewGuid().ToString();
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

            //Create the instruction to release the fingers
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
            QueueStart?.Invoke();
            WalkTargetManager.getInstance().MinWalkTargets();
            
        }

        /// <summary>
        ///     Clear queue and scale walk targets
        /// </summary>
        private void QueueFinishedHandler()
        {
            queueController.Clear();
            WalkTargetManager.getInstance().MaxWalkTargets();
        }
    }
}