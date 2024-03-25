using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public TMP_InputField nameInput, passwordInput;
    public Button loginButton, registerButton, logoutButton,startGameButton;
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
        
        string name = PlayerPrefs.GetString("name");
        if (name != "")
        {
            isLoggedIn = true;
            loginAnimator.SetBool("isLoggedIn", true);
            welcomeText.text = $"Welcome {name}";
        }
        
        
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
        
        GameManager.Instance.Login(nameInput.text, passwordInput.text, (bool success, string message) =>
        {
            
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
