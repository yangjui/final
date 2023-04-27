using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private SettingManager settingManager;
    [SerializeField]
    private GameOverManager gameOverManager;

    [SerializeField]
    private GameObject Map;

    private bool mapActivated = false;

    private void Awake()
    {
        CloseMap();
    }

    private void Update()
    {
        if (SettingManager.isSettingMenuAct) return;
        if (gameOverManager.isDead) return;

        TryOpenMap();
    }

    private void TryOpenMap()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            mapActivated = !mapActivated;

            if (mapActivated)
            {
                OpenMap();
            }
            else
            {
                CloseMap();
            }
        }
    }

    private void OpenMap()
    {
        Map.SetActive(true);
    }

    private void CloseMap()
    {
        Map.SetActive(false);
    }
}