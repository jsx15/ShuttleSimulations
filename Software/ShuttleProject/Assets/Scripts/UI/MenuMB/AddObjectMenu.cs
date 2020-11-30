using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class AddObjectMenu : MonoBehaviour, Menu
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
        private GameObject _removeButton;
        private GameObject _addObjectButton;
        private GameObject _parent;

        public void ShowMenu()
        {
            _canvas = GameObject.Find("Canvas");
            
            //Add object button
            _addObjectButton = Instantiate(Resources.Load("UI/Button"), _canvas.transform) as GameObject;
            if (!(_addObjectButton is null))
            {
                _addObjectButton.transform.position = new Vector3(Screen.width/20f, (Screen.height / 10f) * 8f);
                _addObjectButton.GetComponentInChildren<Text>().text = "Add object";
                // Click listener
                
                _addObjectButton.GetComponent<Button>().onClick.AddListener(() =>
                {
                    GameObject scene = GameObject.Find("Scene");
                    if (scene.GetComponent<ManageObject>().addObjectPressed)
                    {
                        ClearButtons();
                        scene.GetComponent<ManageObject>().addObjectPressed = false;
                        return;
                    }
                    scene.GetComponent<ManageObject>().addObjectPressed = true;
                    AddObjectClicked();
                });
            }
        }
        private void AddObjectClicked()
        {
            var loader = new PrefabLoader();
            var files = loader.getPrefab();
            //Gap between buttons
            var x = 200;
            
            //Create button for every .prefab file
            foreach (var file in files)
            {
                var button = Instantiate(Resources.Load("UI/Button"), _canvas.transform) as GameObject;

                if (button is null) continue;
                button.transform.position = new Vector3(Screen.width / 20 + x, (Screen.height / 10) * 8);
                x += 150;
                
                //Extract prefab name
                var tmp = file.LastIndexOf("\\", StringComparison.Ordinal);
                var prefabeName = file.Substring(tmp + 1);
                prefabeName = prefabeName.Split('.')[0];
                button.GetComponentInChildren<Text>().text = prefabeName;
                GameObject scene = GameObject.Find("Scene");
                _buttonList.Add(button);
                
                //Click listener for prefab button
                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (scene.GetComponent<ManageObject>().addPrefabPressed)
                    {
                        ClearButtons();
                        return;
                    }
                    
                    scene.GetComponent<ManageObject>().selectedPrefab = prefabeName;
                    scene.GetComponent<ManageObject>().addPrefabPressed = true;
                    scene.GetComponent<ManageObject>().addObjectPressed = false;
                });
            }
        }
        
        /**
         * Remove all buttons from GUI and list
         */
        public void ClearButtons()
        {
            foreach (var button in _buttonList)
            {
                Destroy(button);
            }
            _buttonList.Clear();
        }


        public void RemoveMenu()
        {
            GameObject scene = GameObject.Find("Scene");
            scene.GetComponent<ManageObject>().addPrefabPressed = false;
            scene.GetComponent<ManageObject>().addObjectPressed = false;
            Destroy(_addObjectButton);
            Destroy(_removeButton);
            ClearButtons();
        }

        /*
         * Toggles remove button
         */
        public void ObjectSelected(GameObject go)
        {
            _canvas = GameObject.Find("Canvas");
            if (go is null)
            {
                Destroy(_removeButton);
                _removeButton = null;
                return;
            }

            if (_removeButton is null)
            {
                // Add remove object button
                _removeButton = Instantiate(Resources.Load("UI/Button"), _canvas.transform) as GameObject;
                if (_removeButton is null) return;
                _removeButton.transform.position = new Vector3((Screen.width / 20f), (Screen.height / 10) * 6);
                _removeButton.GetComponentInChildren<Text>().text = "Remove";
                _removeButton.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (HandChecker.IsHand(go)) _parent = go.transform.parent.gameObject;
                    Destroy(go);
                    if (_parent != null)
                    {
                        if (!(HandChecker.HasLeftHand(_parent) && HandChecker.HasRightHand(_parent)))
                        {
                            _parent.AddComponent<Rigidbody>();
                        }
                    }

                    Destroy(_removeButton);
                });
            }
        }
    }
}