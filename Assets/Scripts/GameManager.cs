using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            string score = info[0], name1 = info[1], password1 = info[2], level = info[3];

            PlayerPrefs.SetString("name", name1);
            PlayerPrefs.SetString("password", password1);
            PlayerPrefs.SetString("score", score);
            PlayerPrefs.SetString("level", level);
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
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Logout()
    {
        PlayerPrefs.SetString("name", "");
        PlayerPrefs.SetString("password", "");
        PlayerPrefs.SetString("score", "");
        PlayerPrefs.SetString("level", "");
    }
}
