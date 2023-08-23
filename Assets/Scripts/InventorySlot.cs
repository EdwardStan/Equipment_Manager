using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerExitHandler, IPointerEnterHandler, IPointerDownHandler
{
    public ItemType SupportedItems = ItemType.None;

    bool hovering = false;

    public bool ShopSlot;

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem dropedItem = eventData.pointerDrag.GetComponent<InventoryItem>();

        if (transform.childCount == 0 && SupportedItems == ItemType.None)
        {
            /*InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();*/
            dropedItem.parentAfterDrag = transform;
        }
        if (SupportedItems != ItemType.None && dropedItem.item.type != ItemType.None && !dropedItem.IsEquipped)
        {
            Debug.Log("ItemCompatible");
            if (transform.childCount > 0)
            {
                Transform itemToReplace = transform.GetChild(0);
                Inventory.Instance.AddItem(itemToReplace.GetComponent<InventoryItem>().item);
                Destroy(transform.GetChild(0).gameObject);
            }


            /*InventoryItem inventoryItem = dropedItem;*/
            dropedItem.parentAfterDrag = transform;
            dropedItem.image.raycastTarget = false;
            dropedItem.IsEquipped = true;
            EquipItem(dropedItem);
        }
        else if (dropedItem.IsEquipped && SupportedItems == ItemType.None)
        {
            dropedItem.parentAfterDrag = transform;
            dropedItem.image.raycastTarget = false;
            dropedItem.IsEquipped = false;
            if (dropedItem.item.type == ItemType.Head) { PlayerMovement.Instance.EquippedHead = null; }
            if (dropedItem.item.type == ItemType.Body) { PlayerMovement.Instance.EquippedBody = null; }

            PlayerMovement.Instance.UpdateAppearance();
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
        if (transform.childCount == 0) { return; }

        if (hovering && !ShopSlot)
        {
            if (SupportedItems != ItemType.None)
            {
                Transform itemToReplace = transform.GetChild(0);
                Inventory.Instance.AddItem(itemToReplace.GetComponent<InventoryItem>().item);
                Destroy(transform.GetChild(0).gameObject);
            }

            if (SupportedItems == ItemType.Head)
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
