using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    // This class is used to create items that can be stored in the inventory
    public enum ItemType
    {
        Loot,
        Weapon,
        Money,
        Food
    }
    
    public string name, description;
    public ItemType itemType;
    public int maxAmount, price;
    public GameObject itemPrefab;
    public Sprite icon;
    
    
    public Item(string name, string description, ItemType itemType, int maxAmount, int price)
    {
        this.name = name;
        this.description = description;
        this.itemType = itemType;
        this.maxAmount = maxAmount;
        this.price = price;
    }
    
    public Item()
    {
        this.name = "Item";
        this.description = "This is an item";
        this.itemType = ItemType.Loot;
        this.maxAmount = 1;
        this.price = 0;
    }
    
    public Item Copy()
    {
        var item = ScriptableObject.CreateInstance<Item>();
        item.name = name;
        item.description = description;
        item.itemType = itemType;
        item.maxAmount = maxAmount;
        item.price = price;
        item.itemPrefab = itemPrefab;
        item.icon = icon;
        return item;
    }
    
}


