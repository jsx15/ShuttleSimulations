using UnityEditor.Playables;
using UnityEngine;

    public class DragAndRotate
    {
        //The GameObject
        private GameObject go;

        //drag variables
        private Vector3 mOffset;
        private float mZCoord;
        private bool lockY;
        private float yPos;

        //rotate variables
        private bool gravity;
        private bool collider;
        private float speed = 5.0F;
        private float x;
        private float y;
        private float z;
        private Vector3 pos;
        
        //classes
        private ShowAxis showAxis;


        //objectBounds object to get max and min values of x,y & z
        private ObjectBounds objectBounds;

        public DragAndRotate(GameObject go, bool lockY)
        {
            this.go = go;
            this.lockY = lockY;

            //safe settings
            safeGravityAndCollider();

            //set Offset for Drag
            setOffset();
            
            showAxis = new ShowAxis(go);
        }

        //-------------------rotate-------------------

        public void handleRotate()
        {
            if (Input.GetKey(KeyCode.X))
            {

                showAxis.showX();
                //disable gravity and collider
                disableGravityAndCollider();

                //get mouse position on x axis
                x = speed * Input.GetAxis("Mouse X");

                //rotate object
                go.transform.Rotate(x, 0, 0, Space.World);
            }

            else if (Input.GetKey(KeyCode.Y))
            {
                showAxis.showY();
                //disable gravity and collider
                disableGravityAndCollider();

                //get mouse position on x axis
                y = -1 * speed * Input.GetAxis("Mouse X");

                //rotate object
                go.transform.Rotate(0, y, 0, Space.World);
            }

            else if (Input.GetKey(KeyCode.Z))
            {
                showAxis.showZ();
                //disable gravity and collider
                disableGravityAndCollider();

                //get mouse position on x axis
                z = speed * Input.GetAxis("Mouse X");

                //rotate object
                go.transform.Rotate(0, 0, z, Space.World);
            }
            else
            {
                restoreGravityAndCollider();
                showAxis.hideX();
                showAxis.hideY();
                showAxis.hideZ();
            }
        }

        //-------------------drag-------------------

        private void setOffset()
        {
            mZCoord = Camera.main.WorldToScreenPoint(go.transform.position).z;
            yPos = go.transform.position.y;

            // Store offset = gameobject world pos - mouse world pos
            mOffset = go.transform.position - GetMouseAsWorldPoint();
        }


        private Vector3 GetMouseAsWorldPoint()
        {
            // Pixel coordinates of mouse (x,y)
            Vector3 mousePoint = Input.mousePosition;

            // z coordinate of game object on screen
            mousePoint.z = mZCoord;

            // Convert it to world points
            return Camera.main.ScreenToWorldPoint(mousePoint);
        }


        public void handleDrag()
        {
            if (Input.GetKey(KeyCode.M))
            {
                disableGravityAndCollider();

                //stay at this height
                if (lockY)
                {
                    go.transform.position = new Vector3(GetMouseAsWorldPoint().x + mOffset.x, yPos,
                        GetMouseAsWorldPoint().z + mOffset.z);
                }
                //change height with mouse
                else
                {
                    go.transform.position = GetMouseAsWorldPoint() + mOffset;
                    if (go.transform.position.y < 0)
                    {
                        go.transform.position = new Vector3(go.transform.position.x, 0, go.transform.position.z);
                    }
                }
            }
            else
            {
                restoreGravityAndCollider();
            }
        }

        //-------------------safe, restore and disable settings-------------------

        //safe gravity and collider settings
        private void safeGravityAndCollider()
        {
            try
            {
                gravity = go.GetComponent<Rigidbody>().useGravity;
                collider = go.GetComponent<Collider>().enabled;
            }
            catch
            {
            }
        }

        //restore gravity and collider settings
        private void restoreGravityAndCollider()
        {
            try
            {
                go.GetComponent<Rigidbody>().useGravity = gravity;
                go.GetComponent<Collider>().enabled = collider;
            }
            catch
            {
            }
        }

        //disable gravity and collider settings
        private void disableGravityAndCollider()
        {
            try
            {
                go.GetComponent<Rigidbody>().useGravity = false;
                go.GetComponent<Collider>().enabled = false;
            }
            catch
            {
            }
        }
    }
