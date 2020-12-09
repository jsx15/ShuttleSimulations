using System;
using System.Collections.Generic;
using UI.MenuMB;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    /*
     *
     * 10   
     * 9     Main Menu
     * 8     Add Object
     * 7     Place Hand
     * 6     Walk to
     * 5     Create Queue
     * 4
     * 3
     * 2                                     Play
     * 1                                     Quit
     *
     * 
     */
    public static class MenuManager
    {
        private static AddObjectMenu _addObjectMenu;
        private static PlaceHandsMenu _placeHandsMenu;
        private static WalkToMenu _walkTo;
        private static Queue _queue;

        private static List<Menu> _menuList = new List<Menu>()
        {
            _addObjectMenu, _placeHandsMenu, _walkTo, _queue
        };
        

        public static void ShowMainMenu()
        {
            MainMenuMb menuMb = GameObject.Find("Canvas").GetComponent<MainMenuMb>();
            menuMb.ShowMenu();
        }

        public static void ShowAddObjectMenu()
        {
            _addObjectMenu = GameObject.Find("Scene").GetComponent<AddObjectMenu>();
            _addObjectMenu.ShowMenu();
        }
        
        public static void CloseAllMenues()
        {
            _addObjectMenu.RemoveMenu();
            _placeHandsMenu.RemoveMenu();
            _walkTo.RemoveMenu();
            _queue.RemoveMenu();
        }

        public static void ShowPlaceHandMenu()
        {
            _placeHandsMenu = GameObject.Find("Scene").GetComponent<PlaceHandsMenu>();
            _placeHandsMenu.ShowMenu();
        }

        public static void ShowWalkTo()
        {
            _walkTo = GameObject.Find("Scene").GetComponent<WalkToMenu>();
            _walkTo.ShowMenu();
        }

        public static void ShowQueue()
        {
            _queue = GameObject.Find("Scene").GetComponent<Queue>();
            _queue.ShowMenu();
        }

        public static float WidthDistance(float width)
        {
            return width / 25f * 2;
        }

        public static float WidthDistance(float width, String side)
        {
            if (side.Equals("right"))
            {
                return width/ 25 * 22;    
            }

            return 0.0f;

        }

        public static float HeightDistance(float height, int prio)
        {
            return height / 10 * prio;
        }

        public static float SubMenuStart()
        {
            return Screen.width / 25 * 5;
        }
    }
}