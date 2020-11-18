using MMIUnity.TargetEngine.Scene;
using UnityEngine;

namespace Scripts
{
    public class ManageObject : MonoBehaviour
    {
        [HideInInspector]
        public bool addObjectPressed;
        [HideInInspector]
        public bool addPrefabPressed;
        [HideInInspector]
        public string selectedPrefab;

        public void Update()
        {
            
            if (Input.GetButtonDown("Fire1") && addPrefabPressed)
                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                AddObject(Input.mousePosition);
        }
        

        /*
         * Instantiate clicked object
         */
        public void AddObject(Vector2 mousePosition)
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            var hit = RayFromCamera(mousePosition, 1000.0f);
            //Return if no collider found
            if (hit.point.normalized == new Vector3(0, 0, 0))
            {
                return;
            }
    
            if (Instantiate(Resources.Load(selectedPrefab), hit.point, Quaternion.identity) is GameObject clickedObject)
            {
                //Align position
                var position = clickedObject.transform.position;
                position = new Vector3(position.x,
                    // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                    position.y + clickedObject.GetComponent<MeshRenderer>().bounds.size.y / 2,
                    position.z);
                clickedObject.transform.position = position;
                
                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                //Add rigidbody
                clickedObject.AddComponent<Rigidbody>();
                
                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                clickedObject.AddComponent<MMISceneObject>();
                
                //Set scene as parent
                clickedObject.transform.parent = transform;
            }
            addPrefabPressed = false;
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            GetComponent<AddObjectMenu>().ClearButtons();

        }
        
        
        /*
         * Get next collider hit
         */
        private RaycastHit RayFromCamera(Vector3 mousePosition, float rayLength)
        {
            var direction = Camera.main.ScreenPointToRay(mousePosition);
            Physics.Raycast(direction, out var hit, rayLength);
            return hit;
        }
    }
}