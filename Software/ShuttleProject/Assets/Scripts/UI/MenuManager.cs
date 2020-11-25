using System;
using System.Collections.Generic;
using UI.MenuMB;
using UnityEngine;

namespace Scripts
{
    public static class MenuManager
    {
        private static AddObjectMenu _addObjectMenu;
        private static PlaceHandsMenu _placeHandsMenu;
        private static WalkToMenu _walkTo;

        private static List<Menu> _menuList = new List<Menu>()
        {
            _addObjectMenu, _placeHandsMenu, _walkTo
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
    }
}