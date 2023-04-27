using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Event3 : MonoBehaviour
{
    [SerializeField]
    private GameObject chaser = null;

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Chaser"))
        {
            chaser.GetComponent<Animator>().SetBool("isWalk", false);
            chaser.GetComponent<Chaser>().enabled = false;
            chaser.SetActive(false);
        }

        if(_other.CompareTag("Player"))
        {
            chaser.SetActive(true);
            chaser.transform.position = chaser.transform.position + new Vector3(Random.Range(10, 20), Random.Range(0, 10), Random.Range(10, 20));
            chaser.GetComponent<Chaser>().enabled = true;
        }
    }
}
