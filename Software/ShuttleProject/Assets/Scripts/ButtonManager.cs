using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Skripte
{
    public class ButtonManager : MonoBehaviour
    {
        /*
         * Canvas for GUI
         */
        private GameObject _canvas;
        /*
         * List for all buttons
         */
        private readonly List<GameObject> _buttonList = new List<GameObject>();
        
        private void Start()
        {
            _canvas = GameObject.Find("Canvas");
            
            //Add object button
            var addObjectButton = Instantiate(Resources.Load("UI/Button"), _canvas.transform) as GameObject;
            if (!(addObjectButton is null))
            {
                addObjectButton.transform.position = new Vector3(Screen.width/20f, (Screen.height / 10) * 7);
                addObjectButton.GetComponentInChildren<Text>().text = "Add object";
                // Click listener
                addObjectButton.GetComponent<Button>().onClick.AddListener(AddObjectClicked);
            }

            //Add quit button
            var quitButton = Instantiate(Resources.Load("UI/Button"), _canvas.transform) as GameObject;
            if (quitButton is null) return;
            quitButton.transform.position = new Vector3((Screen.width / 20) * 18, (Screen.height / 10) * 1);
            quitButton.GetComponentInChildren<Text>().text = "Quit";
            // Click listener
            quitButton.GetComponent<Button>().onClick.AddListener(() => { Application.Quit(0); });
        }
        /*
         * Button addObject clicked
         */
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
                button.transform.position = new Vector3(Screen.width / 20 + x, (Screen.height / 10) * 7);
                x += 150;
                
                //Extract prefab name
                var tmp = file.LastIndexOf("\\", StringComparison.Ordinal);
                var prefabeName = file.Substring(tmp + 1);
                prefabeName = prefabeName.Split('.')[0];
                button.GetComponentInChildren<Text>().text = prefabeName;
                GameObject scene = GameObject.Find("Scene");
                
                //Click listener for prefab button
                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    scene.GetComponent<SpawnObject>().selectedPrefab = prefabeName;
                    scene.GetComponent<SpawnObject>().addObjectPressed = true;
                });
                _buttonList.Add(button);
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
        
    }
}