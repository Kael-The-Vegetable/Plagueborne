using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AddGizmo : MonoBehaviour
{
    public enum Shape
    {
        Sphere,
        WireSphere,
        Cube,
        WireCube,
        Ray,
    }

    public Shape gizmo;
    public Color color = Color.red;
    public bool onSelected;
    [Space]
    public Space space = Space.Self;
    public Vector3 size = Vector3.right;
    public Vector3 position;

    private void OnDrawGizmos()
    {
        if (enabled && !onSelected)
        {
            DrawGizmo();
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (enabled && onSelected)
        {
            DrawGizmo();
        }
    }

    private void DrawGizmo()
    {
        Vector3 gizmoPosition = transform.position + transform.rotation * position;
        Vector3 gizmoSize = space == Space.Self ? transform.localScale + size : size;
        Gizmos.color = color;
        switch (gizmo)
        {
            case Shape.Sphere:
                Gizmos.DrawSphere(gizmoPosition, gizmoSize.magnitude);
                break;
            case Shape.WireSphere:
                Gizmos.DrawWireSphere(gizmoPosition, gizmoSize.magnitude);
                break;
            case Shape.Cube:
                Gizmos.DrawCube(gizmoPosition, gizmoSize);
                break;
            case Shape.WireCube:
                Gizmos.DrawWireCube(gizmoPosition, gizmoSize);
                break;
            case Shape.Ray:
                gizmoSize = space == Space.Self ? transform.rotation * size : size;
                Gizmos.DrawRay(gizmoPosition, gizmoSize);
                break;
        }
    }
}
