using System.Collections.Generic;
using MMIStandard;
using MMIUnity;
using Scripts;
using UnityEngine;
using UnityEngine.UI;

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

        private bool _menuShowing;
        
        private List<MInstruction> _mInstructions = new List<MInstruction>();
        private TestAvatarBehavior beh;
        
        
        
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
                ClearButtons();
                beh.RunInstruction(_mInstructions);
                MenuManager.CloseAllMenues();
            });
            _buttonList.Add(play);
        }

        private void ShowQueueButton()
        {
            var x = 200;
            _walk = Instantiate(Resources.Load("UI/Button"), _canvas.transform) as GameObject;
            if (!(_walk is null))
            {
                _walk.transform.position = new Vector3(Screen.width / 20f + x, (Screen.height / 10f) * 5f);
                x += 150;
                _walk.GetComponentInChildren<Text>().text = "Walk";
                _walk.GetComponent<Button>().onClick.AddListener(() =>
                {
                    _mInstructions.Add(beh.WalkTo("WalkTarget"));
                });
                _buttonList.Add(_walk);
            }
            
            _walkSphere = Instantiate(Resources.Load("UI/Button"), _canvas.transform) as GameObject;
            if (!(_walkSphere is null))
            {
                _walkSphere.transform.position = new Vector3(Screen.width / 20f + x, (Screen.height / 10f) * 5f);
                x += 150;
                _walkSphere.GetComponentInChildren<Text>().text = "Walk Sphere";
                _walkSphere.GetComponent<Button>().onClick.AddListener(() =>
                {
                    _mInstructions.Add(beh.WalkTo("WalkTargetSphere"));
                });
                _buttonList.Add(_walkSphere);
            }


            _move = Instantiate(Resources.Load("UI/Button"), _canvas.transform) as GameObject;
            if (!(_move is null))
            {
                _move.transform.position = new Vector3(Screen.width / 20f + x, (Screen.height / 10f) * 5f);
                x += 150;
                _move.GetComponentInChildren<Text>().text = "Move";
                _move.GetComponent<Button>().onClick.AddListener(() =>
                {
                    _mInstructions.AddRange(beh.MoveObject(GameObject.Find("Sphere"), GameObject.Find("Sphere").transform.GetChildRecursiveByName("moveTarget").gameObject));
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
                    _mInstructions.AddRange(beh.ReachObject(GameObject.Find("Sphere")));
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
                _mInstructions.AddRange(beh.PickUp(GameObject.Find("Sphere")));
            });
            _buttonList.Add(_pickUp);
            
            _release = Instantiate(Resources.Load("UI/Button"), _canvas.transform) as GameObject;
            if (_release is null) return;
            _release.transform.position = new Vector3(Screen.width / 20f + x, (Screen.height / 10f) * 5f);
            // x += 150;
            _release.GetComponentInChildren<Text>().text = "Release";
            _release.GetComponent<Button>().onClick.AddListener(() =>
            {
                _mInstructions.AddRange(beh.ReleaseObject());
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
    }
}