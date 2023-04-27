using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject RoomTriggerSound;
    private AudioSource audio;

    private void Start()
    {
        audio = RoomTriggerSound.GetComponent<AudioSource>();
        audio.enabled = false;
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            audio.enabled = true;
        }
    }

    private void OnTriggerExit(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            audio.enabled = false;
        }
    }
}
