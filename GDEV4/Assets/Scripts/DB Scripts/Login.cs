using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour {

    [SerializeField] InputField usernameInput;
    [SerializeField] InputField passwordInput;
    [SerializeField] Button loginButton;
    [SerializeField] Button registerButton;
    [SerializeField] Button quitButton;
    [SerializeField] GameObject registerWindow;
    public Text errorBox;

    private void Start() {
        loginButton.onClick.AddListener(() => {
            StartCoroutine(Main.Instance.web.Login(usernameInput.text, passwordInput.text));
        });

        registerButton.onClick.AddListener(() => {
            this.gameObject.SetActive(false);
            registerWindow.SetActive(true);
        });

        quitButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }
}
