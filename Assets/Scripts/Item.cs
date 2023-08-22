using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Item")]
public class Item : ScriptableObject
{
    public int ID;
    public int Cost;
    public Sprite itemIcon;
    public ItemType type;
    public bool stackable = false;
}

public enum ItemType
{
    None,
    Head,
    Body
}
