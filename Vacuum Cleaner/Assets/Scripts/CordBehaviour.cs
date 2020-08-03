using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CordBehaviour : MonoBehaviour
{
    public GameObject ropePoint;
    //public float hookVelocity;
    bool isHooked = false;
    Rigidbody rb;
    GameObject hookBase;
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

    //Reel Rope
    public float reelSpeed;
    public float quickReelMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        CurrentLength = maxLength;

        rb = GetComponent<Rigidbody>();
        hookBase = GameObject.Find("Hook Base");

        joint = GetComponentInParent<ConfigurableJoint>();
        //initialPos = joint.connectedAnchor;
        ResetHook();

        layerMask = ~LayerMask.GetMask("Car"); //set layerMask to exclude Car layer
    }

    public bool GetIsHooked()
    {
        return isHooked;
    }

    public void RopeBendingCheck()
    {
        //Debug.Log("ropePoints " + rp.Count);
        Vector3 newPoint = rp[rp.Count - 1].transform.position;
        Vector3 base2NewPoint = newPoint - hookBase.transform.position;

        //
        newPointBlocked = Physics.Raycast(hookBase.transform.position, base2NewPoint, out baseHit, base2NewPoint.magnitude - 1, layerMask); // giving some leeway to max distance be subtracting 1
        if (newPointBlocked)
        {
            newPoint = baseHit.collider.ClosestPoint(baseHit.point);
            AddRopePoint(newPoint, baseHit.transform);
        }

        if (rp.Count > 1) // if the rope is bending on some surface.
        {
            Vector3 base2PrevPoint = rp[rp.Count - 2].transform.position - hookBase.transform.position;
            prevPointBlocked = Physics.Raycast(hookBase.transform.position, base2PrevPoint, base2PrevPoint.magnitude - 1, layerMask);

            if (!prevPointBlocked)
            {
                Vector3 nearestPoint = hookBase.transform.position + Vector3.Project(rp[rp.Count - 1].transform.position - hookBase.transform.position, base2PrevPoint);
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
        Debug.Log("adding first point");
        transform.position = point;
        rp.Add(Instantiate(ropePoint, point, Quaternion.identity, parent));
        SetRemainingLength();
    }
    private void RemoveNewestRopePoint()
    {
        Debug.Log("removing point");
        Destroy(rp[rp.Count - 1]);
        rp.Remove(rp[rp.Count - 1]);
        transform.position = rp[rp.Count - 1].transform.position;
        SetRemainingLength();
    }

    private void SetRemainingLength()
    {
        remainingLength = CurrentLength;
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

    public void HookRayCast()
    {
        shotBool = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out shotHit, maxLength, layerMask);
        HighlightCrosshair(shotBool);
    }

    public void ShootHook()
    {
        if (shotBool) //if the ray cast did hit something,
        {
            CurrentLength = (shotHit.point - hookBase.transform.position).magnitude;
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

    public void QuickReel()
    {
        ReelRope(-1, quickReelMultiplier);
        if (CurrentLength <= minLength)
        {
            ResetHook();
        }
    }

    public void ReelRope(float input, float multiplier = 1)
    {

        CurrentLength += input * reelSpeed * multiplier * Time.deltaTime;

        if (CurrentLength > maxLength)
        {
            CurrentLength = maxLength;
        }
        else if (CurrentLength < minLength)
        {
            CurrentLength = minLength;
        }

        SetRemainingLength();
    }

    public void ResetHook()
    {
        transform.SetPositionAndRotation(hookBase.transform.position, hookBase.transform.rotation);
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