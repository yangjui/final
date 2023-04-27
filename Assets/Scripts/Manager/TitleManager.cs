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
        Debug.Log("로딩");
        SceneManager.LoadScene(sceneName);
    }

    public void ClickHelp()
    {
        helpTask.SetActive(true);
        Debug.Log("도움말 슝슝");
    }

    public void ClickExitHelp()
    {
        helpTask.SetActive(false);
    }

    public void ClickExit()
    {
        Debug.Log("게임 종료");
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
        Screen.SetResolution(1920, 1080, true); // 전체모드
    }

    public void WindowScreen()
    {
        Screen.SetResolution(1280, 720, false); // 전체모드
    }
}