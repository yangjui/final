using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSound2 : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Debug.Log("2");
            audioSource.Play();
        }
    }
}
