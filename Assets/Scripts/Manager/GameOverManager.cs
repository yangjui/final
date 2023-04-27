using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private MainGateManager mainGateManager;

    [SerializeField]
    private GameObject chaser = null;
    [SerializeField]
    private GameObject chaserEye = null;

    [SerializeField]
    private Animator playerAnimator = null;

    [SerializeField]
    private Animator chaserAnimator = null;

    [SerializeField]
    private GameObject playerCamera = null;

    [SerializeField]
    private GameObject gameOverUI = null;
    private float deathTime = 0;

    [SerializeField]
    private List<GameObject> allUI = null;

    public bool isDead = false;

    private void Awake()
    {
        gameOverUI.SetActive(false);
    }

    private void Update()
    {

        if (Vector3.Distance(chaser.transform.position, player.PlayerCurPosition()) <= 2.5f && !isDead)
        {
            deathTime += Time.deltaTime;

            if (deathTime > 2f)
            {
                isDead = true;
            }
        }
        else
        {
            deathTime = 0;
        }

        if (isDead)
        {
            for (int i = 0; i < allUI.Count; ++i)
            {
                allUI[i].SetActive(false);
            }

            Vector3 targetDirection = chaser.transform.position - player.PlayerCurPosition();
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            player.transform.rotation = Quaternion.Lerp(player.transform.rotation, targetRotation, Time.deltaTime * 5f);


            Vector3 direction = player.PlayerCurPosition() - chaser.transform.position;
            direction.y = 0;

            chaserEye.transform.localEulerAngles = new Vector3(-13.032f, -90f, -90f);
            Invoke("LookEye", 0.3f);
        }

        if (mainGateManager.isEscape)
        {
            Invoke("Escape", 1.5f);
        }
    }

    private void LookEye()
    {
        Vector3 targetDirection = chaserEye.transform.position - playerCamera.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        playerCamera.transform.rotation = Quaternion.Lerp(playerCamera.transform.rotation, targetRotation, Time.deltaTime * 5f);
        playerCamera.GetComponent<Camera>().fieldOfView -= Time.deltaTime * 15f;
        if (playerCamera.GetComponent<Camera>().fieldOfView < 20)
        {
            gameOverUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public bool WhenDie()
    {
        return isDead;
    }

    private void Escape()
    {
        SceneManager.LoadScene("Outro");
    }

    public void ClickTitle()
    {
        SceneManager.LoadScene("Title");
    }
    public void ClickRetry()
    {
        SceneManager.LoadScene("MetaDream");
    }
}