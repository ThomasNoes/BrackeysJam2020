using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    [SerializeField] private float moveSpeed = 5f;

    private PlayerControls inputActions;
    private Vector2 movementInput;
    private Vector2 lookInput;

    private void Awake()
    {
        inputActions = new PlayerControls();
        inputActions.Player.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();

    }

    private void Update()
    {
        transform.position += calculateMovement();
    }

    private Vector3 calculateMovement()
    {
        Debug.Log("Movement input = " + movementInput.x + " : " + movementInput.y);
        //not normalized movement
        Vector3 forwardMove = transform.forward * movementInput.y;
        Vector3 sideMove = transform.right * movementInput.x;

        Vector3 desiredMove = Vector3.Normalize(forwardMove + sideMove) * moveSpeed * Time.deltaTime;
        return desiredMove;
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
