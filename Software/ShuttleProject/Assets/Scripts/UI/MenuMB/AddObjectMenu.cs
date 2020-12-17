﻿using System;
using System.Collections.Generic;
using MMIUnity;
using UI.MenuMB;
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
        private GameObject _parent;
        private GameObject _oldGameObject;
        private GameObject _createWalkTargetButton;

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
            var x = MenuManager.SubMenuStart();
            
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
            Destroy(_createWalkTargetButton);
            ClearButtons();
        }

        /*
         * Toggles remove button and Create Target button
         */
        public void ObjectSelected(GameObject go)
        {
            _canvas = GameObject.Find("Canvas");
            if (go is null) 
            {
                Destroy(_removeButton);
                Destroy(_createTargetButton);
                Destroy(_createWalkTargetButton);
                _createTargetButton = null;
                _removeButton = null;
                _createWalkTargetButton = null;
                return;
            }    
            if (go != _oldGameObject)
            {
                Destroy(_removeButton);
                Destroy(_createTargetButton);
                Destroy(_createWalkTargetButton);
                _createTargetButton = null;
                _removeButton = null;
                _createWalkTargetButton = null;
            }

            //Hands shouldn't have a walktarget or a movetarget
            if (!go.name.Equals("LeftHand(Clone)") && !go.name.Equals("RightHand(Clone)"))
            {
                if ((_createTargetButton is null || _oldGameObject != go || go == _oldGameObject) && !go.transform.Find("moveTarget"))
                {
                    Destroy(_createTargetButton);
                    _createTargetButton = Instantiate(Resources.Load("UI/Button"), _canvas.transform) as GameObject;
                    if (_createTargetButton is null) return;
                    _createTargetButton.transform.position = new Vector3(MenuManager.WidthDistance(Screen.width) + 150,
                        MenuManager.HeightDistance(Screen.height, 1));
                    _createTargetButton.GetComponentInChildren<Text>().text = "Create Target";
                    _buttonList.Add(_createTargetButton);
                    _createTargetButton.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        GameObject target = Instantiate(go, go.transform.position, go.transform.rotation);
                        target.transform.parent = go.transform;
                        target.name = "moveTarget";
                        Material material = (Material) Resources.Load("Materials/targetMaterial", typeof(Material));
                        target.GetComponent<Renderer>().material = material;
                        target.AddComponent<HoldPos>();
                        ObjectBounds _bounds = new ObjectBounds(go.transform.gameObject);
                        float size = _bounds.GetMaxBounds().x - _bounds.GetMinBounds().x;
                        Vector3 newPos = new Vector3(go.transform.position.x + size + 0.25f * size,
                            go.transform.position.y, go.transform.position.z);
                        target.transform.position = newPos;
                        if (target.transform.GetChildRecursiveByName("RightHand(Clone)") ||
                            target.transform.GetChildRecursiveByName("LeftHand(Clone)") ||
                            target.transform.GetChildRecursiveByName("WalkTarget"))
                        {
                            foreach (Transform child in target.transform)
                            {
                                if (child.name.Equals("RightHand(Clone)") || child.name.Equals("LeftHand(Clone)") ||
                                    child.name.Equals("WalkTarget"))
                                {
                                    Destroy(child.gameObject);
                                }
                            }
                        }

                        Destroy(_createTargetButton);
                    });
                    _buttonList.Add(_createTargetButton);
                }

                if ((_createWalkTargetButton is null || _oldGameObject != go || go == _oldGameObject) &&
                    !go.transform.Find("WalkTarget"))
                {
                    Destroy(_createWalkTargetButton);
                    _createWalkTargetButton = Instantiate(Resources.Load("UI/Button"), _canvas.transform) as GameObject;
                    if (_createWalkTargetButton is null) return;
                    _createWalkTargetButton.transform.position = new Vector3(
                        MenuManager.WidthDistance(Screen.width) + 300, MenuManager.HeightDistance(Screen.height, 1));
                    _createWalkTargetButton.GetComponentInChildren<Text>().text = "Create Walktarget";
                    _buttonList.Add(_createWalkTargetButton);
                    _createWalkTargetButton.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        GameObject target =
                            Instantiate(Resources.Load("Utility/WalkTarget"), go.transform) as GameObject;
                        target.name = "WalkTarget";
                        WalkTargetManager.getInstance().AddWalkTarget(target);
                        ObjectBounds _bounds = new ObjectBounds(go.transform.gameObject);
                        float size = _bounds.GetMaxBounds().x - _bounds.GetMinBounds().x;
                        Vector3 newPos = new Vector3(go.transform.position.x - size - 0.15f * size, 0.025f,
                            go.transform.position.z);
                        target.transform.position = newPos;
                        target.transform.Rotate(0, -90, 0, Space.World);
                        Destroy(_createWalkTargetButton);
                    });
                    _buttonList.Add(_createWalkTargetButton);
                }
            }


            if (_removeButton is null || _oldGameObject != go || go == _oldGameObject)
            {
                Destroy(_removeButton);
                // Add remove object button
                _removeButton = Instantiate(Resources.Load("UI/Button"), _canvas.transform) as GameObject;
                if (_removeButton is null) return;
                _removeButton.transform.position = new Vector3(MenuManager.WidthDistance(Screen.width), MenuManager.HeightDistance(Screen.height, 1));
                _removeButton.GetComponentInChildren<Text>().text = "Remove";
                _removeButton.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (HandChecker.IsHand(go)) _parent = go.transform.parent.gameObject;
                    GameObject.Find("Main Camera").GetComponent<SelectObject>().ResetColor();
                    Destroy(go);
                    if (_parent != null && GameObject.Find("Scene").GetComponent<PlaceHandsMenu>().GetRigidBody())
                    {
                        if (!(HandChecker.HasLeftHand(_parent) && HandChecker.HasRightHand(_parent)))
                        {
                            _parent.AddComponent<Rigidbody>();
                        }
                    }
                    Destroy(_removeButton);
                });
            }
            _oldGameObject = go;
            
        }
    }
}