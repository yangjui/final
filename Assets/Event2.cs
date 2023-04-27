using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Event2 : MonoBehaviour
{
    [SerializeField]
    private GameObject chaser = null;

    [SerializeField]
    private GameObject player = null;

    [SerializeField]
    private GameObject Setposiotn = null;

    [SerializeField]
    private GameObject PlayerMainCamera = null;

    private bool isPlayerCanMove = true;

    private int justOneTime = 1;

    [SerializeField]
    private GameObject lightForChaser;

    private void Start()
    {
        
    }

    private void Update()
    {
        if(!isPlayerCanMove)
        {
            player.transform.LookAt(chaser.transform.position);
        }
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            player.GetComponent<Player>().enabled = false;
            player.GetComponent<Animator>().SetBool("isWalk", false);
            player.GetComponent<Animator>().SetBool("isRun", false);
            lightForChaser.GetComponent<Light>().intensity = 2f;
            isPlayerCanMove = false;
            Invoke("NewNav", 3f);
            Invoke("ReMove", 6f);
        }
    }

    private void NewNav()
    {
        chaser.GetComponent<NavMeshAgent>().enabled = true;
        if(justOneTime == 1)
        {
            chaser.GetComponent<NavMeshAgent>().SetDestination(Setposiotn.transform.position);
        }
        ++justOneTime;
        chaser.GetComponent<Animator>().SetBool("isWalk", true);
        isPlayerCanMove = true;
    }

    private void ReMove()
    {
        player.GetComponent<Player>().enabled = true;
        lightForChaser.GetComponent<Light>().intensity = 0.18f;
        gameObject.SetActive(false);
    }


}
