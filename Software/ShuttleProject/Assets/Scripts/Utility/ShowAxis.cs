//using DefaultNamespace;
using UnityEngine;
using Object = UnityEngine.Object;


public class ShowAxis
{
    /// <summary>
    /// selected object
    /// </summary>
    private readonly GameObject _obj;

    /// <summary>
    /// object for x-Axis
    /// </summary>
    private GameObject _xAxis;

    /// <summary>
    /// object for y-Axis
    /// </summary>
    private GameObject _yAxis;

    /// <summary>
    /// object for z-Axis
    /// </summary>
    private GameObject _zAxis;

    /// <summary>
    /// object for object bounds
    /// </summary>
    private readonly ObjectBounds _bounds;

    /// <summary>
    /// Constructor 
    /// </summary>
    /// <param name="obj">object to rotate</param>
    public ShowAxis(GameObject obj)
    {
        //set object to _obj
        this._obj = obj;
        
        //set bounds to _bounds
        _bounds = new ObjectBounds(obj);
    }

    /// <summary>
    /// Show x-Axis
    /// </summary>
    public void ShowX()
    {
        //create Axis if not exits
        if (_xAxis == null)
        {
            //create Axis
            _xAxis = new GameObject();
        }

        //Check if LineRenderer == null
        if (_xAxis.GetComponent<LineRenderer>() == null)
        {
            //max is maximum bounds + seeable length
            var max = 0.4f + _bounds.GetMaxBounds().x;
            
            //min is minimum bounds - seeable length
            var min = _bounds.GetMinBounds().x - 0.4f;
            
            //set start of x-Axis line
            var start = new Vector3(max, _obj.transform.position.y, _obj.transform.position.z);
            
            //set end of x-Axis line
            var end = new Vector3(min, _obj.transform.position.y, _obj.transform.position.z);
            
            //Draw line
            DrawLine(_xAxis, start, end, Color.red);
        }
        
        //set LineRenderer enabled
        _xAxis.GetComponent<LineRenderer>().enabled = true;
    }

    /// <summary>
    /// Hide x-Axis
    /// </summary>
    public void HideX()
    {
        //Destroy x-Axis
        Object.Destroy(_xAxis);
    }

    /// <summary>
    /// Show y-Axis
    /// </summary>
    public void ShowY()
    {
        //create Axis if not exits
        if (_yAxis == null)
        {
            //create Axis
            _yAxis = new GameObject();
        }

        //Check if LineRenderer == null
        if (_yAxis.GetComponent<LineRenderer>() == null)
        {
            //max is maximum bounds + seeable length
            var max = 0.4f + _bounds.GetMaxBounds().y;
            
            //min is minimum bounds - seeable length
            var min = _bounds.GetMinBounds().y - 0.4f;
            
            //set start of y-Axis line
            var start = new Vector3(_obj.transform.position.x, max, _obj.transform.position.z);
            
            //set end of y-Axis line
            var end = new Vector3(_obj.transform.position.x, min, _obj.transform.position.z);
            
            //Draw line
            DrawLine(_yAxis, start, end, Color.green);
        }
        
        //set LineRenderer enabled
        _yAxis.GetComponent<LineRenderer>().enabled = true;
    }

    /// <summary>
    /// Hide y-Axis
    /// </summary>
    public void HideY()
    {
        //Destroy x-Axis
        Object.Destroy(_yAxis);
    }

    /// <summary>
    /// Show z-Axis
    /// </summary>
    public void ShowZ()
    {
        //create Axis if not exits
        if (_zAxis == null)
        {
            //create Axis
            _zAxis = new GameObject();
        }

        //Check if LineRenderer == null
        if (_zAxis.GetComponent<LineRenderer>() == null)
        {
            //max is maximum bounds + seeable length
            var max = 0.4f + _bounds.GetMaxBounds().z;
            
            //min is minimum bounds - seeable length
            var min = _bounds.GetMinBounds().z - 0.4f;
            
            //set start of z-Axis line
            var start = new Vector3(_obj.transform.position.x, _obj.transform.position.y, max);
            
            //set end of z-Axis line
            var end = new Vector3(_obj.transform.position.x, _obj.transform.position.y, min);
            
            //Draw line
            DrawLine(_zAxis, start, end, Color.blue);
        }

        //set LineRenderer enabled
        _zAxis.GetComponent<LineRenderer>().enabled = true;
    }

    /// <summary>
    /// Hide z-Axis
    /// </summary>
    public void HideZ()
    {
        //Destroy z-Axis
        Object.Destroy(_zAxis);
    }

    /// <summary>
    /// Draw line
    /// </summary>
    /// <param name="axis">Line Renderer</param>
    /// <param name="start">Start of Axis</param>
    /// <param name="end">End of Axis</param>
    /// <param name="color">Color of Axis</param>
    /// <param name="duration">Duration of Axis</param>
    private static void DrawLine(GameObject axis, Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        //set position of Axis
        axis.transform.position = start;
        
        //Add LineRenderer to GameObject
        axis.AddComponent<LineRenderer>();
        
        //safe LineRenderer to varable lr
        LineRenderer lr = axis.GetComponent<LineRenderer>();
        
        //set Color of line
        lr.material.color = color;
        
#pragma warning disable 618
        
        //set width of line
        lr.SetWidth(0.015f, 0.015f);

#pragma warning restore 618
        
        //set start of line
        lr.SetPosition(0, start);
        
        //set end of line
        lr.SetPosition(1, end);
    }
}