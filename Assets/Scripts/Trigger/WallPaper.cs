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
        // 초기 Material 저장
        originalMaterial = GetComponent<Renderer>().material;
        // newMaterial = new Material(originalMaterial); // 새 Material 생성
        //originalMaterial.SetTexture("_MainTex", newTexture); // Texture 적용
    }

    private void Update()
    {
        if (isPlayerNearby)
        {
            // 변경된 Material 적용
            //GetComponent<Renderer>().material = newMaterial;
            originalMaterial.SetTexture("_MainTex", newTexture); // Texture 적용

        }
        //else
        //{
        //    // 초기 Material 복원
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
