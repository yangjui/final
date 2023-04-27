using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private GameObject slotsParent;  // Slot���� �θ��� Grid Setting 

    private Slot[] slots;  // ���Ե� �迭
    private SlotKey[] slotsKey;  // ���Ե� �迭
    private SlotSymbol[] slotsSymbol;  // ���Ե� �迭

    private void Start()
    {
        slots = slotsParent.GetComponentsInChildren<Slot>();
        slotsKey = slotsParent.GetComponentsInChildren<SlotKey>();
        slotsSymbol = slotsParent.GetComponentsInChildren<SlotSymbol>();
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        for (int i = 0; i < slots.Length; ++i)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }

    public void AcquireItemKey(Item _item, int _count = 1)
    {
        for (int i = 0; i < slotsKey.Length; ++i)
        {
            if (slotsKey[i].item == null)
            {
                slotsKey[i].AddItem(_item, _count);
                return;
            }
        }
    }

    public void AcquireItemSymbol(Item _item, int _count = 1)
    {
        for (int i = 0; i < slotsSymbol.Length; ++i)
        {
            if (slotsSymbol[i].item == null)
            {
                slotsSymbol[i].AddItem(_item, _count);
                return;
            }
        }
    }
}