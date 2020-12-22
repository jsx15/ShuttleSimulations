using MMIUnity.TargetEngine.Scene;
using UnityEngine;

namespace Scripts
{
    public class ManageObject : MonoBehaviour
    {
        /// <summary>
        ///     Name of selected prefab
        /// </summary>
        [HideInInspector]
        public string selectedPrefab;

        /// <summary>
        ///     Check for mouse click
        /// </summary>
        public void Update()
        {
            // Call AddObject left mouse click and prefab selected
            if (Input.GetButtonDown("Fire1") && !selectedPrefab.Equals(""))
                AddObject(Input.mousePosition);
        }
        

        /// <summary>
        ///     Instantiate clicked object
        /// </summary>
        /// <param name="mousePosition">Position of the mouse</param>
        private void AddObject(Vector2 mousePosition)
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            var hit = RayFromCamera(mousePosition, 1000.0f);
            //Return if no collider found
            if (hit.point.normalized == new Vector3(0, 0, 0))
            {
                return;
            }
    
            //Instantiate selected prefab
            if (Instantiate(Resources.Load(selectedPrefab), hit.point, Quaternion.identity) 
                is GameObject clickedObject)
            {
                //Set position
                var position = clickedObject.transform.position;
                position = new Vector3(position.x,
                    // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                    position.y + clickedObject.GetComponent<MeshRenderer>().bounds.size.y / 2,
                    position.z);
                clickedObject.transform.position = position;

                //Set scene as parent
                clickedObject.transform.parent = transform;
            }
            
			//Reset 
            selectedPrefab = "";
            

        }
        
        
        /// <summary>
        ///     Get next collider hit
        /// </summary>
        /// <param name="mousePosition">Position of the mouse</param>
        /// <param name="rayLength">Distance to observe</param>
        /// <returns>hit point of collider</returns>
        private RaycastHit RayFromCamera(Vector3 mousePosition, float rayLength)
        {
            var direction = Camera.main.ScreenPointToRay(mousePosition);
            Physics.Raycast(direction, out var hit, rayLength);
            return hit;
        }
    }
}