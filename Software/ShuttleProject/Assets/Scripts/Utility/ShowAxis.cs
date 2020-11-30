//using DefaultNamespace;
using UnityEngine;
using Object = UnityEngine.Object;


public class ShowAxis
{
    private readonly GameObject _obj;
    private GameObject _xAxis;
    private GameObject _yAxis;
    private GameObject _zAxis;

    private readonly ObjectBounds _bounds;

    public ShowAxis(GameObject obj)
    {
        this._obj = obj;
        _bounds = new ObjectBounds(obj);
    }

    public void ShowX()
    {
        if (_xAxis == null)
        {
            _xAxis = new GameObject();
        }

        if (_xAxis.GetComponent<LineRenderer>() == null)
        {
            var max = 0.4f + _bounds.GetMaxBounds().x;
            var min = _bounds.GetMinBounds().x - 0.4f;
            var start = new Vector3(max, _obj.transform.position.y, _obj.transform.position.z);
            var end = new Vector3(min, _obj.transform.position.y, _obj.transform.position.z);
            DrawLine(_xAxis, start, end, Color.red);
        }

        _xAxis.GetComponent<LineRenderer>().enabled = true;
    }

    public void HideX()
    {
        Object.Destroy(_xAxis);
    }

    public void ShowY()
    {
        if (_yAxis == null)
        {
            _yAxis = new GameObject();
        }

        if (_yAxis.GetComponent<LineRenderer>() == null)
        {
            var max = 0.4f + _bounds.GetMaxBounds().y;
            var min = _bounds.GetMinBounds().y - 0.4f;
            var start = new Vector3(_obj.transform.position.x, max, _obj.transform.position.z);
            var end = new Vector3(_obj.transform.position.x, min, _obj.transform.position.z);
            DrawLine(_yAxis, start, end, Color.green);
        }

        _yAxis.GetComponent<LineRenderer>().enabled = true;
    }

    public void HideY()
    {
        Object.Destroy(_yAxis);
    }

    public void ShowZ()
    {
        if (_zAxis == null)
        {
            _zAxis = new GameObject();
        }

        if (_zAxis.GetComponent<LineRenderer>() == null)
        {
            var max = 0.4f + _bounds.GetMaxBounds().z;
            var min = _bounds.GetMinBounds().z - 0.4f;
            var start = new Vector3(_obj.transform.position.x, _obj.transform.position.y, max);
            var end = new Vector3(_obj.transform.position.x, _obj.transform.position.y, min);
            DrawLine(_zAxis, start, end, Color.blue);
        }

        _zAxis.GetComponent<LineRenderer>().enabled = true;
    }

    public void HideZ()
    {
        Object.Destroy(_zAxis);
    }

    private static void DrawLine(GameObject axis, Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        axis.transform.position = start;
        axis.AddComponent<LineRenderer>();
        LineRenderer lr = axis.GetComponent<LineRenderer>();
        lr.material.color = color;
#pragma warning disable 618
        lr.SetWidth(0.015f, 0.015f);
#pragma warning restore 618
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        //GameObject.Destroy(axis, duration);
    }
}