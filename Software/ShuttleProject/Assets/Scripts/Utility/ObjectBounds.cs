using UnityEngine;

public class ObjectBounds
{
    private readonly Renderer _rend;
        
    public ObjectBounds(GameObject go)
    {
        _rend = go.GetComponent<Renderer>();
    }
        
    //Return the max bounds
    public Vector3 GetMaxBounds() => _rend.bounds.max;
    
    //Return the min bounds
    public Vector3 GetMinBounds() => _rend.bounds.min;
}
