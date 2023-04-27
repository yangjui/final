using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryManager : MonoBehaviour
{
    [SerializeField]
    private SettingManager SettingManager;
    [SerializeField]
    private FlashLightManager flashLightManager;

    [SerializeField]
    private Sprite[] sprites = new Sprite[5];
    [SerializeField]
    private GameObject chargeImg = null;

    [System.NonSerialized]
    public bool canUseUV = true;
    [System.NonSerialized]
    public float uvCoolTime = 5f;

    private Image batteryImg;

    private void Awake()
    {
        batteryImg = GetComponent<Image>();
        chargeImg.SetActive(false);
        uvCoolTime = 0f;
    }

    private void Update()
    {
        if (SettingManager.isSettingMenuAct) return;

        BatteryState();
    }

    private void BatteryState()
    {
        if (flashLightManager.isUVLightOn)
        {
            chargeImg.SetActive(false);

            switch (flashLightManager.uvUseTime)
            {
                case >= 0 and <= 1:
                    batteryImg.sprite = sprites[0];
                    uvCoolTime = flashLightManager.uvUseTime;
                    break;
                case > 1 and <= 2:
                    batteryImg.sprite = sprites[1];
                    uvCoolTime = flashLightManager.uvUseTime;
                    break;
                case > 2 and <= 3:
                    batteryImg.sprite = sprites[2];
                    uvCoolTime = flashLightManager.uvUseTime;
                    break;
                case > 3 and <= 4:
                    batteryImg.sprite = sprites[3];
                    uvCoolTime = flashLightManager.uvUseTime;
                    break;
                case > 4:
                    batteryImg.sprite = sprites[4];
                    uvCoolTime = 5;
                    canUseUV = false;
                    break;
                default:
                    return;
            }
        }

        else
        {
            uvCoolTime -= Time.deltaTime;
            switch (uvCoolTime)
            {
                case <= 5 and > 4:
                    batteryImg.sprite = sprites[4];
                    chargeImg.SetActive(true);
                    break;
                case <= 4 and > 3:
                    batteryImg.sprite = sprites[3];
                    chargeImg.SetActive(true);
                    break;
                case <= 3 and > 2:
                    batteryImg.sprite = sprites[2];
                    chargeImg.SetActive(true);
                    break;
                case <= 2 and > 1:
                    batteryImg.sprite = sprites[1];
                    chargeImg.SetActive(true);
                    break;
                case <= 1 and > 0:
                    batteryImg.sprite = sprites[0];
                    chargeImg.SetActive(true);
                    break;
                case <= 0:
                    canUseUV = true;
                    flashLightManager.uvUseTime = 0;
                    batteryImg.sprite = sprites[0];
                    chargeImg.SetActive(false);
                    break;
                default:
                    return;
            }
        }
    }
}