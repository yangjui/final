using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGateManager : MonoBehaviour
{
    [SerializeField]
    private SettingManager settingManager;
    [SerializeField]
    private ActionController actionController;
    [SerializeField]
    private Player player;
    [SerializeField]
    private SlotKey slotkey;

    [SerializeField]
    private Text openText = null;
    [SerializeField]
    private GameObject gate_R;
    [SerializeField]
    private GameObject gate_L;

    private Animator gate_L_Animator;
    private Animator gate_R_Animator;

    [System.NonSerialized]
    public bool isEscape = false;
    private bool isCanOpen = false;

    [SerializeField]
    private Renderer keyPadRenderer = null;
    //[SerializeField]
    //private GameObject keyPad = null;
    [SerializeField]
    private AudioSource keyPadSound = null;
    [SerializeField]
    private List<AudioClip> audioClips = null;

    [SerializeField]
    private GameObject gateOpenSound = null;
    private AudioSource gateOpenSoundSource;

    private void Awake()
    {
        openText.gameObject.SetActive(false);
        gate_L_Animator = gate_L.GetComponent<Animator>();
        gate_R_Animator = gate_R.GetComponent<Animator>();
        gateOpenSoundSource = gateOpenSound.GetComponent<AudioSource>();
        gateOpenSoundSource.enabled = false;
    }

    private void Update()
    {
        if (SettingManager.isSettingMenuAct) return;
        Debug.Log("actionController.isKeyOnHand" + actionController.isKeyOnHand);

        if (actionController.isKeyOnHand)
        {
            PlayerRayForDoor();
        }
    }

    private void PlayerRayForDoor()
    {
        if (player.rayString == gameObject.name)
        {
            openText.gameObject.SetActive(true);
            openText.text = " ¹® ¿­±â " + "<color=yellow>" + "[X]" + "</color>";
            isCanOpen = true;
        }
        else
        {
            openText.gameObject.SetActive(false);
        }

        if (isCanOpen)
        {
            DoorOpen();
        }
        else
        {
            openText.gameObject.SetActive(false);
        }
    }

    private void DoorOpen()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (actionController.isRealKeyOnHand)
            {
                keyPadRenderer.material.color = Color.green;
                Invoke("KeypadColorReturn", 1f);
                keyPadSound.clip = audioClips[0];
                keyPadSound.Play();
                gate_L_Animator.SetBool("isOpen", true);
                gate_R_Animator.SetBool("isOpen", true);
                gateOpenSoundSource.enabled = true;
                isEscape = true;
            }
            else
            {
                keyPadRenderer.material.color = Color.red;
                Invoke("KeypadColorReturn", 1f);
                keyPadSound.clip = audioClips[1];
                keyPadSound.Play();
            }
            actionController.isKeyOnHand = false;
            slotkey.RemoveItem();
            openText.gameObject.SetActive(false);
            isCanOpen = false;
        }
    }

    private void KeypadColorReturn()
    {
        keyPadRenderer.material.color = Color.white;
    }
}