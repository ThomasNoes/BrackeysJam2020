using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmo : MonoBehaviour
{
    public bool displayGizmo;
    public float yellowRadius;
    public float blueRadius;
    // Start is called before the first frame update
    void OnDrawGizmos()
    {
        if (displayGizmo)
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, yellowRadius);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, blueRadius);
        }

    }
}
