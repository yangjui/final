using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    [SerializeField]
    private GameObject settingMenu;

    [System.NonSerialized]
    static public bool isSettingMenuAct = false;

    [SerializeField]
    private List<GameObject> foots = new List<GameObject>();


    private void Start()
    {
        Resume();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isSettingMenuAct)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Resume()
    {
        settingMenu.SetActive(false);
        Time.timeScale = 1f;
        isSettingMenuAct = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        settingMenu.SetActive(true);
        Time.timeScale = 0f;
        isSettingMenuAct = true;
        for(int i = 0; i<foots.Count; ++i)
        {
            foots[i].SetActive(false);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
