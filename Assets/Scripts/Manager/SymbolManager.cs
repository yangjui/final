using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymbolManager : MonoBehaviour
{
    [SerializeField]
    private SettingManager settingManager;
    [SerializeField]
    private ActionController actionController;
    [SerializeField]
    private Player player;
    [SerializeField]
    private SlotSymbol slotSymbol;
    [SerializeField]
    private GameObject symbol;

    [SerializeField]
    private Text openText = null;

    public bool sealSuccess = false;

    private void Awake()
    {
        openText.gameObject.SetActive(false);
        symbol.SetActive(false);
    }

    private void Update()
    {
        if (SettingManager.isSettingMenuAct) return;

        if (actionController.isSymbolOnHand)
        {
            PlayerRayForAltar();
        }
    }

    private void PlayerRayForAltar()
    {
        if (player.rayString == gameObject.name)
        {
            openText.gameObject.SetActive(true);
            openText.text = " ∫¿¿Œ " + "<color=yellow>" + "[X]" + "</color>";
            StatueSeal();
        }
        else
        {
            openText.gameObject.SetActive(false);
        }
    }

    private void StatueSeal()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (actionController.isSymbolOnHand)
            {
                actionController.isSymbolOnHand = false;
                symbol.SetActive(true);
                sealSuccess = true;
                player.isSoundOn = true;
            }
            slotSymbol.RemoveItem();
            openText.gameObject.SetActive(false);
        }
    }
}