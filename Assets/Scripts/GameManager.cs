using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    private static GameManager _instance;

    // Public property to access the instance
    //Singleton taken from stack overflow
    public static GameManager Instance
    {
        get
        {
            // If the instance doesn't exist, create one
            if (_instance == null)
            {
                GameObject singletonObject = new GameObject("GameManager");
                _instance = singletonObject.AddComponent<GameManager>();
            }

            return _instance;
        }
    }
    
    private void Awake()
    {
        // Ensure there is only one instance of the GameManager
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);


        }
    }


    private string loginUrl = "https://advanced3dgd.000webhostapp.com/ca2-login.php";
    private string updateUrl = "https://advanced3dgd.000webhostapp.com/ca2-update-user.php";
    public Item[] allItems;

    
    public void Login(string name, string password, Action<bool, string> callback)
    {
        StartCoroutine(SendLogin(name, password, callback));
    }
    
    private IEnumerator SendLogin(string name, string password, Action<bool, string> callback)
    {
        string urlToSend = $"{loginUrl}?name={name}&password={password}&isLogin=1";
        
        WWW www = new WWW(urlToSend);
        yield return www;
        string result = www.text;
        string[] resultArray = result.Split('\n');
        
        bool success = result.Contains("Success");

        if (success)
        {
            string[] info = resultArray[1].Split("--");
            string score = info[0], name1 = info[1], password1 = info[2], level = info[3], lastxp = info[4], lastmoney = info[5], lasthealth = info[6], itemsString = info[7];
            
           

            PlayerPrefs.SetString("name", name1);
            PlayerPrefs.SetString("password", password1);
            PlayerPrefs.SetInt("highscore", score == "" ? 0 : int.Parse(score));
            PlayerPrefs.SetInt("health", lasthealth == "" ? 100 : int.Parse(lasthealth));
            PlayerPrefs.SetInt("xp", lastxp == "" ? 0 : int.Parse(lastxp));
            PlayerPrefs.SetInt("money", lastmoney == "" ? 0 : int.Parse(lastmoney));
            PlayerPrefs.SetInt("level", level == "" ? 0 : int.Parse(level));
            PlayerPrefs.SetString("items", itemsString);
        }


        callback(success, resultArray[0]);
    }
    
    public void Register(string name, string password, Action<bool, string> callback)
    {
        StartCoroutine(SendRegister(name, password, callback));
    }
    
    private IEnumerator SendRegister(string name, string password, Action<bool, string> callback)
    {
        string urlToSend = $"{loginUrl}?name={name}&password={password}&isLogin=0";
        
        WWW www = new WWW(urlToSend);
        yield return www;
        string result = www.text;
        
        callback(result.Contains("Success"), result);
    }
    
    public void SavePlayerData()
    {
        int xp = PlayerPrefs.GetInt("xp"), money = PlayerPrefs.GetInt("money"), health = PlayerPrefs.GetInt("health"), level = PlayerPrefs.GetInt("level");

        var json = PlayerPrefs.GetString("items");

        string name = PlayerPrefs.GetString("name");

        StartCoroutine(SendPlayerData(name, xp, money, health, level, json));
        

        
        

    }

    private IEnumerator SendPlayerData(string s, int xp, int money, int health, int level, string items)
    {
        int highscore = PlayerPrefs.GetInt("highscore");
        if (xp > highscore)
        {
            PlayerPrefs.SetInt("highscore", xp);
            highscore = xp;
        }
        
        
        string urlToSend = $"{updateUrl}?name={s}&xp={xp}&money={money}&health={health}&level={level}&highscore={highscore}&items={items}";
        
        WWW www = new WWW(urlToSend);
        yield return www;
        string result = www.text;
        Debug.Log(result);
    }

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void NextLevel()
    {
        int level = PlayerPrefs.GetInt("level");
        level++;
        var player = GameObject.FindGameObjectWithTag("Player");
        var playerInventory = player.GetComponent<Inventory>();
        var playerHealth = player.GetComponent<Health>();
        
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetInt("health", playerHealth.currentHealth);
        PlayerPrefs.SetInt("xp", playerInventory.xp);
        PlayerPrefs.SetInt("money", playerInventory.money);
        var saveData = new InventorySaveData();
        saveData.Populate(playerInventory.items);
        PlayerPrefs.SetString("items", JsonUtility.ToJson(saveData));
        
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex + 1);
    }
    
    public void RestartLevel()
    {
        int level = PlayerPrefs.GetInt("level");
        // var player = GameObject.FindGameObjectWithTag("Player");
        // var playerInventory = player.GetComponent<Inventory>();
        // var playerHealth = player.GetComponent<Health>();
        //
        // PlayerPrefs.SetInt("health", playerHealth.currentHealth);
        // PlayerPrefs.SetInt("xp", playerInventory.xp);
        // PlayerPrefs.SetInt("money", playerInventory.money);
        
        SceneManager.LoadScene(level);
    }


    public void Logout()
    {
        PlayerPrefs.SetString("name", "");
        PlayerPrefs.SetString("password", "");
        PlayerPrefs.SetString("score", "");
        PlayerPrefs.SetString("level", "");
    }
}

[System.Serializable]
public class InventorySaveData
{
    public List<int> amounts;
    public List<string> itemNames;
    
    public void Populate(List<(int amount, Item item)> items)
    {
        amounts = new List<int>();
        itemNames = new List<string>();
        
        foreach (var item in items)
        {
            amounts.Add(item.amount);
            itemNames.Add(item.item.name);
        }
    }
    public List<(int amount, Item item)> GetItems()
    {
        //Zip together the two lists
        return amounts.Zip(itemNames, (amount, name) => (amount, GameManager.Instance.allItems.First(i => i.name == name))).ToList();
        
    }
}