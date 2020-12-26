using UnityEngine;

namespace Scripts
{
    public class EscMenu : MonoBehaviour
    {
        /// <summary>
        ///     The boolean to represent status
        /// </summary>
        private bool _open = false;

        /// <summary>
        ///     The menu canvas
        /// </summary>
        public GameObject menuCanvas;

        /// <summary>
        ///     Check for esc key pressed
        /// </summary>
        public void Update()
        {
            if (!Input.GetKey(KeyCode.Escape)) return;
            _open = true;
            menuCanvas.SetActive(true);
        }

        /// <summary>
        ///    Exit the application
        /// </summary>
        public void Quit()
        {
            Application.Quit();
        }

        /// <summary>
        ///     Hide menu canvas -> return to application
        /// </summary>
        public void Continue()
        {
            _open = false;
            menuCanvas.SetActive(false);
        }
        
    }
}