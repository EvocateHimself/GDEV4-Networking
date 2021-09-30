using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour {

    [SerializeField] string hostIPAdress;
    [SerializeField] InputField hostIPAdressInput;
    [SerializeField] Button hostButton;
    [SerializeField] Button clientButton;
    [SerializeField] Button backButton;
    [SerializeField] GameObject welcomeWindow;
    public GameObject waitingWindow;
    [SerializeField] Button hostBackButton;
    public GameObject gameWindow;
    public Text errorBox;

    public Server server;
    public Client client;

    private void Awake() {
        RegisterEvents();
    }

    private void Start() {

        // Connect to host, and connect to ourselves (the client)
        hostButton.onClick.AddListener(() => {
            server.Init(1551);
            client.Init("127.0.0.1", 1551);
            waitingWindow.SetActive(true);
            //this.gameObject.SetActive(false);
            //StartCoroutine(Main.instance.web.Login(usernameInput.text, passwordInput.text));
        });

        backButton.onClick.AddListener(() => {
            welcomeWindow.SetActive(true);
            this.gameObject.SetActive(false);
            Main.Instance.canCheck = false;
        });

        clientButton.onClick.AddListener(() => {
            client.Init(hostIPAdressInput.text, 1551);        
            //gameWindow.SetActive(true);
        });
    }

    #region Manage Events
    private void RegisterEvents() {
        NetUtility.C_START_GAME += OnStartGameClient;
    }

    private void UnregisterEvents() {
        NetUtility.C_START_GAME -= OnStartGameClient;
    }

    private void OnStartGameClient(NetMessage msg) {
        // Show the start canvas (game window)
        Main.Instance.lobby.gameWindow.SetActive(true);
        Main.Instance.lobby.waitingWindow.SetActive(false);
    }
    #endregion
}
