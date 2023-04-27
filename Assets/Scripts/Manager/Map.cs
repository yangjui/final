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

        // ���� ���� ũ��� ���� �̹��� ũ�⸦ ����
        Vector2 worldSize = GetWorldSize();
        Vector2 mapSize = GetMapSize();

        // ���� ���� ũ��� ���� �̹��� ũ���� ������ ����Ͽ� �÷��̾� ��ġ�� ����
        float xRatio = mapSize.x / worldSize.x;
        float yRatio = mapSize.y / worldSize.y;

        float x = playerPosition.x * xRatio;
        float y = playerPosition.z * yRatio;

        // �÷��̾� ������ ��ġ ����
        RectTransform iconTransform = playerIcon.rectTransform;
        iconTransform.anchoredPosition = new Vector2(x, y);
        iconTransform.sizeDelta = new Vector2(30, 30); // ������ ũ�� ����
    }

    private Vector2 GetWorldSize()
    {
        // ���� ���� ũ�⸦ �����ɴϴ�. (���������� �̿����� ���� ũ�⸦ �����ɴϴ�.)
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
        // UI Image ������Ʈ�� ũ�⸦ �����ɴϴ�.
        RectTransform rectTransform = GetComponent<RectTransform>();
        return rectTransform.sizeDelta;
    }
}
