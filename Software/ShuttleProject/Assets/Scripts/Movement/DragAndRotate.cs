using UnityEngine;

    public class DragAndRotate
    {
        //The GameObject
        private GameObject _go;

        //drag variables
        private Vector3 _mOffset;
        private float _mZCoord;
        private readonly bool _lockY;
        private float _yPos;
        private Vector3 _hitpoint;
        private Vector3 _hitpointNormal;
        private Vector3 _goPosOld;
        private Vector3 _dragOffset;

        //rotate variables
        private bool _gravity;
        private bool _collider;
        private float _speed = 5.0F;
        private float _x;
        private float _y;
        private float _z;
        private Vector3 _pos;
        
        //classes
        private ShowAxis showAxis;
        private ObjectBounds _objectBounds;

        public DragAndRotate(GameObject go, Vector3 hitpoint, Vector3 hitpointNormal, bool lockY)
        {
            _go = go;
            _goPosOld = go.transform.position;
            _lockY = lockY;

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

                showAxis.ShowX();
                //disable gravity and collider
                disableGravityAndCollider();

                //get mouse position on x axis
                _x = _speed * Input.GetAxis("Mouse X");

                //rotate object
                _go.transform.Rotate(_x, 0, 0, Space.World);
            }

            else if (Input.GetKey(KeyCode.Y))
            {
                showAxis.ShowY();
                //disable gravity and collider
                disableGravityAndCollider();

                //get mouse position on x axis
                _y = -1 * _speed * Input.GetAxis("Mouse X");

                //rotate object
                _go.transform.Rotate(0, _y, 0, Space.World);
            }

            else if (Input.GetKey(KeyCode.Z))
            {
                showAxis.ShowZ();
                //disable gravity and collider
                disableGravityAndCollider();

                //get mouse position on x axis
                _z = _speed * Input.GetAxis("Mouse X");

                //rotate object
                _go.transform.Rotate(0, 0, _z, Space.World);
            }
            else
            {
                restoreGravityAndCollider();
                showAxis.HideX();
                showAxis.HideY();
                showAxis.HideZ();
            }
        }

        //-------------------drag-------------------

        private void setOffset()
        {
            _mZCoord = Camera.main.WorldToScreenPoint(_go.transform.position).z;
            _yPos = _go.transform.position.y;

            // Store offset = gameobject world pos - mouse world pos
            _mOffset = _go.transform.position - GetMouseAsWorldPoint();
        }


        private Vector3 GetMouseAsWorldPoint()
        {
            // Pixel coordinates of mouse (x,y)
            Vector3 mousePoint = Input.mousePosition;

            // z coordinate of game object on screen
            mousePoint.z = _mZCoord;

            // Convert it to world points
            return Camera.main.ScreenToWorldPoint(mousePoint);
        }


        public void handleDrag()
        {
            if (Input.GetKey(KeyCode.M))
            {
                disableGravityAndCollider();

                //stay at this height
                if (_lockY)
                {
                    _go.transform.position = new Vector3(GetMouseAsWorldPoint().x + _mOffset.x, _yPos,
                        GetMouseAsWorldPoint().z + _mOffset.z);
                    _dragOffset = _go.transform.position - _goPosOld;
                }
                //change height with mouse
                else
                {
                    _go.transform.position = GetMouseAsWorldPoint() + _mOffset;
                    if (_go.transform.position.y < 0)
                    {
                        _go.transform.position = new Vector3(_go.transform.position.x, 0, _go.transform.position.z);
                    }
                    _dragOffset = _go.transform.position - _goPosOld;
                }
            }
            else
            {
                restoreGravityAndCollider();
            }
        }
        public Vector3 GetOffsetAfterDrag()=>_dragOffset;
        
        //-------------------safe, restore and disable Settings-------------------

        //safe gravity and collider settings
        private void safeGravityAndCollider()
        {
            try
            {
                _collider = _go.GetComponent<Collider>().enabled;
                _gravity = _go.GetComponent<Rigidbody>().useGravity;

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
                _go.GetComponent<Rigidbody>().useGravity = _gravity;
                _go.GetComponent<Collider>().enabled = _collider;
            }
            catch
            {
            }
        }

        //disable gravity and collider settings
        public void disableGravityAndCollider()
        {
            try
            {
                _go.GetComponent<Rigidbody>().useGravity = false;
                _go.GetComponent<Collider>().enabled = false;
            }
            catch
            {
            }
        }
    }
