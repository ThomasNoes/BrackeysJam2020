using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    LineRenderer line;

    public GameObject cordBase;

    List<GameObject> ropePoints;
    CordBehaviour cb;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        cb = GetComponent<CordBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        ropePoints = cb.GetRopePoints();
        //Debug.Log("ropePoints: " + ropePoints.Count);
        line.positionCount = ropePoints.Count + 1;

        for (int i = 0; i < ropePoints.Count; i++)
        {
            line.SetPosition(i, ropePoints[i].transform.position);
        }


        line.SetPosition(ropePoints.Count, cordBase.transform.position);
    }
}
