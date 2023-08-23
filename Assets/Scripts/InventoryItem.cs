using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;

    [HideInInspector] public Image image;
    [HideInInspector] public Transform parentAfterDrag;

    public bool IsEquipped = false;

    public bool ShopItem = false;

    bool hovering = false;

    private void Start()
    {
        InitializeItem(item);
    }

    public void InitializeItem(Item newItem)
    {
        if (!ShopItem)
        {
            item = newItem;
        }
        image.sprite = newItem.itemIcon;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (ShopItem) { return; }
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (ShopItem) { return; }
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (ShopItem) { return; }
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (hovering)
        {
            if (ShopItem)
            {
                Inventory.Instance.AttemptTranzaction(true, item, gameObject);
            }
            else
            {
                if (Inventory.Instance.IsShopingTime)
                {
                    Inventory.Instance.AttemptTranzaction(false, item, gameObject);
                }
                
            }
        }

    }
}
