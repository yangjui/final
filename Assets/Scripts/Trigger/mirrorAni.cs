using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mirrorAni : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private AudioSource BreakSound = null;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        BreakSound = GetComponent<AudioSource>();
    }

    public void AniStop()
    {
        Rigidbody[] childRigidbodies = GetComponentsInChildren<Rigidbody>();
        BoxCollider[] childColliders = GetComponentsInChildren<BoxCollider>();

        for (int i = 0; i < childRigidbodies.Length - 1; ++i)
        {
            childRigidbodies[i].useGravity = true;
            childColliders[i].isTrigger = false;
        }
        animator.enabled = false;
    }

    private void SoundOn()
    {
        BreakSound.Play();
    }
}
