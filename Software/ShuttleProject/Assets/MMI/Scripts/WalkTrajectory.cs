using MMIStandard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkTrajectory : MonoBehaviour
{
    public List<Transform> Points = new List<Transform>();

    public Color Color = Color.red;


    /// <summary>
    /// Returns a trajectory constraint describing the 2D positions (x-z)
    /// </summary>
    /// <returns></returns>
    public MConstraint GetPathConstraint()
    {
        MConstraint constraint = new MConstraint(System.Guid.NewGuid().ToString());

        MPathConstraint pathConstraint = new MPathConstraint()
        {
            PolygonPoints = new List<MGeometryConstraint>()
        };

        foreach (Transform t in this.Points)
        {
            pathConstraint.PolygonPoints.Add(new MGeometryConstraint()
            {
                ParentObjectID = "",
                ParentToConstraint = new MTransform("", new MVector3(t.position.x, 0, t.position.z), new MQuaternion(0, 0, 0, 1))
            });
        }

        constraint.PathConstraint = pathConstraint;

        return constraint;
    }


    private void OnDrawGizmos()
    {
        if (this.Points.Count >= 1)
        {
            for (int i = 0; i < this.Points.Count; i++)
            {
                if (this.Points[i] == null)
                    break;

                if (i < this.Points.Count - 1)
                {
                    if (this.Points[i + 1] != null)
                        Debug.DrawLine(this.Points[i].position, this.Points[i + 1].position, this.Color);
                }

                else
                {
                    if (this.Points[i] != null)
                        Debug.DrawLine(this.Points[i].position, this.transform.position, this.Color);
                }
            }


        }
    }

}
