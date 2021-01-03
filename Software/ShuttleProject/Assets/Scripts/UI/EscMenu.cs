using System;
using System.Threading;
using UnityEngine;

namespace Scripts
{
    public class EscMenu : MonoBehaviour
    {
        /// <summary>
        ///     The main menu canvas
        /// </summary>
        public GameObject escMenu;

        /// <summary>
        ///     The help menu canvas
        /// </summary>
        public GameObject helpMenu;

        /// <summary>
        ///    The background
        /// </summary>
        public GameObject background;

        /// <summary>
        ///     Stores current state
        /// </summary>
        private State _currentState = State.Running;

        /// <summary>
        ///     Block input
        /// </summary>
        private bool _block;

        /// <summary>
        ///     Check for esc key pressed
        /// </summary>
        public void Update()
        {
            //Check if input is blocked
            if (_block) return;
            
            //Check if esc key is pressed
            if (Input.GetKey(KeyCode.Escape))
            {
                //Start block input
                new Thread(BlockInput).Start();
                
                //Check current state
                switch (_currentState)
                {
                    //Show esc menu
                    case State.Running:
                        ActivateState(State.Esc);
                        break;
                    
                    //Exit esc menu
                    case State.Esc:
                        ExitState();
                        break;
                    
                    //Exit help menu
                    case State.Help:
                        ExitState();
                        break;
                }
                
            }

        }

        /// <summary>
        ///     Click listener for help button
        /// </summary>
        public void HelpButtonPressed()
        {
            if (_currentState is State.Running)
            {
                ActivateState(State.Help);
                Thread t1 = new Thread(BlockInput);
                t1.Start();
            }

            else if (_currentState is State.Help)
            {
                ExitState();
            }
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
            ExitState();
            
        }

        /// <summary>
        ///     Show state
        /// </summary>
        /// <param name="state">State to show</param>
        private void ActivateState(State state)
        {
            background.SetActive(true);
            switch (state)
            {
                case State.Esc:
                    escMenu.SetActive(true);
                    break;
                case State.Help:
                    helpMenu.SetActive(true);
                    break;
            }
            _currentState = state;
        }

        /// <summary>
        ///     Return to running state
        /// </summary>
        private void ExitState()
        {
            background.SetActive(false);
            switch (_currentState)
            {
                case State.Esc:
                    escMenu.SetActive(false);
                    break;
                case State.Help:
                    helpMenu.SetActive(false);
                    break;
            }
            _currentState = State.Running;

        }
        
        private enum State
        {
            Running, Help, Esc
        }

        private void BlockInput()
        {
            _block = true;
            Thread.Sleep(100);
            _block = false;
        } 
        
        
    }
}