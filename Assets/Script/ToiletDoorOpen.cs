using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToiletDoorOpen : MonoBehaviour
{
    [SerializeField]
    private Player player = null;

    [SerializeField]
    private Text openText = null;

    private AudioSource openSound;

    [SerializeField]
    private GameObject openTextUI = null;

    private bool isCanOpen = false;

    private void Awake()
    {
        openText.text = "¹® ¿­±â " + "<color=yellow>" + "[X]" + "</color>";
        openTextUI.SetActive(false);

        openSound = GetComponent<AudioSource>();
        openSound.enabled = false;
    }

    private void Update()
    {
        if (!isCanOpen) return;

        if (Input.GetKey(KeyCode.X))
        {
            RotateDoor();
        }
    }

    private void OnTriggerStay(Collider _other)
    {
        if (!_other.CompareTag("Player")) return;
        if (transform.name != player.DoorNameCheck()) return;
        isCanOpen = true;
        openTextUI.SetActive(true);
        openSound.enabled = true;
    }

    private void OnTriggerExit(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            openTextUI.SetActive(false);
        }
        isCanOpen = false;
    }

    private void RotateDoor()
    {
        transform.GetChild(0).GetChild(0).transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        openSound.Play();
        openTextUI.SetActive(false);
        Invoke("DestroyAll", 1f);
    }

    private void DestroyAll()
    {
        Destroy(transform.GetComponent<ToiletDoorOpen>());
        Destroy(openSound);
    }
}
