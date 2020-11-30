using Scripts;
using UnityEngine;

public class ChaserCamera : MonoBehaviour
{
    
    public float smoothness;
    public Transform targetObject;
    private Vector3 _initalOffset;
    private Vector3 _cameraPosition;
    private bool _followMode ;
    
   
    // Start is called before the first frame update
    private void Start()
    {
        targetObject = GameObject.Find("Avatar").transform;
        _initalOffset = new Vector3(0,2,-2) - targetObject.position;
        MenuManager.ShowMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _followMode = !_followMode;
        }
        
        if (_followMode) {
            
        }
        else
        {
            _cameraPosition = targetObject.position + _initalOffset;
            transform.position = Vector3.Lerp(transform.position, _cameraPosition, smoothness*Time.fixedDeltaTime);
        }
    }
}
