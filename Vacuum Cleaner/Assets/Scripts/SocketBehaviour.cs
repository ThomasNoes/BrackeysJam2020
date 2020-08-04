using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SocketBehaviour : MonoBehaviour
{
    bool interactable = false;
    public GameObject signifier;

    public string plugText;
    public string unplugText;
    public string tangleCordText;
    public string missingCordText;

    TMP_Text text;

    bool cordAttached;

    CordBehaviour cb;
    Assets.Scripts.Interaction.Vacuum.VacuumSource vs;

    private PlayerControls inputActions;

    void Awake()
    {
        inputActions = new PlayerControls();
    }

    // Start is called before the first frame update
    void Start()
    {
        cb = GameObject.Find("CordHead").GetComponent<CordBehaviour>();
        vs = GameObject.Find("vaccum head").GetComponent<Assets.Scripts.Interaction.Vacuum.VacuumSource>();
        text = gameObject.GetComponentInChildren<TMP_Text>();
        text.transform.rotation = Camera.main.transform.rotation;
        signifier.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (interactable)
        {

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
        vs.PowerOn();
        UpdateText();
    }

    private void DetachCord()
    {
        Debug.Log("DETACH!");
        cb.ResetHook();
        cordAttached = false;
        vs.PowerOff();
        UpdateText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            interactable = true;
            signifier.SetActive(true);
            UpdateText();
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

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
