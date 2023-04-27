using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnce : MonoBehaviour
{
    [SerializeField]
    private GameObject TriggerSound;
    private AudioSource audio;
    private bool isInvoked = false;

    private void Start()
    {
        audio = TriggerSound.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            if (isInvoked) return;

            audio.Play();
            isInvoked = true;
        }
    }
}