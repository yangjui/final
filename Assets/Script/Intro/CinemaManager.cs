using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;

public class CinemaManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineBlendListCamera blendList;
    [SerializeField]
    private GameObject CV1 = null;
    [SerializeField]
    private GameObject CV2 = null;
    [SerializeField]
    private GameObject CV3 = null;
    [SerializeField]
    private GameObject CV4 = null;
    [SerializeField]
    private GameObject building = null;
    private CinemachineVirtualCameraBase vCam1;
    private CinemachineVirtualCameraBase vCam2;
    private CinemachineVirtualCameraBase vCam3;
    private CinemachineVirtualCameraBase vCam4;

    [SerializeField]
    private string sceneName = "MetaDream";

    [SerializeField]
    private GameObject dateText = null;

    [SerializeField]
    private GameObject player = null;
    [SerializeField]
    private GameObject playerMovePosition = null;

    [SerializeField]
    private Animator walkAnimation = null;

    [SerializeField]
    private GameObject textCanvas = null;
    [SerializeField]
    private TMPro.TMP_Text text = null;
    private bool isTextStart = false;
    private int listNO = 0;
    private List<string> textList = new List<string>();
    string no1 = "여기가 바로 나의 새로운 일터이자 부산의 명물, 부산의 중심지 '영화의 전당'이구나";
    string no2 = "단순한 경비 업무 지만 수상할 정도로 일당이 높은 건 마음에 걸린다.";
    string no3 = "하지만 이런 기회를 놓칠 순 없지";
    string no4 = "업계 쪽 소문에 따르면 전시물들이 움직이고 경비원을 죽인다나 뭐라나";
    string no5 = "나는 그런 헛소문 믿지 않아, 열심히 일해보자고!";

    private bool camera3RotateStart = false;
    private float originalRotateY;
    private float currRotateY;
    private bool isEnterWalk = false;
    private bool isTextStartFuntion = true;
    float alpha = 0;

    [SerializeField]
    private GameObject fade = null;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        textCanvas.SetActive(false);
        dateText.SetActive(false);
        fade.SetActive(false);
        vCam1 = CV1.GetComponent<CinemachineVirtualCameraBase>();
        vCam3 = CV3.GetComponent<CinemachineVirtualCameraBase>();
        vCam2 = CV2.GetComponent<CinemachineVirtualCameraBase>();
        vCam4 = CV4.GetComponent<CinemachineVirtualCameraBase>();
        camera3RotateStart = false;
        originalRotateY = CV3.transform.eulerAngles.y;

    }

    private void Start()
    {
        FirstChange();
        textList.Add(no1);
        textList.Add(no2);
        textList.Add(no3);
        textList.Add(no4);
        textList.Add(no5);

        dateText.SetActive(true);
    }

    private void Update()
    {
        Debug.Log("listNO : " + listNO);
        Debug.Log("textList.Count : " + textList.Count);
        currRotateY = CV3.transform.eulerAngles.y;
        //Debug.Log(camera3RotateStart);
        if (camera3RotateStart)
        {
            Invoke(nameof(RotateCamera3), 4f);
        }
        else
        {
            CancelInvoke(nameof(RotateCamera3));
            blendList.m_Instructions[0].m_VirtualCamera = vCam3;
            blendList.m_Instructions[1].m_VirtualCamera = vCam2;
            blendList.m_Instructions[1].m_Blend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
            blendList.m_Instructions[1].m_Blend.m_Time = 2.0f;
            blendList.m_Instructions[0].m_Hold = 1.0f;
        }

        if (Input.GetKey(KeyCode.Tab))
        {
            SceneManager.LoadScene(sceneName);
        }

        //Debug.Log(blendList.m_Instructions[0].m_VirtualCamera.name);

        if (!camera3RotateStart && blendList.m_Instructions[0].m_VirtualCamera.name == "CM vcam3")
        {
            dateText.SetActive(false);

            if (isTextStartFuntion)
            {
                Invoke("TextStart", 2f);
            }
            else
            {
                CancelInvoke("TextStart");
            }


            if (isTextStart && Input.GetKeyDown(KeyCode.Space) && listNO <= 5)
            {
                ++listNO;
                if (listNO <= 4)
                {
                    text.text = textList[listNO];
                }
                else if (listNO == textList.Count)
                {
                    isEnterWalk = true;
                    isTextStart = false;
                }
            }

            if (isEnterWalk)
            {
                blendList.m_Instructions[0].m_VirtualCamera = vCam3;
                blendList.m_Instructions[1].m_VirtualCamera = vCam4;
                blendList.m_Instructions[1].m_Blend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
                blendList.m_Instructions[1].m_Blend.m_Time = 1.0f;
                blendList.m_Instructions[0].m_Hold = 1.0f;
                player.transform.position = Vector3.MoveTowards(player.transform.position, playerMovePosition.transform.position, Time.deltaTime * 4f);
                player.transform.LookAt(playerMovePosition.transform.position);
                walkAnimation.SetBool("isWalk", true);
                textCanvas.SetActive(false);

                if (Vector3.Distance(player.transform.position, playerMovePosition.transform.position) < 0.1f)
                {
                    walkAnimation.SetBool("isWalk", false);
                    fade.SetActive(true);
                    StartCoroutine("fadeCorutin");

                    if (alpha >= 3f)
                    {
                        StopAllCoroutines();
                        SceneManager.LoadScene(sceneName);
                    }
                }
            }
        }
    }

    private void RotateCamera3()
    {
        CV3.transform.RotateAround(building.transform.position, CV3.transform.up, Time.deltaTime * 50f);
        if (currRotateY >= 178f && currRotateY <= 179.9f)
        {
            camera3RotateStart = false;
        }
    }

    private void FirstChange()
    {
        blendList.m_Instructions[0].m_VirtualCamera = vCam1;
        blendList.m_Instructions[1].m_VirtualCamera = vCam3;
        blendList.m_Instructions[1].m_Blend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
        blendList.m_Instructions[1].m_Blend.m_Time = 2.0f;
        blendList.m_Instructions[0].m_Hold = 1.0f;
        camera3RotateStart = true;
    }

    private void TextStart()
    {
        textCanvas.SetActive(true);
        isTextStart = true;
        text.text = textList[listNO];
        isTextStartFuntion = false;
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