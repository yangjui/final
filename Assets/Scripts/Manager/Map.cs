using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    [SerializeField]
    private Player player = null;

    [SerializeField]
    private Image playerIcon = null;

    [SerializeField]
    private Sprite playerIconSprite = null;

    [SerializeField]
    private RectTransform mapTransform = null;



    private void Update()
    {
        Vector3 playerPosition = player.PlayerCurPosition();

        // 게임 월드 크기와 지도 이미지 크기를 설정
        Vector2 worldSize = GetWorldSize();
        Vector2 mapSize = GetMapSize();

        // 게임 월드 크기와 지도 이미지 크기의 비율을 계산하여 플레이어 위치를 조정
        float xRatio = mapSize.x / worldSize.x;
        float yRatio = mapSize.y / worldSize.y;

        float x = playerPosition.x * xRatio;
        float y = playerPosition.z * yRatio;

        // 플레이어 아이콘 위치 조정
        RectTransform iconTransform = playerIcon.rectTransform;
        iconTransform.anchoredPosition = new Vector2(x, y);
        iconTransform.sizeDelta = new Vector2(30, 30); // 아이콘 크기 조절
    }

    private Vector2 GetWorldSize()
    {
        // 게임 월드 크기를 가져옵니다. (프리팹으로 이용중인 맵의 크기를 가져옵니다.)
        Vector3 size = Vector3.zero;
        Transform[] transforms = mapTransform.GetComponentsInChildren<Transform>();
        foreach (Transform transform in transforms)
        {
            Renderer renderer = transform.GetComponent<Renderer>();
            if (renderer != null)
            {
                size = Vector3.Max(size, renderer.bounds.size);
            }
        }
        return new Vector2(size.x, size.z);
    }

    private Vector2 GetMapSize()
    {
        // UI Image 컴포넌트의 크기를 가져옵니다.
        RectTransform rectTransform = GetComponent<RectTransform>();
        return rectTransform.sizeDelta;
    }
}
