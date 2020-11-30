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
        private GameObject _createTargetButton;

        public void Start()
        {
        }


        public void ShowMenu()
        {
            _canvas = GameObject.Find("Canvas");
            
            //Add object button
            _addObjectButton = Instantiate(Resources.Load("UI/Button"), _canvas.transform) as GameObject;
            if (!(_addObjectButton is null))
            {
                _addObjectButton.transform.position = new Vector3(MenuManager.WidthDistance(Screen.width), MenuManager.HeightDistance(Screen.height, 8));
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
            Destroy(_createTargetButton);
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
                return;
            }
            // Add remove object button
            _removeButton = Instantiate(Resources.Load("UI/Button"), _canvas.transform) as GameObject;
            if (_removeButton is null) return;
            _removeButton.transform.position = new Vector3((Screen.width / 20f), (Screen.height / 10) * 1);
            _removeButton.GetComponentInChildren<Text>().text = "Remove";
            _removeButton.GetComponent<Button>().onClick.AddListener(() =>
                {
                    Destroy(go);
                    Destroy(_removeButton);
                });
            
            _createTargetButton = Instantiate(Resources.Load("UI/Button"), _canvas.transform) as GameObject;
            if (_createTargetButton is null) return;
            _createTargetButton.transform.position = new Vector3((Screen.width / 20f) + 150, (Screen.height / 10) * 1);
            _createTargetButton.GetComponentInChildren<Text>().text = "Create Target";
            _createTargetButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                GameObject target = Instantiate(go, go.transform.position, go.transform.rotation);
                target.transform.parent = go.transform;
                target.name = "moveTarget";
                Material material = (Material) Resources.Load("Materials/targetMaterial",typeof(Material));
                target.GetComponent<Renderer>().material = material;
                target.AddComponent<HoldPos>();
            });

        }
        
    }
}