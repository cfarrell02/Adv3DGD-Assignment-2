using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public TMP_InputField nameInput, passwordInput;
    public Button loginButton, registerButton, logoutButton,startGameButton, resumeButton;
    public TextMeshProUGUI alertText, welcomeText;
    bool alertActive = false;
    bool isLoggedIn = false;
    
    private Animator loginAnimator;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        loginButton.onClick.AddListener(Login);
        registerButton.onClick.AddListener(Register);
        logoutButton.onClick.AddListener(Logout);
        loginAnimator = GetComponentInChildren<Animator>();
        
        //On application quit, clear the player prefs
        Application.quitting += () =>
        {
            PlayerPrefs.DeleteAll();
        };
        
        
        resumeButton.onClick.AddListener(() =>
        {
            int level = PlayerPrefs.GetInt("level", 1);
            level = (level<1 || level > 3) ? 1 : level; //If level is not between 1 and 3, set it to 1
            
            SceneManager.LoadScene(level);
            
            //Lock cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        });
        
        string name = PlayerPrefs.GetString("name");
        if (name != "")
        {
            isLoggedIn = true;
            loginAnimator.SetBool("isLoggedIn", true);
            welcomeText.text = $"Welcome {name}";
        }
        
        
        startGameButton.onClick.AddListener(() =>
        {
            if (isLoggedIn)
            {
                
                string name = PlayerPrefs.GetString("name"), password = PlayerPrefs.GetString("password");
                PlayerPrefs.DeleteAll();
                PlayerPrefs.SetString("name", name);
                PlayerPrefs.SetString("password", password);
                SceneManager.LoadScene(1);
                
                //Lock cursor
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                SetAlertText("Please login to start the game.");
            }
        });
        
        
        
    }

    private void Logout()
    {
        GameManager.Instance.Logout();
        isLoggedIn = false;
        loginAnimator.SetBool("isLoggedIn", false);
    }

    private void Login()
    {
        if(nameInput.text == "" || passwordInput.text == "")
        {
            SetAlertText("Please enter a name and password.");
            return;
        }
        alertText.text = "Loading...";
        
        GameManager.Instance.Login(nameInput.text, passwordInput.text, (bool success, string message) =>
        {
            alertText.text = "";
            SetAlertText(message);
            if (success)
            {
                isLoggedIn = true;
                loginAnimator.SetBool("isLoggedIn", true);
                string name = PlayerPrefs.GetString("name");
                welcomeText.text = $"Welcome {name}";
            }
        });
    }
    
    private void Register()
    {
        if(nameInput.text == "" || passwordInput.text == "")
        {
            SetAlertText("Please enter a name and password.");
            return;
        }
        alertText.text = "Loading...";
        GameManager.Instance.Register(nameInput.text, passwordInput.text, (bool success, string message) =>
        {
            SetAlertText(message);
        });
    }
    
    
    private void SetAlertText(string text, float timeOut = 2f)
    {
        if (alertActive)
        {
            StopAllCoroutines();
            alertActive = false;
        }
        Debug.Log(text);
        StartCoroutine(AlertText(text, timeOut));
    }
    
    IEnumerator AlertText(string text, float timeOut = 5f)
    {
        alertActive = true;
        alertText.text = text;
        yield return new WaitForSeconds(timeOut);
        alertText.text = "";
        alertActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
