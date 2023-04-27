using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Table_drawerOpen : MonoBehaviour
{

    [SerializeField]
    private Text openText = null;

    [SerializeField]
    private GameObject openTextUI = null;

    private Color color;

    private AudioSource openSound;

    private bool isCanOpen = false;

    private void Awake()
    {
        openText.text = "서랍 열기 " + "<color=yellow>" + "[X]" + "</color>";
        openTextUI.SetActive(false);
        color = Color.red;
        openSound = GetComponent<AudioSource>();
        openSound.enabled = false;
    }


    private void Update()
    {
        if (!isCanOpen) return;

        if (Input.GetKeyDown(KeyCode.X))
        {
            OpenDrawer();
        }
    }
    private void OnTriggerStay(Collider _other)
    {
        if (!_other.CompareTag("Player"))
        {
            color = Color.red;
            return;
        }
        openSound.enabled = true;
        isCanOpen = true;
        openTextUI.SetActive(true);
        color = Color.green;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawWireCube(transform.position + transform.forward * 2f - transform.up, new Vector3(2f, 2f, 2f));
    }


    private void OnTriggerExit(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            color = Color.red;
            openTextUI.SetActive(false);
            isCanOpen = false;
        }
    }

    private void OpenDrawer()
    {
        Vector3 localPosition = transform.localPosition;
        openSound.Play();
        localPosition.z = 1.5f;
        transform.localPosition = localPosition;
        openTextUI.SetActive(false);
        Invoke("DestroyAll", 0.2f);
    }

    private void DestroyAll()
    {
        Destroy(openSound);
        openTextUI.SetActive(false);
        Destroy(transform.GetComponent<Table_drawerOpen>());
    }
}