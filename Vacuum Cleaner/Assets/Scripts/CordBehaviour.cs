using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CordBehaviour : MonoBehaviour
{
    public GameObject ropePoint;
    //public float hookVelocity;
    bool isHooked = false;
    Rigidbody rb;
    public GameObject cordBase;
    ConfigurableJoint joint;
    public float maxLength;
    public float minLength;
    float CurrentLength;
    public float hookToBase;

    //shot raycasting
    int layerMask;
    RaycastHit shotHit;
    bool shotBool;

    //Rope
    List<GameObject> rp = new List<GameObject>();
    RaycastHit baseHit;
    RaycastHit pointHit;
    bool newPointBlocked;
    bool prevPointBlocked;
    float remainingLength;

    // Start is called before the first frame update
    void Start()
    {
        CurrentLength = maxLength;

        rb = GetComponent<Rigidbody>();

        joint = GetComponentInParent<ConfigurableJoint>();
        //initialPos = joint.connectedAnchor;
        ResetHook();

        layerMask = LayerMask.GetMask("Default"); //set layerMask to exclude Car layer
    }

    void Update()
    {
        if (isHooked)
        {
            RopeBendingCheck();
            //Debug.Log("current length: " + (rp[rp.Count - 1].transform.position - cordBase.transform.position).magnitude);
        }
    }

    public bool GetIsHooked()
    {
        return isHooked;
    }

    public bool GetRopeBent()
    {
        if (rp.Count >1)
            return true;
        return false;
    }

    public void RopeBendingCheck()
    {
        Vector3 newPoint = rp[rp.Count - 1].transform.position;
        Vector3 base2NewPoint = newPoint - cordBase.transform.position;

        //
        newPointBlocked = Physics.Raycast(cordBase.transform.position, base2NewPoint, out baseHit, base2NewPoint.magnitude - 0.1f, layerMask); // giving some leeway to max distance be subtracting 1
        if (newPointBlocked)
        {
            newPoint = baseHit.collider.ClosestPoint(baseHit.point);
            AddRopePoint(newPoint, baseHit.transform);
        }

        if (rp.Count > 1) // if the rope is bending on some surface.
        {
            Vector3 base2PrevPoint = rp[rp.Count - 2].transform.position - cordBase.transform.position;
            prevPointBlocked = Physics.Raycast(cordBase.transform.position, base2PrevPoint, base2PrevPoint.magnitude - 0.1f, layerMask);

            if (!prevPointBlocked)
            {
                Vector3 nearestPoint = cordBase.transform.position + Vector3.Project(rp[rp.Count - 1].transform.position - cordBase.transform.position, base2PrevPoint);
                Vector3 nearestVector = rp[rp.Count - 1].transform.position - nearestPoint;

                //Debug.DrawRay(nearestPoint, nearestVector, Color.blue);

                if (!Physics.Raycast(nearestPoint, nearestVector, nearestVector.magnitude - 0.1f, layerMask))
                {
                    RemoveNewestRopePoint();
                }
            }
        }
        DebugRope();
    }

    private void AddRopePoint(Vector3 point, Transform parent)
    {
        transform.position = point;
        rp.Add(Instantiate(ropePoint, point, Quaternion.identity, parent));
        SetRemainingLength();
    }
    private void RemoveNewestRopePoint()
    {
        Destroy(rp[rp.Count - 1]);
        rp.Remove(rp[rp.Count - 1]);
        transform.position = rp[rp.Count - 1].transform.position;
        SetRemainingLength();
    }

    private void SetRemainingLength()
    {
        remainingLength = maxLength;
        for (int i = 1; i < rp.Count; i++)
        {
            remainingLength -= Vector3.Distance(rp[i - 1].transform.position, rp[i].transform.position);
        }
        joint.linearLimit = SetLinearLimit(joint, remainingLength);
    }

    private SoftJointLimit SetLinearLimit(ConfigurableJoint joint, float Lenght)
    {
        SoftJointLimit softJoint = joint.linearLimit;
        softJoint.limit = Lenght;
        //Debug.Log("remaining lenght:" + Lenght);
        return softJoint;
    }

    private void DebugRope()
    {
        for (int i = 1; i < rp.Count; i++)
        {
            //Debug.DrawRay(rp[i].transform.position, Vector3.up, Color.yellow, 1);
            //Debug.DrawRay(rp[i-1].transform.position, rp[i].transform.position - rp[i-1].transform.position, Color.yellow, 1);
        }
        //Debug.DrawRay(rp[rp.Count-1].transform.position, hookBase.transform.position - rp[rp.Count - 1].transform.position, Color.yellow);
    }

    void constrainPosition(bool boolean)
    {
        if (boolean)
        {
            rb.constraints = RigidbodyConstraints.FreezePosition;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.None;
        }
    }

    public void AttachToSocket(GameObject socket)
    {
        CurrentLength = maxLength; //(socket.transform.position - cordBase.transform.position).magnitude;
        AddRopePoint(socket.transform.position, socket.transform);
        transform.SetParent(socket.transform);
        isHooked = true;
    }


    public void ShootHook()
    {
        if (shotBool) //if the ray cast did hit something,
        {
            CurrentLength = (shotHit.point - cordBase.transform.position).magnitude;
            Debug.Log("Shot distance: " + shotHit.distance);
            AddRopePoint(shotHit.point, shotHit.transform); //move hook to hit point
            transform.SetParent(shotHit.transform); // set the object that was hit as a parent
            isHooked = true;
        }
        else
        {
            ResetHook();
        }
        

    }

    public void ResetHook()
    {
        transform.SetPositionAndRotation(cordBase.transform.position, cordBase.transform.rotation);
        //UniversalFunctions.SetGlobalScale(transform, new Vector3(0.1f, 0.1f, 0.2f));
        rp.Clear();
        joint.linearLimit = SetLinearLimit(joint, maxLength);
        transform.SetParent(joint.transform);
        isHooked = false;
    }

    public List<GameObject> GetRopePoints()
    {
        return rp;
    }
}