using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;

public class OutroManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player = null;
    [SerializeField]
    private GameObject chaser = null;
    [SerializeField]
    private GameObject chaserEye = null;
    [SerializeField]
    private GameObject playerMovePosition = null;

    [SerializeField]
    private GameObject thankText = null;

    [SerializeField]
    private Animator playerRunAnimation = null;
    [SerializeField]
    private Animator Door1Animation = null;
    [SerializeField]
    private Animator Door2Animation = null;
    [SerializeField]
    private Animator chaserAnimation = null;

    [SerializeField]
    private GameObject CV1 = null;

    [SerializeField]
    private GameObject fade = null;

    private bool isDoorOpen = false;

    private float alpha = 0f;

    private void Awake()
    {
        fade.SetActive(false);
        thankText.SetActive(false);
    }

    private void Start()
    {
        Invoke("DoorOpen", 2f);
    }

    private void Update()
    {
        if (isDoorOpen)
        {
            PlayerAndChaserMoveStart();

            if (Vector3.Distance(chaser.transform.position, transform.position) < 0.1f)
            {
                chaserAnimation.SetBool("isWalk", false);
                chaser.transform.position = chaser.transform.position;
                CV1.GetComponent<CinemachineVirtualCamera>().m_LookAt = player.transform;
            }

            if (Vector3.Distance(player.transform.position, transform.position) < 0.1f)
            {
                CV1.GetComponent<CinemachineVirtualCamera>().m_LookAt = player.transform;
            }

            if (alpha >= 3f)
            {
                thankText.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SceneManager.LoadScene("Title");
                }
            }
        }


        if (Vector3.Distance(player.transform.position, playerMovePosition.transform.position) < 5f)
        {
            player.SetActive(false);
            CV1.GetComponent<CinemachineVirtualCamera>().m_LookAt = chaserEye.transform;
            CV1.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView -= Time.deltaTime * 10f;
            chaser.transform.LookAt(CV1.transform.position);
            if (CV1.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView < 2f)
            {
                fade.SetActive(true);
                StartCoroutine(fadeCorutin());
            }
        }

    }

    private void DoorOpen()
    {
        Door1Animation.SetBool("isOpen", true);
        Door2Animation.SetBool("isOpen", true);
        isDoorOpen = true;
        CancelInvoke("DoorOpen");
    }


    private void PlayerAndChaserMoveStart()
    {
        playerRunAnimation.SetBool("isRun", true);
        chaserAnimation.SetBool("isWalk", true);

        player.transform.position = Vector3.MoveTowards(player.transform.position, playerMovePosition.transform.position, Time.deltaTime * 9f);
        player.transform.LookAt(playerMovePosition.transform.position);

        chaser.transform.position = Vector3.MoveTowards(chaser.transform.position, transform.position, Time.deltaTime * 4f);
        chaser.transform.LookAt(player.transform.position);
    }


    private IEnumerator fadeCorutin()
    {
        while (alpha <= 3f)
        {
            alpha += 0.001f;
            yield return
            fade.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
        }
    }
}