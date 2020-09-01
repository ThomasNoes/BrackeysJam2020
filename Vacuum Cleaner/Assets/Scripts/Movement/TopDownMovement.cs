using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Assets.Scripts.Input;

public class TopDownMovement : MonoBehaviour
{
    [Header("Assignables")] [SerializeField]
    private GameObject playerCharacter;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private string keyboardSchemeName = "KeyboardMouse";
    [SerializeField] private string gamepadSchemeName = "Gamepad";

    [Header("Properties")]
    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] LayerMask groundMask;

    private Camera cam;
    private PlayerControls inputActions;
    private InputHandler inputHandler;
    private Vector2 movementInput;
    private Vector2 lookInput;
    private Rigidbody rb;
    private IMove moveInterface;

    private Vector3 previousRotationInput = Vector3.zero;

    private Vector3 LastestLookRotation;

    private void Start()
    {
        if (playerCharacter != null)
            moveInterface = playerCharacter.GetComponent<IMove>();

        inputHandler = InputHandler.inputHandler;
        if (inputHandler != null)
        {
            inputActions = inputHandler.GetPlayerControls();

            inputActions.Player.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
            inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
            inputActions.Player.Move.performed += ctx => ToggleMovementAnimation(ctx.ReadValue<Vector2>());
        }

        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        LastestLookRotation = calculateOrientation();
        transform.rotation = Quaternion.LookRotation(LastestLookRotation);
        rb.AddForce(calculateMovement() * Time.fixedDeltaTime, ForceMode.VelocityChange);
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
        #region Gamepad character rotation
        if (GetCurrentControlscheme() == gamepadSchemeName)
        {
            desiredLookPoint = new Vector3(lookInput.x, 0, lookInput.y);
            if(desiredLookPoint == Vector3.zero)
            {
                desiredLookPoint = previousRotationInput;
            }
            previousRotationInput = desiredLookPoint;
            return desiredLookPoint;
        }
        #endregion
        else if (GetCurrentControlscheme() == keyboardSchemeName)
        {
            //Debug.Log("Using keyboard and mouse!");

            Vector3 mousePos = inputActions.Player.MousePosition.ReadValue<Vector2>();
            Ray camRay = cam.ScreenPointToRay(mousePos);
            Debug.DrawRay(camRay.origin, camRay.direction*100, Color.blue);

            RaycastHit hit;
            if (Physics.Raycast(camRay, out hit, Mathf.Infinity, groundMask))
            {
                mousePos.z = hit.distance;
                Vector3 playerToMouse = hit.point - transform.position;
                playerToMouse.y = 0;// transform.position.y;
                desiredLookPoint = playerToMouse;
            }
            else
            {
                mousePos.z = (cam.transform.position - transform.position).magnitude;
                desiredLookPoint = cam.ScreenToWorldPoint(mousePos);
                desiredLookPoint.y = 0;// transform.position.y;
            }
        }

        //check if there was an input
        if (desiredLookPoint.magnitude !=0)
        {
            return desiredLookPoint;
        }
        else
        {
            return LastestLookRotation;
        }

    }

    string GetCurrentControlscheme()
    {
        string controlScheme = playerInput.currentControlScheme;
        return controlScheme;
    }

    private void ToggleMovementAnimation(Vector2 moveValue)
    {
        if (moveInterface == null)
            return;

        if (moveValue.x <= 0.1f && moveValue.y <= 0.1f
            && moveValue.x >= -0.1f && moveValue.y >= -0.1f)
            moveInterface.StopMoving();
        else
            moveInterface.StartMoving();
    }
}
