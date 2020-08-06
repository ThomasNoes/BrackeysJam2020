using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using Assets.Scripts.Audio;
using UnityEngine.InputSystem;
using Assets.Scripts.Input;

public class SocketBehaviour : MonoBehaviour
{
    bool interactable = false;
    public GameObject signifier;

    public string plugText;
    public string unplugText;
    public string tangleCordText;
    public string missingCordText;


    [Space]public GameObject audioComponent;
    private IAudio audio;

    TMP_Text text;

    bool cordAttached;

    CordBehaviour cb;
    Assets.Scripts.Interaction.Vacuum.VacuumSource vs;

    private PlayerControls inputActions;
    private InputHandler inputHandler;

    // Start is called before the first frame update
    void Start()
    {
        inputHandler = FindObjectOfType<InputHandler>();
        if (inputHandler != null)
            inputActions = inputHandler.GetPlayerControls();

        cb = GameObject.Find("CordHead").GetComponent<CordBehaviour>();
        vs = GameObject.Find("vaccum head").GetComponent<Assets.Scripts.Interaction.Vacuum.VacuumSource>();
        text = gameObject.GetComponentInChildren<TMP_Text>();
        //text.transform.rotation = Camera.main.transform.rotation;
        signifier.SetActive(false);
        audio = audioComponent.GetComponent<IAudio>();


    }

    // Update is called once per frame
    void Update()
    {


        if (interactable)
        {
            if (inputActions.Player.Fire.triggered 
                && !cb.GetIsHooked())
            {
                AttachCord();

            }
            else if (cordAttached && !cb.GetRopeBent() && inputActions.Player.Fire.triggered)
            {
                DetachCord();
            }
            else if ((cb.GetIsHooked() && !cordAttached || cb.GetRopeBent()) && inputActions.Player.Fire.triggered)
            {
                audio.Play(2);
            }

        }
    }

    private void AttachCord()
    {
        Debug.Log("ATTACH!");
        cb.AttachToSocket(gameObject);
        cordAttached = true;
        vs.PowerOn();
        UpdateText();
        audio.Play(0);
    }

    private void DetachCord()
    {
        Debug.Log("DETACH!");
        cb.ResetHook();
        cordAttached = false;
        vs.PowerOff();
        UpdateText();
        audio.Play(1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            interactable = true;
            signifier.SetActive(true);
            UpdateText();
            audio.Play(3);
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

    private void UpdateText()
    {
        if (cb.GetIsHooked() && !cordAttached || cb.GetRopeBent())
        {
            text.text = tangleCordText;
        }
        else if (cordAttached)
        {
            text.text = unplugText;
        }
        else if (!cordAttached)
        {
            text.text = plugText;
        }
    }
}
