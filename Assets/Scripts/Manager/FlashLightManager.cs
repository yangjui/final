using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightManager : MonoBehaviour
{
    [SerializeField]
    private SettingManager settingManager;
    [SerializeField]
    private BatteryManager batteryManager;

    [Header("Flashlight")]

    [SerializeField]
    private GameObject flashLight = null;
    [SerializeField]
    private GameObject flashLightSound = null;

    private bool isLightOn = true;

    [Space(10f)]
    [Header("UVLight")]

    [SerializeField]
    private GameObject uvLight = null;
    [System.NonSerialized]
    public bool isUVLightOn = false;

    [System.NonSerialized]
    public float uvUseTime = 0f;

    private void Awake()
    {
        flashLightSound.SetActive(false);
    }

    private void Update()
    {
        if (SettingManager.isSettingMenuAct) return;

        Light();

        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            isLightOn = !isLightOn;
            ToggleLight();
            flashLightSound.SetActive(false);
            flashLightSound.SetActive(true);
        }
        else if(Input.GetKeyUp(KeyCode.Keypad1) || Input.GetKeyUp(KeyCode.Alpha1))
        {
            flashLightSound.SetActive(false);
            flashLightSound.SetActive(true);
        }
    }

    private void Light()
    {
        if (!isLightOn) return;

        float wheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (wheelInput > 0 || !batteryManager.canUseUV)
        {
            flashLight.SetActive(true);
            uvLight.SetActive(false);
            isUVLightOn = false;
            flashLightSound.SetActive(false);
            flashLightSound.SetActive(true);
        }
        else if (wheelInput < 0 && batteryManager.canUseUV && batteryManager.uvCoolTime <= 0)
        {
            flashLight.SetActive(false);
            uvLight.SetActive(true);
            isUVLightOn = true;
            flashLightSound.SetActive(false);
            flashLightSound.SetActive(true);
        }
        else if (isUVLightOn)
        {
            uvUseTime += Time.deltaTime;
        }
    }

    public void ToggleLight()
    {
        if (isLightOn)
        {
            if (isUVLightOn)
            {
                uvLight.SetActive(true);
                flashLight.SetActive(false);
            }
            else
            {
                flashLight.SetActive(true);
                uvLight.SetActive(false);
            }
        }
        else
        {
            flashLight.SetActive(false);
            uvLight.SetActive(false);

            isUVLightOn = false;
        }
    }
}