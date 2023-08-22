using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    bool inventoryOpened = false;
    
    public Transform InventoryPanel;
    public Transform ShopPanel;
    public GameObject EquipamentSubsection;

    public GameObject inventoryItemPrefab;
    public InventorySlot[] inventorySlots;

    int playerGold = 0;

    public TMP_Text goldText;

    public bool IsShopingTime = false;

    public GameObject AttemptScreen;
    public TMP_Text AttemptText;
    public TMP_Text AttemptedItemName;
    public TMP_Text AttemptedPrice;
    public Image AttemptedImage;
    public Button AttemptSuccessButton;


    private void Awake()
    {
        playerGold = 0;
        UpdateUI();

        Instance = this;
    }

    public void AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return;
            }
        }
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemObj = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemObj.GetComponentInChildren<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }

    public void OpenInventory(bool isShop)
    {
        EquipamentSubsection.SetActive(!isShop);
        float targetPosition = inventoryOpened ? -2000f : -500f;
        inventoryOpened = !inventoryOpened;
        IsShopingTime = false;

        InventoryPanel.LeanMoveLocalX(targetPosition, 1f);

        if (isShop)
        {
            IsShopingTime = true;
            ShopPanel.LeanMoveLocalX(-targetPosition, 1f);
        }

    }

    public void AttemptTranzaction(bool buy, Item item, GameObject objToSell)
    {
        AttemptScreen.SetActive(true);
        AttemptText.text = buy ? "BUY" : "SELL";
        AttemptedItemName.text = item.name;
        AttemptedPrice.text = item.Cost.ToString();
        AttemptedImage.sprite = item.itemIcon;

        AttemptSuccessButton.onClick.RemoveAllListeners();
        AttemptSuccessButton.onClick.AddListener(buy ? () => PurchaseItem(item) : () => SellItem(item, objToSell));
    }

    public void SellItem(Item item, GameObject objToSell)
    {
        Debug.Log("Item Sold");
        Destroy(objToSell);
        GetGold(item.Cost);
        AttemptScreen.SetActive(false);
    }

    public void PurchaseItem(Item item)
    {
        Debug.Log("Item Purchased");
        if (item.Cost <= playerGold)
        {
            AddItem(item);
            ConsumeGold(item.Cost);
            AttemptScreen.SetActive(false);
        }
        else
        {
            Debug.Log("Not Enough Gold");
        }
    }

    void UpdateUI()
    {
        goldText.text = playerGold.ToString();
    }

    public void GetGold(int goldAmount)
    {
        playerGold = playerGold + goldAmount;
        UpdateUI();
    }

    public void ConsumeGold(int goldAmount)
    {
        playerGold = playerGold - goldAmount;
        UpdateUI();
    }

}
