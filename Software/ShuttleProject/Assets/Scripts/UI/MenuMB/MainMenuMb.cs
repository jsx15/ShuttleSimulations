using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class MainMenuMb : MonoBehaviour, Menu
    {
        /*
        * Canvas for GUI
        */
        private GameObject _canvas;
        /*
         * Showing menu
         */
        private bool _showing;

        public void Start()
        {
            _canvas = GameObject.Find("Canvas");
        }

        public void RemoveMenu()
        {
        }

        public void ShowMenu()
        {
            _canvas = GameObject.Find("Canvas");
            var mainMenuButton = Instantiate(Resources.Load("UI/Button"), _canvas.transform) as GameObject;
            if (mainMenuButton is null) return;
            mainMenuButton.transform.position = new Vector3(MenuManager.WidthDistance(Screen.width), MenuManager.HeightDistance(Screen.height, 9));
            mainMenuButton.GetComponentInChildren<Text>().text = "Main menu";
            mainMenuButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (_showing)
                {
                    MenuManager.CloseAllMenues();
                    _showing = false;
                    return;
                    
                }

                MenuManager.ShowAddObjectMenu();
                MenuManager.ShowPlaceHandMenu();
                MenuManager.ShowWalkTo();
                MenuManager.ShowQueue();
                _showing = true;
            });

            //Add quit button
            var quitButton = Instantiate(Resources.Load("UI/Button"), _canvas.transform) as GameObject;
            if (quitButton is null) return;
            quitButton.transform.position = new Vector3(MenuManager.WidthDistance(Screen.width, "right"), MenuManager.HeightDistance(Screen.height, 1));
            quitButton.GetComponentInChildren<Text>().text = "Quit";
            // Click listener
            quitButton.GetComponent<Button>().onClick.AddListener(() => { Application.Quit(0); });
        }
        
    }
}