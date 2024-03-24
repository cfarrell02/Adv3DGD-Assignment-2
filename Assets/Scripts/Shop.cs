using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ShopItem
{
    public int amount;
    public Item item;
}

public class Shop : MonoBehaviour
{
    public List<Item> allItems = new List<Item>();
    
    bool isPlayerNear = false, shopOpen = false;
    public List<ShopItem> stock = new List<ShopItem>();
    private Inventory playerInventory;
    private GameObject player;
    public int selectedIndex = 0;
    
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = player.GetComponent<Inventory>();
        
    }
    
    private void OnGUI()
    {
        if (isPlayerNear && shopOpen)
        {
            GUI.Box(new Rect(80, 100, 400, 500), "Shop");
            //Instructions
            GUI.Label(new Rect(100, 120, 400, 20), "Press E to leave shop");
            GUI.Label(new Rect(100, 140, 400, 20), "Press T and G to navigate");
            GUI.Label(new Rect(100, 160, 400, 20), "Press Enter to buy");
            GUI.Label(new Rect(100, 180, 400, 20), "Press Q to sell");
            
            for (int i = 0; i < allItems.Count; i++)
            {
                var item = allItems[i];
                //Check store stock of it and player inventory stock of it
                var stockItem = stock.Find(x => x.item.name == item.name);
                var playerItem = playerInventory.GetItem(item.name);
                
                string label = $"{item.name} - {item.price} ----- We have {stockItem.amount} ----- You have {playerItem.amount}";
                if (selectedIndex == i)
                {
                    label += " <---";
                }
                GUI.Label(new Rect(100, 200 + i * 20, 400, 20), label);
            }
        }
        else if (isPlayerNear)
        {
            GUI.Label(new Rect(100, 200, 100, 20), "Press E to open shop");
        } 
    }
    
    

    // Update is called once per frame
    void Update()
    {
        isPlayerNear = Vector3.Distance(player.transform.position, transform.position) < 5;
        
        if(!isPlayerNear)
        {
            shopOpen = false;
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            shopOpen = !shopOpen;
        }
        
        if(!shopOpen)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.T ))
        {
            selectedIndex--;
            if (selectedIndex < 0)
            {
                selectedIndex = allItems.Count - 1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            selectedIndex++;
            if (selectedIndex >= allItems.Count)
            {
                selectedIndex = 0;
            }
        }
        
        
        
    }
    
    
}
