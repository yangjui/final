using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject mirror1 = null;
    [SerializeField]
    private GameObject mirror2 = null;

    private Animator animator1;
    private Animator animator2;

    private bool isInvoked = false;

    private void Awake()
    {
        animator1 = mirror1.GetComponent<Animator>();
        animator2 = mirror2.GetComponent<Animator>();
        animator1.enabled = false;
        animator2.enabled = false;
        mirror1.GetComponent<mirrorAni>().enabled = false;
        mirror2.GetComponent<mirrorAni>().enabled = false;
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            if (isInvoked) return;

            animator1.enabled = true;
            animator2.enabled = true;
            mirror1.GetComponent<mirrorAni>().enabled = true;
            mirror2.GetComponent<mirrorAni>().enabled = true;

            isInvoked = true;
        }
    }
}
