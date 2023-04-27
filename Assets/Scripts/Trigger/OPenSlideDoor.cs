using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OPenSlideDoor : MonoBehaviour
{
    [SerializeField]
    private Player player = null;

    private Animator animator;

    [SerializeField]
    private Text openText = null;

    [SerializeField]
    private GameObject openTextUI = null;

    [SerializeField]
    private Animator ghostAni = null;

    private bool isCanOpen = false;

    [SerializeField]
    private AudioSource DoorSound;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        openText.text = "¹® ¿­±â " + "<color=yellow>" + "[X]" + "</color>";
        openTextUI.SetActive(false);
        DoorSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.X) && isCanOpen)
        {
            DoorSound.Play();
            animator.SetBool("isOpen", true);

            if (transform.name == "DOOR_InGhost")
            {
                Invoke("GhostAni", 0.3f);
            }
            isCanOpen = false;

            Destroy(DoorSound);
        }
    }

    private void OnTriggerStay(Collider _other)
    {
        if (!_other.CompareTag("Player")) return;

        if (_other.CompareTag("Player"))
        {
            if (transform.name != player.DoorNameCheck()) return;
            openTextUI.SetActive(true);
            isCanOpen = true;
        }
    }

    private void OnTriggerExit(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            openTextUI.SetActive(false);
            isCanOpen = false;
        }
    }

    private void DoorClose()
    {
        animator.SetBool("isOpen", false);
    }

    private void GhostAni()
    {
        ghostAni.SetBool("isMove", true);
    }
}
