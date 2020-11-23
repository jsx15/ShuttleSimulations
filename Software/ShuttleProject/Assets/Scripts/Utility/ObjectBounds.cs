using UnityEngine;

public class ObjectBounds
{
    private GameObject go;
    private Renderer rend;
        
    public ObjectBounds(GameObject go)
    {
        this.go = go;
        rend = go.GetComponent<Renderer>();
    }
        
    //Return the max bounds
    public Vector3 getMaxBounds() => rend.bounds.max;
    
    //Return the min bounds
    public Vector3 getMinBounds() => rend.bounds.min;
}
