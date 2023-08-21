using UnityEngine;

namespace MMS.Menu
{
    public class MenuManager : MonoBehaviour                       
    {
        [SerializeField] private MenuHandler[] menus;
        [SerializeField] private MenuHandler loadingMenu;
        [SerializeField] private MenuHandler roomMenu;
        [SerializeField] private MenuHandler createRoomMenu;
        [SerializeField] private MenuHandler findRoomMenu;
        [SerializeField] private MenuHandler controlPanel;

        public void OpenLoadingMenu()
        {
            CloseAllMenus();
            loadingMenu.Open();
        }

        private void CloseAllMenus()
        {
            for (int i = 0; i < menus.Length; i++)
            {
                menus[i].Close();
            }
        }

        public void OpenMainMenu()
        {
            CloseAllMenus();   
            controlPanel.Open();
        }

        public void OpenRoomMenu()
        {
            CloseAllMenus();
            roomMenu.Open();
        }

        public void OpenCreateMenu()
        {
            CloseAllMenus();
            createRoomMenu.Open();
        }

        public void OpenFindRoomMenu()
        {
            CloseAllMenus();
            findRoomMenu.Open();
        }
    }
}