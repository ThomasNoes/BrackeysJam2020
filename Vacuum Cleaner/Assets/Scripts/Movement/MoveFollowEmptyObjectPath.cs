using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFollowEmptyObjectPath : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Transform[] path;
    private int _currentCheckpointIndex = 1;
    private float checkpointErrorMargin = .1f;

    private void Start()
    {
        path = GetComponentsInChildren<Transform>();
        path[0] = null; //remove the parent object from the array
    }

    private void FixedUpdate()
    {
        MoveWithPath(speed);    
    }

    private void UpdateNextCheckpoint()
    {
        Debug.Log("Path = " + path.Length);
        if(_currentCheckpointIndex + 1 >= path.Length)
        {
            _currentCheckpointIndex = 1; //return first checkpoint position. 
        }
        else
        {
            _currentCheckpointIndex += 1;
            
        }
    }

    private void MoveWithPath(float speed)
    {
        Vector3 checkpoint = GetTargetPosition(path[_currentCheckpointIndex]);
        float dist = (checkpoint - transform.position).magnitude;

        if (dist <= checkpointErrorMargin)
        {
            UpdateNextCheckpoint();
        }
        else
        {
            if(dist <= speed * Time.deltaTime)
            {
                UpdateNextCheckpoint();
            }
        }
        Vector3 dir = GetTargetPosition(path[_currentCheckpointIndex]) + transform.position - transform.position;
        transform.Translate(dir * speed * Time.deltaTime);
    }

    private Vector3 GetTargetPosition(Transform obj)
    {
        Vector3 targetPos = obj.transform.localPosition;
        return targetPos;
    }
}
