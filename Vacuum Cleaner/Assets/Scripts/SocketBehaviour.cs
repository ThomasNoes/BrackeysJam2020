﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketBehaviour : MonoBehaviour
{
    bool interactable = false;
    public GameObject signifier;

    bool cordAttached;

    CordBehaviour cb;

    private PlayerControls inputActions;

    void Awake()
    {
        inputActions = new PlayerControls();
    }

    // Start is called before the first frame update
    void Start()
    {
        cb = GameObject.Find("CordHead").GetComponent<CordBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if (interactable)
        {
            Debug.Log(cb.name);

            if (!cb.GetIsHooked() && inputActions.Player.Fire.triggered)
            {
                AttachCord();
            }
            else if (cordAttached && !cb.GetRopeBent() && inputActions.Player.Fire.triggered)
            {
                DetachCord();
            }


        }
    }

    private void AttachCord()
    {
        Debug.Log("ATTACH!");
        cb.AttachToSocket(gameObject);
        cordAttached = true;
    }

    private void DetachCord()
    {
        Debug.Log("DETACH!");
        cb.ResetHook();
        cordAttached = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            interactable = true;
            signifier.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {

            interactable = false;
            signifier.SetActive(false);
        }
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