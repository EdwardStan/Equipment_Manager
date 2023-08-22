using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerExitHandler, IPointerEnterHandler, IPointerDownHandler
{
    public ItemType SupportedItems = ItemType.None;

    bool hovering = false;

    public bool ShopSlot;

    public void OnDrop(PointerEventData eventData)
    {

       if(transform.childCount == 0 && SupportedItems == ItemType.None)
       {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
        }
        else if(SupportedItems != ItemType.None && eventData.pointerDrag.GetComponent<InventoryItem>().item.type != ItemType.None)
        {
            Debug.Log("ItemCompatible");
            if(transform.childCount > 0)
            {
                Transform itemToReplace = transform.GetChild(0);
                Inventory.Instance.AddItem(itemToReplace.GetComponent<InventoryItem>().item);
                Destroy(transform.GetChild(0).gameObject);
            }


            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
            inventoryItem.image.raycastTarget = false;
            EquipItem(inventoryItem);
        }
    }

    private void EquipItem(InventoryItem item)
    {
        Debug.Log("trying to equip " + item.name);
        switch (SupportedItems)
        {
            case ItemType.Head:
                Debug.Log("Equip for head");
                PlayerMovement.Instance.EquippedHead = item;
                PlayerMovement.Instance.UpdateAppearance();
                break;
            case ItemType.Body:
                Debug.Log("Equip for body");
                PlayerMovement.Instance.EquippedBody = item; 
                PlayerMovement.Instance.UpdateAppearance();
                break; 
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("hovering");
        hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("not hovering");
        hovering = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(transform.childCount == 0) { return; }

        if (hovering && !ShopSlot)
        {
            if (SupportedItems != ItemType.None)
            {
                Transform itemToReplace = transform.GetChild(0);
                Inventory.Instance.AddItem(itemToReplace.GetComponent<InventoryItem>().item);
                Destroy(transform.GetChild(0).gameObject);
            }

            if(SupportedItems == ItemType.Head)
            {
                PlayerMovement.Instance.EquippedHead = null;
            }
            else if (SupportedItems == ItemType.Body)
            {
                PlayerMovement.Instance.EquippedBody = null;
            }
         
            PlayerMovement.Instance.UpdateAppearance();
        }
    }
}
