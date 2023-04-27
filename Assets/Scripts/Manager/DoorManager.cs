using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DoorManager : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private Text openText = null;

    [SerializeField]
    private List<GameObject> doorList = new List<GameObject>();

    [SerializeField]
    private GameObject openTextUI = null;


    private GameObject currentDoor = null;

    public bool isCanOpen = false;

    private void Awake()
    {
        openText.text = "¹® ¿­±â " + "<color=yellow>" + "[X]" + "</color>";
        openTextUI.SetActive(false);
    }

    private void Update()
    {
        for(int i = 0; i<doorList.Count; ++i)
        {
            if (player.rayString == doorList[i].name)
            {
                openTextUI.SetActive(true);
                isCanOpen = true;
                currentDoor = doorList[i];
            }
        }

        if (Input.GetKey(KeyCode.X) && isCanOpen)
        {
            currentDoor.GetComponent<Animator>().SetBool("isOpen", true);
            openTextUI.SetActive(false);
            currentDoor = null;
            isCanOpen = false; 
        }

        if(!isCanOpen)
        {
            openTextUI.SetActive(false);
        }
    }

}
