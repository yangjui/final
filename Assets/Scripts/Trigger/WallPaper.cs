using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPaper : MonoBehaviour
{
    public Texture2D newTexture;

    private Material originalMaterial;
    private Material newMaterial;
    private bool isPlayerNearby = false;

    private void Start()
    {
        // �ʱ� Material ����
        originalMaterial = GetComponent<Renderer>().material;
        // newMaterial = new Material(originalMaterial); // �� Material ����
        //originalMaterial.SetTexture("_MainTex", newTexture); // Texture ����
    }

    private void Update()
    {
        if (isPlayerNearby)
        {
            // ����� Material ����
            //GetComponent<Renderer>().material = newMaterial;
            originalMaterial.SetTexture("_MainTex", newTexture); // Texture ����

        }
        //else
        //{
        //    // �ʱ� Material ����
        //    GetComponent<Renderer>().material = originalMaterial;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}
