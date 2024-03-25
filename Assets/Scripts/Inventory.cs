using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<(int amount, Item item)> items = new List<(int amount, Item item)>();
    public int space = 20;

    public Item testItem;
    public int money = 0, xp = 0;
    
    public int selectedIndex = 0;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Add(testItem);
        
    }

    private void OnGUI()
    {
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;
        GUI.Label(new Rect(screenWidth - 100, 10, 100, 20), "Money: " + money);
        GUI.Label(new Rect(screenWidth - 200, 10, 100, 20), "XP: " + xp);
    }

    public bool Add(Item item)
    {
        if (items.Count >= space)
        {
            Debug.Log("Not enough room.");
            return false;
        }
        
        if(Contains(item))
        {
            var itemTuple = GetItem(item.name);
            if (item.maxAmount > itemTuple.amount)
            {
                ModifyAmount(item, 1);
                return true;
            }
            else
            {
                Debug.Log("Item is at max amount.");
                return false;
            }
            
        }
        
        var itemCopy = item.Copy();
        
        var itemTupleCopy = (1, itemCopy);
        
        items.Add(itemTupleCopy);
        return true;
    }
    
    public void AddMoney(int amount)
    {
        money += amount;
    }
    
    public bool RemoveMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            return true;
        }
        return false;
    }
    
    public void AddXP(int amount)
    {
        xp += amount;
    }
    
    public void Remove(Item item)
    {
        if (Contains(item))
        {
            var itemTuple = GetItem(item.name);
            if (itemTuple.amount > 1)
            {
                ModifyAmount(item, -1);
            }
            else
            {
                items.Remove(GetItem(item.name));
            }
        }
    }
    
    public void RemoveAt(int index)
    {
        items.RemoveAt(index);
    }
    
    public bool Contains(Item item)
    {
        return items.Select(i => i.item.name).Contains(item.name);
    }
    
    public bool Contains(string name)
    {
        return items.Select(i => i.item.name).Contains(name);
    }
    
    public (int amount,Item item) GetItem(string name)
    {
        return items.FirstOrDefault(i => i.item.name == name);
    }
    
    private void ModifyAmount(Item item, int amount)
    {
        for(int i = 0; i < items.Count; i++)
        {
            if (items[i].item.name == item.name)
            {
                items[i] = (items[i].amount + amount, items[i].item);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        // Change index with mouse wheel
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            selectedIndex++;
            if (selectedIndex >= items.Count)
            {
                selectedIndex = 0;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            selectedIndex--;
            if (selectedIndex < 0)
            {
                selectedIndex = items.Count - 1;
            }
        }
        
        for (int i = 0; i < items.Count; i++)
        {
            if (Input.GetKeyDown((i+1).ToString()))
            {
                selectedIndex = i;
            }
        }
        
        
        if(Input.GetMouseButtonDown(0))
        {
            if (items.Count > 0)
            {
                var item = items[selectedIndex];
                if (item.item.itemType == Item.ItemType.Food)
                {
                    var health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
                    health.Heal(10);
                    item.amount--;
                    if (item.amount <= 0)
                    {
                        items.Remove(item);
                    }
                }
            }
        }

        
        
    }
}
