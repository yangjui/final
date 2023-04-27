using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject  // ���� ������Ʈ�� ���� �ʿ� X 
{
    public enum ItemType  // ������ ����
    {
        Used,
        RealKey,
        FakeKey,
        Symbol
    }

    public string itemName; // �������� �̸�
    public ItemType itemType; // ������ ����
    public Sprite itemImage; // �������� �̹���(�κ� �丮 �ȿ��� ���)
    public GameObject itemPrefab;  // �������� ������ (������ ������ ���������� ��)

    public string weaponType;  // ���� ����
}