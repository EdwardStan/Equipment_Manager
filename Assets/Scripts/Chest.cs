using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    Animator anim;

    bool opened = false;

    [SerializeField] int goldAmount;
    [SerializeField] Item[] itemContent;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OpenChest()
    {
        if (opened) { return; }

        opened = true;
        Debug.Log("You opened a chest and gained " + goldAmount + " gold");
        anim.SetBool("Opened", true);
        Inventory.Instance.GetGold(goldAmount);
        GivePlayerItem();
    }

    void GivePlayerItem()
    {
        if(!(itemContent.Length > 0)) { return; }

        foreach (Item item in itemContent)
        {
            Inventory.Instance.AddItem(item);
        }
    }
}
