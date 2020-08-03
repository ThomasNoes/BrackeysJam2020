﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownMovement : MonoBehaviour
{
    [Header("Assignables")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private string keyboardSchemeName = "KeyboardMouse";
    [SerializeField] private string gamepadSchemeName = "Gamepad";

    [Header("Properties")]
    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] LayerMask groundMask;

    private Camera cam;
    private PlayerControls inputActions;
    private Vector2 movementInput;
    private Vector2 lookInput;
    private Rigidbody rb;

    private void Awake()
    {
        inputActions = new PlayerControls();
        inputActions.Player.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();

        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        //transform.position += calculateMovement() * Time.fixedDeltaTime;
        rb.AddForce(calculateMovement() * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }

    private void LateUpdate()
    {        
        transform.LookAt(calculateOrientation());
    }

    private Vector3 calculateMovement()
    {
        //not normalized movement
        Vector3 forwardMove = Vector3.forward * movementInput.y;
        Vector3 sideMove = Vector3.right * movementInput.x;

        //normalize the combined input
        Vector3 desiredMove = Vector3.Normalize(forwardMove + sideMove) * moveSpeed;
        return desiredMove;
    }

    private Vector3 calculateOrientation()
    {
        Vector3 desiredLookPoint = Vector3.zero;
        if (GetCurrentControlscheme() == gamepadSchemeName)
        {
            //Debug.Log("Using gamepad!");
             desiredLookPoint = new Vector3(transform.position.x + lookInput.x, transform.position.y, 
                transform.position.z + lookInput.y);
        }
        else if(GetCurrentControlscheme() == keyboardSchemeName)
        {
            //Debug.Log("Using keyboard and mouse!");

            Vector3 mousePos = inputActions.Player.MousePosition.ReadValue<Vector2>();
            Ray camRay = cam.ScreenPointToRay(mousePos);
            
            RaycastHit hit;
            if(Physics.Raycast(camRay, out hit, Mathf.Infinity, groundMask))
            {
                mousePos.z = hit.distance;
                Vector3 playerToMouse = hit.point - transform.position;
                playerToMouse.y = transform.position.y;
                desiredLookPoint = playerToMouse;
            }
            else
            {
                mousePos.z = (cam.transform.position - transform.position).magnitude;
                desiredLookPoint = cam.ScreenToWorldPoint(mousePos);
                desiredLookPoint.y = transform.position.y;
            }
        }

        return desiredLookPoint;
    }

    string GetCurrentControlscheme()
    {
        string controlScheme = playerInput.currentControlScheme;
        return controlScheme;
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
