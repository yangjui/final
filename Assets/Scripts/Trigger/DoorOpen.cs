using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorOpen : MonoBehaviour
{
    [SerializeField]
    private Player player = null;

    private Animator animator;

    [SerializeField]
    private Text openText = null;

    [SerializeField]
    private GameObject openTextUI = null;

    [SerializeField]
    private GameObject DoorSound = null;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        openText.text = "¹® ¿­±â " + "<color=yellow>" + "[X]" + "</color>";
        openTextUI.SetActive(false);
        DoorSound.SetActive(false);
    }

    private void OnTriggerStay(Collider _other)
    {
        if (!_other.CompareTag("Player")) return;

        if(_other.CompareTag("Player"))
        {
            if (transform.name != player.DoorNameCheck()) return;

            openTextUI.SetActive(true);

            if (Input.GetKey(KeyCode.X))
            {
                DoorSound.SetActive(true);
                animator.SetBool("isOpen", true);
            }
        }
    }

    private void OnTriggerExit(Collider _other)
    {
        if(_other.CompareTag("Player"))
        {
            openTextUI.SetActive(false);
            DoorSound.SetActive(false);
        }
    }

    private void DoorClose()
    {
        animator.SetBool("isOpen", false);
        DoorSound.SetActive(false);
    }

    private void DoorColliderOff()
    {
        GetComponent<BoxCollider>().enabled = false;
        transform.GetChild(1).GetComponent<BoxCollider>().enabled = false;
        openTextUI.SetActive(false);
    }

    private void DoorColliderOn()
    {
        GetComponent<BoxCollider>().enabled = true;
        transform.GetChild(1).GetComponent<BoxCollider>().enabled = true;
        openTextUI.SetActive(false);
    }

}
