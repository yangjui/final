using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnHandManager : MonoBehaviour
{
    [SerializeField]
    private Slot slot;
    [SerializeField]
    private SettingManager settingManager;

    [System.NonSerialized]
    static public GameObject go = null;
    [System.NonSerialized]
    static public bool isFireOnHand = false;

    [SerializeField]
    private GameObject equipPoint = null;
    [SerializeField]
    private GameObject fireExtinguisher = null;

    private void Start()
    { 
        isFireOnHand = false;
    }

    private void Update()
    {
        if (SettingManager.isSettingMenuAct) return;

        EquipFire();
    }

    public void EquipFire()
    {
        if ((Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2)) && slot.itemCount != 0 && !isFireOnHand) // 장착 개념
        {
            go = Instantiate(fireExtinguisher, equipPoint.transform); // 공 생성, 위치는 정해져있는 위치. 플레이어의 손 끝
            go.GetComponent<Rigidbody>().isKinematic = true;
            go.GetComponent<Collider>().isTrigger = true;

            isFireOnHand = true;
            slot.RemoveItem();
        }
    }
}