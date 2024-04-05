using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private Inventory inventory;
    public Slider healthBar;
    public Color lowHealth, highHealth;
    
    private Health playerHealth;
    private bool isPaused = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        inventory = player.GetComponent<Inventory>();
        playerHealth = player.GetComponent<Health>();
        
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "Health: " + playerHealth.currentHealth);
    }

    // Update is called once per frame
    void Update()
    {

        UpdateInventory();
        UpdateHealthBar();
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        

    }
    
    private void UpdateHealthBar()
    {
        healthBar.value = playerHealth.currentHealth;
        healthBar.maxValue = playerHealth.maxHealth;
        healthBar.fillRect.GetComponent<Image>().color = Color.Lerp(lowHealth, highHealth, healthBar.normalizedValue);
    }

    private void UpdateInventory()
    {
        foreach (GameObject icon in GameObject.FindGameObjectsWithTag("InventoryIcon"))
        {
            Destroy(icon.gameObject);
        }
        
        for (int i = 0; i < inventory.items.Count; i++)
        {
            var itemtuple = inventory.items[i];
            var item = itemtuple.item;
            var amount = itemtuple.amount;
            if (item.icon == null)
            {
                item.icon = GenerateIcon(item, i);
            }
            
            var icon = new GameObject();
            icon.layer = 5;
        
            // Add a SpriteRenderer component and assign the item's icon
            var image = icon.AddComponent<RawImage>();
            image.texture = item.icon.texture;
        
            // Set position, name, scale, and tag for the icon
            icon.transform.position = new Vector3(i * 150 + 100, 75, 0);
            icon.transform.parent = transform;
            icon.name = item.name;
            icon.tag = "InventoryIcon";
            
            var title = new GameObject();
            title.layer = 5;
            var text = title.AddComponent<TextMeshProUGUI>();
            text.text = $"{item.name} ({amount})";
            text.alignment = TextAlignmentOptions.Center;
            text.fontSize = 24;
            text.color = inventory.selectedIndex == i ? Color.red : Color.black;
            title.transform.parent = icon.transform;
            title.transform.position = new Vector3(icon.transform.position.x, icon.transform.position.y +62, 0);
            title.name = item.name + "Title";
            title.tag = "InventoryIcon";
            
        }
    }
    
    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        
        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            CreateButton((button => QuitGame()), "Quit");
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            var quitButton = GameObject.Find("QuitButton");
            if (quitButton != null)
            {
                Destroy(quitButton);
            }
        }
    }

    private void CreateButton( Action<Button> action, string name)
    {
        var button = new GameObject();
        button.layer = 5;
        var buttoncmpt = button.AddComponent<Button>();
        buttoncmpt.onClick.AddListener(() =>
        {
            action(buttoncmpt);
        });
        var text = button.AddComponent<TextMeshProUGUI>();
        text.text = name;
        text.fontSize = 24;
        button.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        button.transform.parent = transform;
        button.name = $"{name}Button";
    }

    private void QuitGame()
    {
        Time.timeScale = 1;
        GameManager.Instance.SavePlayerData();
        SceneManager.LoadScene(0);
    }

    private Sprite GenerateIcon(Item entity, int index)
    {
        if(entity.icon != null)
        {
            return entity.icon;
        }
    
        var camObject = new GameObject();
        var cam = camObject.AddComponent<Camera>();
        cam.name = "IconMakerCamera " + index;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.white;   
        cam.transform.position = new Vector3(index*100,500,0);

        GameObject item = Instantiate(entity.itemPrefab, cam.transform.position + cam.transform.forward * 2, Quaternion.identity);
        //Rotate to be angled in icon
        item.name = "IconMakerItem " + index;
        item.transform.Rotate(new Vector3(-20, 45, -20));
        var renderer = item.GetComponent<Renderer>();
        
    
    
    
        cam.orthographicSize = renderer != null ? renderer.bounds.extents.y + 0.1f : 1;
    
        //Get dimensions 
        int resX = cam.pixelWidth;
        int resY = cam.pixelHeight;

        int clipX = 0;
        int clipY = 0;
    
        if(resX > resY)
        {
            clipX = resX - resY ;
        }
        else if (resY > resX)
        {
            clipY = resY - resX ;
        }
    
    
        //Initialize everything
        Texture2D tex = new Texture2D(resX - clipX, resY -clipY, TextureFormat.RGBA32, false);
        RenderTexture rt = new RenderTexture(resX, resY, 24);
        cam.targetTexture = rt;
        RenderTexture.active = rt;
    
        cam.Render();
        tex.ReadPixels(new Rect(clipX/2, clipY/2, resX, resY), 0, 0);
        tex.Apply();
    
        cam.targetTexture = null;
        RenderTexture.active = null;

        foreach (Transform child in camObject.transform)
        {
            Destroy(child.gameObject);
        
        }
        Destroy(camObject.gameObject); 
        Destroy(item);
        var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
        entity.icon = sprite;
        return sprite;
    }
}
