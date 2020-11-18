using System;
using System.Collections;
using System.Collections.Generic;
//using DefaultNamespace;
using UnityEngine;


public class ShowAxis
{
    private GameObject obj;
    private GameObject xAxis;
    private GameObject yAxis;
    private GameObject zAxis;

    private ObjectBounds bounds;

    public ShowAxis(GameObject obj)
    {
        this.obj = obj;
        bounds = new ObjectBounds(obj);
    }

    public void showX()
    {
        if (xAxis == null)
        {
            xAxis = new GameObject();
        }

        if (xAxis.GetComponent<LineRenderer>() == null)
        {
            float max = 0.4f + bounds.getMaxBounds().x;
            float min = bounds.getMinBounds().x - 0.4f;
            Vector3 start = new Vector3(max, obj.transform.position.y, obj.transform.position.z);
            Vector3 end = new Vector3(min, obj.transform.position.y, obj.transform.position.z);
            DrawLine(xAxis, start, end, Color.red);
        }

        xAxis.GetComponent<LineRenderer>().enabled = true;
    }

    public void hideX()
    {
        GameObject.Destroy(xAxis);
    }

    public void showY()
    {
        if (yAxis == null)
        {
            yAxis = new GameObject();
        }

        if (yAxis.GetComponent<LineRenderer>() == null)
        {
            float max = 0.4f + bounds.getMaxBounds().y;
            float min = bounds.getMinBounds().y - 0.4f;
            Vector3 start = new Vector3(obj.transform.position.x, max, obj.transform.position.z);
            Vector3 end = new Vector3(obj.transform.position.x, min, obj.transform.position.z);
            DrawLine(yAxis, start, end, Color.green);
        }

        yAxis.GetComponent<LineRenderer>().enabled = true;
    }

    public void hideY()
    {
        GameObject.Destroy(yAxis);
    }

    public void showZ()
    {
        if (zAxis == null)
        {
            zAxis = new GameObject();
        }

        if (zAxis.GetComponent<LineRenderer>() == null)
        {
            float max = 0.4f + bounds.getMaxBounds().z;
            float min = bounds.getMinBounds().z - 0.4f;
            Vector3 start = new Vector3(obj.transform.position.x, obj.transform.position.y, max);
            Vector3 end = new Vector3(obj.transform.position.x, obj.transform.position.y, min);
            DrawLine(zAxis, start, end, Color.blue);
        }

        zAxis.GetComponent<LineRenderer>().enabled = true;
    }

    public void hideZ()
    {
        GameObject.Destroy(zAxis);
    }

    void DrawLine(GameObject axis, Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        axis.transform.position = start;
        axis.AddComponent<LineRenderer>();
        LineRenderer lr = axis.GetComponent<LineRenderer>();
        lr.material.color = color;
        lr.SetWidth(0.015f, 0.015f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        //GameObject.Destroy(axis, duration);
    }
}