using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmo : MonoBehaviour
{
    public bool displayGizmo = true;
    public float yellowRadius = 10;
    public float blueRadius = 15.75f;
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
