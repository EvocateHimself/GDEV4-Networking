using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour {

    [SerializeField] InputField usernameInput;
    [SerializeField] InputField passwordInput;
    [SerializeField] InputField repeatPasswordInput;
    [SerializeField] Button submitButton;
    [SerializeField] Button backButton;
    [SerializeField] GameObject loginWindow;
    public Text errorBox;

    // Start is called before the first frame update
    private void Start() {
        submitButton.onClick.AddListener(() => {
            StartCoroutine(Main.Instance.web.Register(usernameInput.text, passwordInput.text, repeatPasswordInput.text));
        });

        backButton.onClick.AddListener(() => {
            loginWindow.SetActive(true);
            this.gameObject.SetActive(false);
        });
    }
}
