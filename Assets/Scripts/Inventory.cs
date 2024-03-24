using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<(int amount, Item item)> items = new List<(int amount, Item item)>();
    public int space = 20;

    public Item testItem;
    
    public int selectedIndex = 0;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Add(testItem);
        
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
                itemTuple.amount++;
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
    
    public void Remove(Item item)
    {
        if (Contains(item))
        {
            var itemTuple = GetItem(item.name);
            if (itemTuple.amount > 1)
            {
                itemTuple.amount--;
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
    
    public (int amount,Item item) GetItem(string name)
    {
        return items.FirstOrDefault(i => i.item.name == name);
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
