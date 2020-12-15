using System;
using System.Collections.Generic;
using MMIStandard;
using MMIUnity;
using Scripts;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace UI.MenuMB
{
    public class Queue: MonoBehaviour, Menu
    {
        /*
        * Canvas for GUI
        */
        private GameObject _canvas;
        /*
         * List for all buttons
         */
        private readonly List<GameObject> _buttonList = new List<GameObject>();
        /*
         * 
         */
        private GameObject _queue, _walk, _move, _reach, _pickUp, _walkSphere, _release;
        private SelectObject _selectObject;

        private bool _menuShowing;
        
        private List<MInstruction> _mInstructions = new List<MInstruction>();
        private TestAvatarBehavior beh;
        private Transform scrollView;

        public void Start()
        {
            _selectObject = GameObject.Find("Main Camera").GetComponent<SelectObject>();
            
        }

        public void RemoveMenu()
        {
            Destroy(_queue);
            Destroy(_walk);
            Destroy(_move);
            Destroy(_reach);
            Destroy(_pickUp);
            ClearButtons();
        }

        public void ShowMenu()
        {
            scrollView = GameObject.Find("Content").transform;
            beh = GameObject.Find("Avatar").GetComponent<TestAvatarBehavior>();
            _menuShowing = false;
            _canvas = GameObject.Find("Canvas");
            _queue = Instantiate(Resources.Load("UI/Button"), _canvas.transform) as GameObject;
            if (_queue is null) return;
            _queue.transform.position = new Vector3(MenuManager.WidthDistance(Screen.width), MenuManager.HeightDistance(Screen.height, 5));
            _queue.GetComponentInChildren<Text>().text = "Create Queue";
            _queue.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (_menuShowing)
                {
                    ClearButtons();
                    _menuShowing = false;
                    return;
                }
                ShowQueueButton();
                ShowPlayButton();
                _menuShowing = true;
            });
        }

        private void ShowPlayButton()
        {
            GameObject play = Instantiate(Resources.Load("UI/Button"), _canvas.transform) as GameObject;
            play.transform.position = new Vector3(MenuManager.WidthDistance(Screen.width, "right"), MenuManager.HeightDistance(Screen.height, 2));
            play.GetComponentInChildren<Text>().text = "Play";
            play.GetComponent<Button>().onClick.AddListener(() =>
            {
                List<GameObject> walkTargets = WalkTargetManager.getInstance().GetWalkTarget();
                foreach (var x in walkTargets)
                {
                    x.transform.localScale = new Vector3(0, 0, 0);
                }
                ClearButtons();
                beh.RunInstruction(_mInstructions);
                MenuManager.CloseAllMenues();
                _mInstructions.Clear();
                foreach (Transform child in scrollView) {
                    GameObject.Destroy(child.gameObject);
                }
                
            });
            _buttonList.Add(play);
        }

        private void ShowQueueButton()
        {
            var x = MenuManager.SubMenuStart();
            _walk = Instantiate(Resources.Load("UI/Button"), _canvas.transform) as GameObject;
            if (!(_walk is null))
            {
                _walk.transform.position = new Vector3(Screen.width / 20f + x, (Screen.height / 10f) * 5f);
                x += 150;
                _walk.GetComponentInChildren<Text>().text = "Walk";
                _walk.GetComponent<Button>().onClick.AddListener(() =>
                {
                    try
                    {
                        GameObject walkTarget =
                            _selectObject.GetObject().transform.GetChildRecursiveByName("WalkTarget").gameObject;
                        _mInstructions.Add(beh.WalkTo(walkTarget));
                        addToList("Walk to " + _selectObject.GetObject().name);
                    }
                    catch
                    {
                        SSTools.ShowMessage("Walktarget not found", SSTools.Position.bottom, SSTools.Time.twoSecond);
                    }
                });
                _buttonList.Add(_walk);
            }



            _move = Instantiate(Resources.Load("UI/Button"), _canvas.transform) as GameObject;
            if (!(_move is null))
            {
                _move.transform.position = new Vector3(Screen.width / 20f + x, (Screen.height / 10f) * 5f);
                x += 150;
                _move.GetComponentInChildren<Text>().text = "Place object";
                _move.GetComponent<Button>().onClick.AddListener(() =>
                {
                    try
                    {
                        _mInstructions.AddRange(beh.MoveObject(_selectObject.GetObject(),
                            _selectObject.GetObject().transform.GetChildRecursiveByName("moveTarget").gameObject));
                        addToList("Move " + _selectObject.GetObject().name);
                    }
                    catch (Exception ex)
                    {
                        SSTools.ShowMessage("Move target not defined", SSTools.Position.bottom, SSTools.Time.twoSecond);
                    }
                    
                });
                _buttonList.Add(_move);
            }

            _reach = Instantiate(Resources.Load("UI/Button"), _canvas.transform) as GameObject;
            if (!(_reach is null))
            {
                _reach.transform.position = new Vector3(Screen.width / 20f + x, (Screen.height / 10f) * 5f);
                x += 150;
                _reach.GetComponentInChildren<Text>().text = "Reach";
                _reach.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (HandChecker.HasLeftHand(_selectObject.GetObject()) ||
                        HandChecker.HasRightHand(_selectObject.GetObject()))
                    {
                        _mInstructions.AddRange(beh.ReachObject(_selectObject.GetObject()));
                        addToList("Reach " + _selectObject.GetObject().name); 
                    }
                    else
                    {
                        SSTools.ShowMessage("Place hands first", SSTools.Position.bottom, SSTools.Time.twoSecond);
                    }
                    
                });
                _buttonList.Add(_reach);
            }

            _pickUp = Instantiate(Resources.Load("UI/Button"), _canvas.transform) as GameObject;
            if (_pickUp is null) return;
            _pickUp.transform.position = new Vector3(Screen.width / 20f + x, (Screen.height / 10f) * 5f);
            x += 150;
            _pickUp.GetComponentInChildren<Text>().text = "Pick Up";
            _pickUp.GetComponent<Button>().onClick.AddListener(() =>
            {
                _mInstructions.AddRange(beh.PickUp(_selectObject.GetObject()));
                addToList("Pick Up object");
            });
            _buttonList.Add(_pickUp);
            
            _release = Instantiate(Resources.Load("UI/Button"), _canvas.transform) as GameObject;
            if (_release is null) return;
            _release.transform.position = new Vector3(Screen.width / 20f + MenuManager.SubMenuStart(), (Screen.height / 10f) * 4f);
            // x += 150;
            _release.GetComponentInChildren<Text>().text = "Release";
            _release.GetComponent<Button>().onClick.AddListener(() =>
            {
                GameObject go = _selectObject.GetObject();
                if (go != null)
                {
                    _mInstructions.AddRange(beh.ReleaseObject(go));
                    addToList("Release");
                }
                else
                {
                    SSTools.ShowMessage("Select Object",SSTools.Position.bottom, SSTools.Time.twoSecond);
                }
            });
            _buttonList.Add(_release);
        }
        
        private void ClearButtons()
        {
            foreach (var button in _buttonList)
            {
                Destroy(button);
            }
            _buttonList.Clear();
        }

        private void addToList(string instruction)
        {
            GameObject textItem = Instantiate(Resources.Load("UI/Text"), GameObject.Find("Content").transform) as GameObject;
            textItem.GetComponent<Text>().text = instruction;

        }
    }
}