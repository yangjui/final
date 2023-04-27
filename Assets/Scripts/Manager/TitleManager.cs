using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private string sceneName = "Intro";
    [SerializeField]
    private GameObject helpTask = null;
    [SerializeField]
    private GameObject settingMenu;

    private void Awake()
    {
        helpTask.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ClickStart()
    {
        Debug.Log("�ε�");
        SceneManager.LoadScene(sceneName);
    }

    public void ClickHelp()
    {
        helpTask.SetActive(true);
        Debug.Log("���� ����");
    }

    public void ClickExitHelp()
    {
        helpTask.SetActive(false);
    }

    public void ClickExit()
    {
        Debug.Log("���� ����");
        Application.Quit();
    }

    public void SettingOn()
    {
        settingMenu.SetActive(true);
    }

    public void SettingOff()
    {
        settingMenu.SetActive(false);
    }

    public void ClickWeb()
    {
        Application.OpenURL("https://www.dureraum.org/bcc/main/main.do?rbsIdx=1");
    }

    public void FullScreen()
    {
        Screen.SetResolution(1920, 1080, true); // ��ü���
    }

    public void WindowScreen()
    {
        Screen.SetResolution(1280, 720, false); // ��ü���
    }
}