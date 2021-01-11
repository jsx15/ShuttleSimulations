using UnityEngine;

namespace Scripts
{
    public class AddObjectButtons : MonoBehaviour
    {
        /// <summary>
        ///     The manage object script
        /// </summary>
        public ManageObject manageObject;
        
        /// <summary>
        ///     Click listener for cube button
        /// </summary>
        public void AddCube()
        {
            manageObject.selectedPrefab = "Cube";
        }
        
        /// <summary>
        ///     Click listener for capsule button
        /// </summary>
        public void AddCapsule()
        {
            manageObject.selectedPrefab = "Capsule";
        }

        public void AddWheel()
        {
            manageObject.selectedPrefab = "Bus/Reifen";
        }

        public void AddFrontDoor()
        {
            manageObject.selectedPrefab = "Bus/VorderTuer";
        }
    }
}