using System;
using System.Collections.Generic;
using MMIUnity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class AddObjectMenu : MonoBehaviour
    {
        /// <summary>
        ///    The object selected bar
        /// </summary>
        public GameObject objectSelectedBar;

        /// <summary>
        ///     The object selected buttons
        /// </summary>
        public GameObject removeButton, createWalkTargetButton, createTargetButton;
        
        /// <summary>
        ///     Add object panel for buttons
        /// </summary>
        public GameObject addObjectPanel;
        
        /// <summary>
        ///    Reference for selectObject script
        /// </summary>
        private SelectObject _selectObject;
        
        /// <summary>
        ///     Parent object
        /// </summary>
        private GameObject _parent;
        
        /// <summary>
        ///     Create prefab buttons and click listener for object selected bar
        /// </summary>
        public void Start()
        {
            //Get reference
            _selectObject = GameObject.Find("Main Camera").GetComponent<SelectObject>();
            
            // Create button for every prefab in resource folder
            var files = PrefabLoader.GETPrefab();
            foreach (var file in files)
            {
                //Instantiate button
                var button = Instantiate(Resources.Load("UI/Button"), addObjectPanel.transform) as GameObject;
                
                //Set button text
                var index = file.LastIndexOf("\\", StringComparison.Ordinal);
                var prefabeName = file.Substring(index + 1);
                prefabeName = prefabeName.Split('.')[0];
                button.transform.GetChildRecursiveByName("Text").GetComponent<TextMeshProUGUI>().text = prefabeName;
                
                //Add  button onClick listener
                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    //Set prefabName in ManageObject
                    GameObject scene = GameObject.Find("Scene");
                    scene.GetComponent<ManageObject>().selectedPrefab = prefabeName;
                });
            }
            
            //Add click listener
            createWalkTargetButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                //Instantiate walk target
                GameObject target = Instantiate(Resources.Load("Utility/WalkTarget"), _selectObject.GetObject().transform) as GameObject;
                target.name = "WalkTarget";
                WalkTargetManager.getInstance().AddWalkTarget(target);
                
                //Set bounds and position
                ObjectBounds _bounds = new ObjectBounds(_selectObject.GetObject().transform.gameObject);
                float size = _bounds.GetMaxBounds().x - _bounds.GetMinBounds().x;
                Vector3 newPos = new Vector3(_selectObject.GetObject().transform.position.x - size - 0.15f*size, 0.025f, _selectObject.GetObject().transform.position.z);
                target.transform.position = newPos;
                
                //Hide button
                createWalkTargetButton.SetActive(false);
            });
            
            //Add click listener
            createTargetButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                GameObject go = _selectObject.GetObject();
                GameObject target = Instantiate(go, go.transform.position, go.transform.rotation);
                target.transform.parent = go.transform;
                target.name = "moveTarget";
                target.GetComponent<Renderer>().material = (Material) Resources.Load("Materials/targetMaterial",typeof(Material));

                //Add script
                target.AddComponent<HoldPos>();
                
                //Set bounds and position
                ObjectBounds bounds = new ObjectBounds(go.transform.gameObject);
                float size = bounds.GetMaxBounds().x - bounds.GetMinBounds().x;
                Vector3 newPos = new Vector3(go.transform.position.x + size + 0.25f*size, go.transform.position.y, go.transform.position.z);
                target.transform.position = newPos;
                
                //Remove hands and walk targets from move target 
                if (target.transform.GetChildRecursiveByName("RightHand(Clone)") || target.transform.GetChildRecursiveByName("LeftHand(Clone)") || target.transform.GetChildRecursiveByName("WalkTarget"))
                {
                    foreach (Transform child in target.transform)
                    {
                        if (child.name.Equals("RightHand(Clone)") || child.name.Equals("LeftHand(Clone)") || child.name.Equals("WalkTarget"))
                        {
                            Destroy(child.gameObject);
                        }
                    }
                }
                    
                //Hide button
                createTargetButton.SetActive(false);
            });
        }
        
        /// <summary>
        ///     Show all object selected buttons
        /// </summary>
        private void ShowButtons()
        {
            //SetActive for every button
            foreach (Transform child in objectSelectedBar.transform)
            {
                child.gameObject.SetActive(true);
            }
            //SetActive for bar
            objectSelectedBar.SetActive(true);
        }

        /// <summary>
        ///     Toggles remove button and Create Target button
        /// </summary>
        /// <param name="go">Selected game object</param>
        public void ObjectSelected(GameObject go)
        {
            // If no object is selected (e.g. click on background)
            if (go is null) 
            {
                objectSelectedBar.SetActive(false);
                return;
            }
            
            // Show all buttons in bar
            ShowButtons();
            
            //Check if moveTarget has not been already created
            if (_selectObject.GetObject().transform.Find("moveTarget"))
            {
                createTargetButton.SetActive(false);
            }
            
            //Hide button if walk target already created
            if (_selectObject.GetObject().transform.Find("WalkTarget"))
            {
                createWalkTargetButton.SetActive(false);
            }

            // Add click listener on remove button
            removeButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (HandChecker.IsHand(go)) _parent = go.transform.parent.gameObject;
                GameObject.Find("Main Camera").GetComponent<SelectObject>().ResetColor();
                Destroy(go);
                /*
                if (_parent != null )//&& GameObject.Find("Scene").GetComponent<PlaceHandsMenu>().GetRigidBody())
                {
                    if (!(HandChecker.HasLeftHand(_parent) && HandChecker.HasRightHand(_parent)))
                    {
                        _parent.AddComponent<Rigidbody>();
                    }
                }
                */
                
                //Hide button
                removeButton.SetActive(false);
            });

        }
    }
}