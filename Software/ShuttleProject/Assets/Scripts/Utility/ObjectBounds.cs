using UnityEngine;

public class ObjectBounds
{
    /// <summary>
    ///     The variable for renderer
    /// </summary>
    private readonly Renderer _rend;
        
    /// <summary>
    ///     Store renderer
    /// </summary>
    /// <param name="go">Game object whose renderer is stored</param>
    public ObjectBounds(GameObject go)
    {
        _rend = go.GetComponent<Renderer>();
    }
        
    /// <summary>
    ///     Return the max bounds
    /// </summary>
    /// <returns>Max bounds</returns>
    public Vector3 GetMaxBounds() => _rend.bounds.max;
    
    /// <summary>
    ///     Return the min bounds
    /// </summary>
    /// <returns>Min bounds</returns>
    public Vector3 GetMinBounds() => _rend.bounds.min;
}