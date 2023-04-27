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
    string no1 = "������ ���´ٴ� ����� �޸� ������ �߰� ������ ���� �� �־���.";
    string no2 = "������ �ϸ鼭 �˰� �� ���� �� ���� �ִ�.";
    string no3 = "1. 'W, A, S, D'�� �̿��Ͽ� ���� ������ �� �ְ� 'shift Ű' ������ �޸� �� �ִ�.";
    string no4 = "2. '���콺'�� �̿��Ͽ� ���� ���ϴ� ������ �� �� �ִ�.";
    string no5 = "3. 'Ű���� 1�� Ű'�� �̿��Ͽ� �������� ���� ų �� ������ ���콺 ���� '������' UV ����Ʈ�� ų �� �ְ�, ���콺 ���� '�ø���' �Ϲ� ����Ʈ�� ���ƿ´�.";
    string no6 = "4. 'MŰ'�� ������ ������ ���� ų �� �ִ�.";
    string no7 = "�׷��� ���� �˰� �� ������ �����ϸ� �繫�Ƿ� ���ƿ��� �߿� ���ڱ� ���� ������ ���� ���� ���Ҵ�. ������...";
    string no8 = "�������� �������� ��Ű�� �ʰ� ������ ���Ƿ� ���ƿ� �� �־���. ������ �ѽùٻ� �̰��� Ż���ؾ� �Ѵ�.";
    string no9 = "'ī�� Ű'�� �̿��ؼ� �繫�� �� ������ ���� Ż���� �� �ִµ� �ְ� �ٹ��ڰ� ī�� Ű�� �Ҿ���ȴٰ� �Ѵ�.";
    string no10 = "'ī�� Ű'�� ������ ���� ��¥ �� ��¥�� �ִµ� �� �߿� ��¥ Ű�� �������� ��Ű�� �ʰ� ã�� ������ ��Ƴ�����";

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
