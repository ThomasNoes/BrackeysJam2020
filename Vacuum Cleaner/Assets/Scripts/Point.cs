using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Point : MonoBehaviour
{
    [SerializeField] private float gizmoSize = .5f;
    private void OnDrawGizmos()
    {
        PointGizmo();
    }
    private void PointGizmo()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawCube(transform.position, Vector3.one * gizmoSize);
    }
}
