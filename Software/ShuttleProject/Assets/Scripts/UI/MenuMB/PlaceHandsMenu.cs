using System;
using System.Collections.Generic;
using System.IO;
using Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MenuMB
{
    public class PlaceHandsMenu  : MonoBehaviour, Menu
    {
        /*
        * Canvas for GUI
        */
        private GameObject _canvas;
        /*
         * Button 
         */
        private GameObject _placeHandsButton;
        private GameObject _placeLeftHandButton;
        private GameObject _placeRightHandButton;
        private GameObject _placeLeftHandPinchButton;
        private GameObject _placeRightHandPinchButton;
        private bool _menuShowing;

        private List<GameObject> _buttonList = new List<GameObject>();
        
        /*
         * Necessary game object variables
         */
        private GameObject _go;
        private Vector3 _hitPoint;
        private string _carryID;
        private BoxCollider _boxColliderLeftHand;
        private BoxCollider _boxColliderRightHand;
        private ObjectBounds _objectBounds;
        private const float OffSetValue = 0.005f;


        public void RemoveMenu()
        {
            Destroy(_placeHandsButton);
            foreach (var button in _buttonList)
            {
                Destroy(button);
            }
            _buttonList.Clear();
        }

        public void ShowMenu()
        {
            _canvas = GameObject.Find("Canvas");
            
            _placeHandsButton = Instantiate(Resources.Load("UI/Button"), _canvas.transform) as GameObject;
            if (!(_placeHandsButton is null))
            {
                _placeHandsButton.transform.position = new Vector3(Screen.width/20f, (Screen.height / 10f) * 7f);
                _placeHandsButton.GetComponentInChildren<Text>().text = "Place Hand";
                _menuShowing = false;
                
                _placeHandsButton.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (_menuShowing)
                    {
                        foreach (var button in _buttonList)
                        {
                            Destroy(button);
                        }
                        _buttonList.Clear();
                        _menuShowing = false;
                        return;
                    }
                    CreateSubMenu();
                    _menuShowing = true;

                });
                
            }
            
        }

        private void CreateSubMenu()
        {
            var x = 200;
            _placeLeftHandButton = Instantiate(Resources.Load("UI/Button"), _canvas.transform) as GameObject;
            if (!(_placeLeftHandButton is null))
            {
                _placeLeftHandButton.GetComponentInChildren<Text>().text = "Left Hand";
                _placeLeftHandButton.transform.position = new Vector3(Screen.width / 20 + x, (Screen.height / 10) * 7);
                _placeLeftHandButton.GetComponent<Button>().onClick.AddListener(() =>
                {
                    //Access data from the SelectObject script and change the GameObject's color back to normal 
                    try
                    {
                        _go = GameObject.Find("Main Camera").GetComponent<SelectObject>().getObject();
                        _hitPoint = GameObject.Find("Main Camera").GetComponent<SelectObject>().getHitPoint();
                        GameObject.Find("Main Camera").GetComponent<SelectObject>().resetColor();
                    }
                    catch (Exception)
                    {
                        SSTools.ShowMessage("No object selected", SSTools.Position.bottom, SSTools.Time.threeSecond);
                    }

                    if (_go != null)
                    {
                        if (!HandChecker.HasLeftHand(_go))
                        {
                            //Get max and min values of the selected GameObject
                            _objectBounds = new ObjectBounds(_go);
                            Vector3 max = _objectBounds.getMaxBounds();
                            Vector3 min = _objectBounds.getMinBounds();

                            Vector3 offsetLeft;
                            Vector3 rotationLeft;


                            //determine Offset vector and needed rotation in order to let the palm face the object
                            if (_hitPoint.x == max.x)
                            {
                                offsetLeft = new Vector3(OffSetValue, 0, 0);
                                rotationLeft = new Vector3(0, 180, 0);
                            }
                            else if (_hitPoint.x == min.x)
                            {
                                offsetLeft = new Vector3(-OffSetValue, 0, 0);
                                rotationLeft = new Vector3(0, 0, 0);
                            }
                            else if (_hitPoint.y == max.y)
                            {
                                offsetLeft = new Vector3(0, OffSetValue, 0);
                                rotationLeft = new Vector3(0, 0, -90);
                            }
                            else if (_hitPoint.y == min.y)
                            {
                                offsetLeft = new Vector3(0, -OffSetValue, 0);
                                rotationLeft = new Vector3(0, 0, 90);
                            }
                            else if (_hitPoint.z == max.z)
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
                            GameObject leftHandPrefab =
                                Resources.Load("HandPrefab" + Path.DirectorySeparatorChar + "LeftHand") as GameObject;
                            GameObject leftHand = Instantiate(leftHandPrefab,
                                new Vector3(_hitPoint.x, _hitPoint.y, _hitPoint.z) + offsetLeft,
                                Quaternion.Euler(rotationLeft));
                            leftHand.transform.SetParent(_go.transform);

                            //Add a BoxCollider to the hand
                            _boxColliderLeftHand = leftHand.AddComponent<BoxCollider>();
                            adjustBoxCollider(_boxColliderLeftHand, 0);
                        }
                        else
                        {
                            SSTools.ShowMessage("LeftHand already placed", SSTools.Position.bottom,
                                SSTools.Time.threeSecond);
                        }
                    }
                });
                x += 150;
                _placeRightHandButton = Instantiate(Resources.Load("UI/Button"), _canvas.transform) as GameObject;
                if (!(_placeRightHandButton is null))
                {
                    _placeRightHandButton.GetComponentInChildren<Text>().text = "Right Hand";
                    _placeRightHandButton.transform.position =
                        new Vector3(Screen.width / 20 + x, (Screen.height / 10) * 7);
                    _placeRightHandButton.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        //Access data from the SelectObject script and change the GameObject's color back to normal
                        try
                        {
                            _go = GameObject.Find("Main Camera").GetComponent<SelectObject>().getObject();
                            _hitPoint = GameObject.Find("Main Camera").GetComponent<SelectObject>().getHitPoint();
                            GameObject.Find("Main Camera").GetComponent<SelectObject>().resetColor();
                        }
                        catch (Exception)
                        {
                            SSTools.ShowMessage("No object selected", SSTools.Position.bottom,
                                SSTools.Time.threeSecond);
                        }

                        if (_go != null)
                        {
                            if (!HandChecker.HasRightHand(_go))
                            {
                                //Get max and min values of the selected GameObject
                                _objectBounds = new ObjectBounds(_go);
                                Vector3 max = _objectBounds.getMaxBounds();
                                Vector3 min = _objectBounds.getMinBounds();

                                Vector3 offsetRight;
                                Vector3 rotationRight;

                                //determine Offset vector and needed rotation in order to let the palm face the object
                                if (_hitPoint.x == max.x)
                                {
                                    offsetRight = new Vector3(OffSetValue, 0, 0);
                                    rotationRight = new Vector3(0, 180, 0);
                                }
                                else if (_hitPoint.x == min.x)
                                {
                                    offsetRight = new Vector3(-OffSetValue, 0, 0);
                                    rotationRight = new Vector3(0, 0, 0);
                                }
                                else if (_hitPoint.y == max.y)
                                {
                                    offsetRight = new Vector3(0, OffSetValue, 0);
                                    rotationRight = new Vector3(0, 0, -90);
                                }
                                else if (_hitPoint.y == min.y)
                                {
                                    offsetRight = new Vector3(0, -OffSetValue, 0);
                                    rotationRight = new Vector3(0, 0, 90);
                                }
                                else if (_hitPoint.z == max.z)
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
                                GameObject rightHandPrefab =
                                    Resources.Load("HandPrefab" + Path.DirectorySeparatorChar + "RightHand") as
                                        GameObject;
                                GameObject rightHand = Instantiate(rightHandPrefab,
                                    new Vector3(_hitPoint.x, _hitPoint.y, _hitPoint.z) + offsetRight,
                                    Quaternion.Euler(rotationRight));
                                rightHand.transform.SetParent(_go.transform);

                                //Add a BoxCollider to the hand
                                _boxColliderRightHand = rightHand.AddComponent<BoxCollider>();
                                adjustBoxCollider(_boxColliderRightHand, 1);
                            }
                            else
                            {
                                SSTools.ShowMessage("RightHand already placed", SSTools.Position.bottom,
                                    SSTools.Time.threeSecond);
                            }
                        }
                    });
                }


                _buttonList.Add(_placeLeftHandButton);
            }

            _buttonList.Add(_placeRightHandButton);
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
        
        
    }
}