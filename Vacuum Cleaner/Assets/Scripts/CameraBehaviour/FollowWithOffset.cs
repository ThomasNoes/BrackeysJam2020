using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWithOffset : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform objectToFollow;
    [SerializeField] private float smoothTime = 0.15f;
    [SerializeField] private float maxFollowSpeed = 20f;

    //for smoothdamp to reference
    private Vector3 velocity; 

    private void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, objectToFollow.position + offset, 
            ref velocity, smoothTime, maxFollowSpeed);
    }
}
