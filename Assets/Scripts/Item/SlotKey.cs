using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotKey : MonoBehaviour
{
    public Item item; // 획득한 아이템
    public int itemCount; // 획득한 아이템의 개수
    public Image itemImage;  // 아이템의 이미지

    // 아이템 이미지의 투명도 조절
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // 인벤토리에 새로운 아이템 슬롯 추가
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        SetColor(1);
    }

    // 해당 슬롯 하나 삭제
    public void RemoveItem()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);
    }
}