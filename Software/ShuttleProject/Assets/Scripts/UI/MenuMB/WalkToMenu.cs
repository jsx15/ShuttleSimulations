using System;
using Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MenuMB
{

    public class WalkToMenu : MonoBehaviour, Menu
    {
        /*
        * Button 
        */
        private GameObject _walkTo;

        /*
        * Canvas for GUI
        */
        private GameObject _canvas;
        

        public void RemoveMenu()
        {
            Destroy(_walkTo);
        }

        public void ShowMenu()
        {
            _canvas = GameObject.Find("Canvas");
            _walkTo = Instantiate(Resources.Load("UI/Button"), _canvas.transform) as GameObject;
            if (_walkTo is null) return;
            _walkTo.GetComponentInChildren<Text>().text = "Walk to";
            _walkTo.transform.position = new Vector3(MenuManager.WidthDistance(Screen.width), MenuManager.HeightDistance(Screen.height, 6));
            _walkTo.GetComponent<Button>().onClick.AddListener(() =>
            {
                SSTools.ShowMessage("Select object to walk to", SSTools.Position.bottom,
                    SSTools.Time.threeSecond);
                //TestAvatarBehavior script = GameObject.Find("Avatar").GetComponent<TestAvatarBehavior>();
                //script.POPO();
            });
        }


        public class WalkToHandler
        {
            private WalkToMenu _walkToMenu;
            
            public void ObjectSelectedHandler(object sender, EventArgs e, string s)
            {
                //TestAvatarBehavior script = GameObject.Find("Avatar").GetComponent<TestAvatarBehavior>();
                //script.POPO();
            }
        }
    }

}