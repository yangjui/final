using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [Header("Script")]

    [SerializeField]
    private InventoryManager inventoryManager;
    [SerializeField]
    private SettingManager settingManager;
    [SerializeField]
    private Player player;
    [SerializeField]
    private Slot slot;
    [SerializeField]
    private SlotKey slotKey;
    [SerializeField]
    private SlotSymbol slotSymbol;

    [Space(10f)]
    [Header("Inventory")]

    [SerializeField]
    private float itemMaxRange = 5f;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private Text actionText;

    private bool pickupActivated = false;
    private RaycastHit hitInfo;

    [System.NonSerialized]
    public bool isRealKeyOnHand = false;
    [System.NonSerialized]
    public bool isKeyOnHand = false;
    [System.NonSerialized]
    public bool isSymbolOnHand = false;

    [SerializeField]
    private AudioSource itemPickUpSound = null;

    private void Update()
    {
        if (SettingManager.isSettingMenuAct) return;

        CheckItem();

        if (Input.GetKeyDown(KeyCode.F))
        {
            CanPickUp();
        }
    }

    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, itemMaxRange, layerMask))
        {
            if (hitInfo.transform.CompareTag("Item"))
            {
                ItemInfoAppear();
            }
        }
        else
        {
            ItemInfoDisappear();
        }
    }

    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " È¹µæ " + "<color=yellow>" + "[F]" + "</color>";
    }

    private void ItemInfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
        player.isSoundOn = false;
    }

    private void CanPickUp()
    {
        if (!pickupActivated) return;

        var hitTransform = hitInfo.transform;
        if (hitTransform == null) return;

        var itemPickUp = hitTransform.GetComponent<ItemPickUp>();
        Item.ItemType itemType = itemPickUp.item.itemType;

        switch (itemType)
        {
            case Item.ItemType.FakeKey:
            case Item.ItemType.RealKey:
                if (slotKey.itemCount == 0)
                {
                    inventoryManager.AcquireItemKey(itemPickUp.item);
                    isKeyOnHand = true;
                    if (itemType == Item.ItemType.RealKey)
                    {
                        isRealKeyOnHand = true;
                    }
                    player.isSoundOn = true;
                    itemPickUpSound.Play();
                    Destroy(hitTransform.gameObject);
                }
                break;
            case Item.ItemType.Used:
                if (slot.itemCount == 0)
                {
                    inventoryManager.AcquireItem(itemPickUp.item);
                    itemPickUpSound.Play();
                    Destroy(hitTransform.gameObject);
                }
                break;
            case Item.ItemType.Symbol:
                if (slotSymbol.itemCount == 0)
                {
                    isSymbolOnHand = true;
                    inventoryManager.AcquireItemSymbol(itemPickUp.item);
                    itemPickUpSound.Play();
                    Destroy(hitTransform.gameObject);
                }
                break;
            default:
                break;

        }


    }
}