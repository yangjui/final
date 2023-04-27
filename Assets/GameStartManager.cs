using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartManager : MonoBehaviour
{
    [SerializeField]
    private GameObject fade = null;

    [SerializeField]
    private GameObject textUI = null;

    [SerializeField]
    private Text text = null;

    [SerializeField]
    private Player player = null;
    [SerializeField]
    private CameraManager cameraManager = null;
    [SerializeField]
    private ChaserAI chaser = null;
    [SerializeField]
    private GameObject mapManager = null;

    private bool isTextStart = false;
    private int listNO = 0;
    private List<string> textList = new List<string>();
    string no1 = "괴물이 나온다는 우려와 달리 무사히 야간 순찰을 끝낼 수 있었다.";
    string no2 = "순찰을 하면서 알게 된 것이 몇 가지 있다.";
    string no3 = "1. 'W, A, S, D'를 이용하여 몸을 움직을 수 있고 'shift 키' 빠르게 달릴 수 있다.";
    string no4 = "2. '마우스'를 이용하여 내가 원하는 방향을 볼 수 있다.";
    string no5 = "3. '키보드 1번 키'를 이용하여 손전등을 끄고 킬 수 있으며 마우스 휠을 '내리면' UV 라이트를 킬 수 있고, 마우스 휠을 '올리면' 일반 라이트로 돌아온다.";
    string no6 = "4. 'M키'를 누르면 지도를 끄고 킬 수 있다.";
    string no7 = "그렇게 내가 알게 된 내용을 정리하며 사무실로 돌아오던 중에 갑자기 불이 꺼지고 나는 보고 말았다. 괴물을...";
    string no8 = "다행히도 괴물에게 들키지 않고 무사히 경비실로 돌아올 수 있었다. 하지만 한시바삐 이곳을 탈출해야 한다.";
    string no9 = "'카드 키'를 이용해서 사무실 옆 정문을 통해 탈출할 수 있는데 주간 근무자가 카드 키를 잃어버렸다고 한다.";
    string no10 = "'카드 키'는 보안을 위해 진짜 와 가짜가 있는데 이 중에 진짜 키를 괴물에게 들키지 않고 찾아 무사히 살아나가자";

    private float alpha = 2f;

    private void Awake()
    {
        fade.SetActive(true);
        fade.GetComponent<Image>().color = new Color(0, 0, 0, 1);
        player.enabled = false;
        cameraManager.enabled = false;
        chaser.enabled = false;
        player.GetComponent<Animator>().enabled = false;
        textUI.SetActive(false);
        textList.Add(no1);
        textList.Add(no2);
        textList.Add(no3);
        textList.Add(no4);
        textList.Add(no5);
        textList.Add(no6);
        textList.Add(no7);
        textList.Add(no8);
        textList.Add(no9);
        textList.Add(no10);
        text.text = textList[listNO];
        Time.timeScale = 0f;
    }

    private void Start()
    {
        StartCoroutine(fadeCorutin());
    }

    private void Update()
    {
        Debug.Log(alpha);
        if (alpha <= 0.7f)
        {
            StopAllCoroutines();
            isTextStart = true;
            Destroy(fade);
        }

        if (Input.GetKey(KeyCode.Tab))
        {
            Destroy(textUI);
            player.enabled = true;
            cameraManager.enabled = true;
            chaser.enabled = true;
            player.GetComponent<Animator>().enabled = true;
            Destroy(gameObject);
            isTextStart = false;
        }

        if (isTextStart)
        {
            textUI.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Space) && listNO <= 10)
            {

                ++listNO;
                if (listNO <= 9)
                {
                    text.text = textList[listNO];
                }
                else if (listNO == textList.Count)
                {
                    Destroy(textUI);
                    player.enabled = true;
                    cameraManager.enabled = true;
                    chaser.enabled = true;
                    player.GetComponent<Animator>().enabled = true;
                    Destroy(gameObject);
                    isTextStart = false;
                }
            }
        }
    }

    private IEnumerator fadeCorutin()
    {
        while (alpha >= 0.7f)
        {
            alpha -= Time.deltaTime;
            fade.GetComponent<Image>().color = new Color(0, 0, 0, alpha);

            yield return null;
        }
    }
}
