using UnityEngine;

namespace Scripts
{
    public static class MenuManager
    {
        private static AddObjectMenu _addObjectMenu;

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
        }
    }
}