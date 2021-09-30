using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;
using Unity.Collections;
using UnityEngine.UI;

public enum Elements { 
    NONE,
    AIR,
    FIRE,
    EARTH,
    WATER
}

public class GameManager : MonoBehaviour {

    public static GameManager Instance { set; get; }

    // Reference variables
    [Header("Main")]
    [SerializeField] private GameObject chooseWindow;
    [SerializeField] private GameObject versusWindow;
    [SerializeField] private GameObject quitWindow;
    [SerializeField] private GameObject gameWindow;
    [SerializeField] private GameObject lobbyWindow;
    [SerializeField] private GameObject waitingWindow;
    [SerializeField] private Text playerOneScoreText;
    [SerializeField] private Text playerTwoScoreText;
    [SerializeField] private Button leaveButton;

    [Header("Choose Window")]
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button infoButton;
    [SerializeField] private List<GameObject> infoDetails;
    [SerializeField] private GameObject airCard, fireCard, earthCard, waterCard;
    [SerializeField] private Sprite airSprite, fireSprite, earthSprite, waterSprite, unknownSprite;
    
    public Button airCardButton, fireCardButton, earthCardButton, waterCardButton;

    [Header("Versus Window")]
    [SerializeField] private Text headerText;
    [SerializeField] private Text statusText;
    [SerializeField] private GameObject playerOneCard, playerTwoCard;
    [SerializeField] private GameObject playerOneCardBorder, playerTwoCardBorder;
    [SerializeField] private GameObject playerOneText, playerTwoText;

    // Game variables
    private Elements playerElement;
    private bool playerOneChosenCard = false;
    private bool playerTwoChosenCard = false;
    private FixedString64 playerOneChosenElement, playerTwoChosenElement;
    private string winMessage = "You Win!";
    private string lostMessage = "Opponent Won!";
    private string drawMessage = "It's a Draw!";
    public int playerOneScore = 0;
    public int playerTwoScore = 0;
    public string username_player1 = "";
    public string username_player2 = "";

    // Networking variables
    private int playerCount = -1;
    private int currentTeam = -1;
    

    private void Awake() {
        Instance = this;
        statusText.gameObject.SetActive(false);

        playerElement = Elements.NONE;

        // When pressed on the confirm button
        confirmButton.onClick.AddListener(() => {
            if (playerElement != Elements.NONE) {
                Debug.Log("Player one choice " + playerElement);

                // Battle (versus window)
                ConfirmChoice();
            }
        });

        RegisterEvents();
    }


    public void SetPlayerOneChoice(Elements elements) {

        switch(elements) {

            case Elements.AIR:
                playerElement = Elements.AIR;

                // Create white outlines
                airCardButton.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
                fireCardButton.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0f);
                earthCardButton.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0f);
                waterCardButton.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0f);

                // Set P1 unknownCard sprite to sprite of element
                playerOneCard.GetComponent<Image>().sprite = airSprite;
                break;

            case Elements.FIRE:
                playerElement = Elements.FIRE;

                // Create white outlines
                airCardButton.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0f);
                fireCardButton.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
                earthCardButton.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0f);
                waterCardButton.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0f);

                // Set P1 unknownCard sprite to sprite of element
                playerOneCard.GetComponent<Image>().sprite = fireSprite;
                break;

            case Elements.EARTH:
                playerElement = Elements.EARTH;

                // Create white outlines
                airCardButton.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0f);
                fireCardButton.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0f);
                earthCardButton.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
                waterCardButton.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0f);

                // Set P1 unknownCard sprite to sprite of element
                playerOneCard.GetComponent<Image>().sprite = earthSprite;
                break;

            case Elements.WATER:
                playerElement = Elements.WATER;

                // Create white outlines
                airCardButton.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0f);
                fireCardButton.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0f);
                earthCardButton.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0f);
                waterCardButton.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 1f);

                // Set P1 unknownCard sprite to sprite of element
                playerOneCard.GetComponent<Image>().sprite = waterSprite;
                break;
        }
    }


    // When someone quits/leaves the game
    public void OnQuitButton() {
        // Net implementation
        NetQuitGame qm = new NetQuitGame();
        qm.teamId = currentTeam;
        qm.hasQuit = currentTeam;
        Client.Instance.SendToServer(qm);

        // Save score
        if (qm.teamId == 0) {
            StartCoroutine(Main.Instance.web.SaveScore(Main.Instance.userInfo.UserId, Main.Instance.userInfo.Score + playerOneScore));
        } else {
            StartCoroutine(Main.Instance.web.SaveScore(Main.Instance.userInfo.UserId, Main.Instance.userInfo.Score + playerTwoScore));
        }

        versusWindow.SetActive(false);
        chooseWindow.SetActive(true);
        gameWindow.SetActive(false);
        lobbyWindow.SetActive(true);

        // Save game data to highscores
        StartCoroutine(Main.Instance.web.SaveGame(username_player1, playerOneScore, username_player2, playerTwoScore));

        Invoke("ShutdownRelay", 1.0f);

        // Reset some values
        //playerCount = -1;
        //currentTeam = -1;
    }


    // When someone quits/leaves the game
    public void OnExitButton() {
        NetQuitGame qm = new NetQuitGame();
        qm.teamId = currentTeam;
        qm.hasQuit = currentTeam;
        Client.Instance.SendToServer(qm);

        versusWindow.SetActive(false);
        chooseWindow.SetActive(true);
        gameWindow.SetActive(false);
        lobbyWindow.SetActive(true);
        quitWindow.SetActive(false);

        Invoke("ShutdownRelay", 1.0f);

        // Reset some values
        playerCount = -1;
        currentTeam = -1;
    }


    public void OnHostBackButton() {
        // When exiting host
        Invoke("ShutdownRelay", 1.0f);
        
        //this.gameObject.SetActive(true);
        waitingWindow.SetActive(false);

        // Reset some values
        playerCount = -1;
        currentTeam = -1;
    }
    


    public void GameReset() {

    }


    public void DetermineWinner(NetMessage msg) {
        
        NetMakeMove mm = msg as NetMakeMove;

        // At some point I also have to add sound effects here LOL
        // Check player choices

        // Draw
        if (playerOneChosenElement == playerTwoChosenElement) {
            headerText.text = drawMessage;
        } else if (playerOneChosenElement == "AIR" && playerTwoChosenElement == "EARTH") {
            VictoryPlayerOne(msg);
        } else if (playerTwoChosenElement == "AIR" && playerOneChosenElement == "EARTH") {
            VictoryPlayerTwo(msg);
        } else if (playerOneChosenElement == "FIRE" && playerTwoChosenElement == "AIR") {
            VictoryPlayerOne(msg);
        } else if (playerTwoChosenElement == "FIRE" && playerOneChosenElement == "AIR") {
            VictoryPlayerTwo(msg);
        } else if (playerOneChosenElement == "EARTH" && playerTwoChosenElement == "WATER") {
            VictoryPlayerOne(msg);
        } else if (playerTwoChosenElement == "EARTH" && playerOneChosenElement == "WATER") {
            VictoryPlayerTwo(msg);
        } else if (playerOneChosenElement == "WATER" && playerTwoChosenElement == "FIRE") {
            VictoryPlayerOne(msg);
        } else if (playerTwoChosenElement == "WATER" && playerOneChosenElement == "FIRE") {
            VictoryPlayerTwo(msg);
        } else {
            headerText.text = drawMessage;
        }

        // Start new game in x seconds
        StartCoroutine(StartNewRound());
    }


    // Wait till opponent has picked something
    private void ConfirmChoice() {
        // Net implementation
        NetMakeMove mm = new NetMakeMove();
        mm.playerElementNW = playerElement.ToString();
        mm.playerName = Main.Instance.userInfo.UserName.ToString();
        mm.teamId = currentTeam;
        Client.Instance.SendToServer(mm);

        // Wait for opponent to pick a card
        if (playerElement != Elements.NONE) {
            //headerText.text = "Waiting for opponent to pick a card...";
        }
        chooseWindow.SetActive(false);
        versusWindow.SetActive(true);
    }


    private void VictoryPlayerOne(NetMessage msg) {
        NetMakeMove mm = msg as NetMakeMove;

        if (mm.teamId == currentTeam) {
            if (mm.teamId == 1) {
                headerText.text = lostMessage;
                headerText.color = new Color32(217, 67, 52, 255);
                playerOneCard.GetComponent<Image>().color = new Color32(255, 255, 255, 50);
                playerTwoCardBorder.GetComponent<Image>().color = new Color32(217, 67, 52, 255);
                playerTwoText.GetComponent<Text>().color = new Color32(217, 67, 52, 255);
                playerOneScore += 1;
                playerTwoScoreText.text = playerOneScore.ToString();
            } else {
                headerText.text = winMessage;
                headerText.color = new Color32(76, 176, 80, 255);
                playerTwoCard.GetComponent<Image>().color = new Color32(255, 255, 255, 50);
                playerOneCardBorder.GetComponent<Image>().color = new Color32(76, 176, 80, 255);
                playerOneText.GetComponent<Text>().color = new Color32(76, 176, 80, 255);
                playerOneScore += 1;
                playerOneScoreText.text = playerOneScore.ToString();
            }
        } else {
            if (mm.teamId == 1) {
                headerText.text = winMessage;
                headerText.color = new Color32(76, 176, 80, 255);
                playerTwoCard.GetComponent<Image>().color = new Color32(255, 255, 255, 50);
                playerOneCardBorder.GetComponent<Image>().color = new Color32(76, 176, 80, 255);
                playerOneText.GetComponent<Text>().color = new Color32(76, 176, 80, 255);
                playerOneScore += 1;
                playerOneScoreText.text = playerOneScore.ToString();
            } else {
                headerText.text = lostMessage;
                headerText.color = new Color32(217, 67, 52, 255);
                playerOneCard.GetComponent<Image>().color = new Color32(255, 255, 255, 50);
                playerTwoCardBorder.GetComponent<Image>().color = new Color32(217, 67, 52, 255);
                playerTwoText.GetComponent<Text>().color = new Color32(217, 67, 52, 255);
                playerOneScore += 1;
                playerTwoScoreText.text = playerOneScore.ToString();
            }
        }
}


    private void VictoryPlayerTwo(NetMessage msg) {
        NetMakeMove mm = msg as NetMakeMove;

        if (mm.teamId == currentTeam) {
            if (mm.teamId == 0) {
                headerText.text = lostMessage;
                headerText.color = new Color32(217, 67, 52, 255);
                playerOneCard.GetComponent<Image>().color = new Color32(255, 255, 255, 50);
                playerTwoCardBorder.GetComponent<Image>().color = new Color32(217, 67, 52, 255);
                playerTwoText.GetComponent<Text>().color = new Color32(217, 67, 52, 255);
                playerTwoScore += 1;
                playerTwoScoreText.text = playerTwoScore.ToString();
            } else {
                headerText.text = winMessage;
                headerText.color = new Color32(76, 176, 80, 255);
                playerTwoCard.GetComponent<Image>().color = new Color32(255, 255, 255, 50);
                playerOneCardBorder.GetComponent<Image>().color = new Color32(76, 176, 80, 255);
                playerOneText.GetComponent<Text>().color = new Color32(76, 176, 80, 255);
                playerTwoScore += 1;
                playerOneScoreText.text = playerTwoScore.ToString();
            }
        } else {
            if (mm.teamId == 0) {
                headerText.text = winMessage;
                headerText.color = new Color32(76, 176, 80, 255);
                playerTwoCard.GetComponent<Image>().color = new Color32(255, 255, 255, 50);
                playerOneCardBorder.GetComponent<Image>().color = new Color32(76, 176, 80, 255);
                playerOneText.GetComponent<Text>().color = new Color32(76, 176, 80, 255);
                playerTwoScore += 1;
                playerOneScoreText.text = playerTwoScore.ToString();
            } else {
                headerText.text = lostMessage;
                headerText.color = new Color32(217, 67, 52, 255);
                playerOneCard.GetComponent<Image>().color = new Color32(255, 255, 255, 50);
                playerTwoCardBorder.GetComponent<Image>().color = new Color32(217, 67, 52, 255);
                playerTwoText.GetComponent<Text>().color = new Color32(217, 67, 52, 255);
                playerTwoScore += 1;
                playerTwoScoreText.text = playerTwoScore.ToString();
            }
        }
    }


    // Reveal cards 
    private IEnumerator Battle(NetMessage msg) {

        NetMakeMove mm = msg as NetMakeMove;

        // Start countdown
        int counter = 3;
        while (counter > 0) {
            headerText.text = "Revealing cards in " + counter + "...";
            yield return new WaitForSeconds (1);
            counter--;
        }

        //headerText.text = "The winner is...";
        statusText.gameObject.SetActive(true);

        // Set P2 unknownCard sprite to sprite of element
        if (mm.teamId == currentTeam) {
            if (mm.teamId == 1) {
                if (playerOneChosenElement == "AIR") {
                    playerTwoCard.GetComponent<Image>().sprite = airSprite;
                } else if (playerOneChosenElement == "FIRE") {
                    playerTwoCard.GetComponent<Image>().sprite = fireSprite;
                } else if (playerOneChosenElement == "EARTH") {
                    playerTwoCard.GetComponent<Image>().sprite = earthSprite;
                } else if (playerOneChosenElement == "WATER") {
                    playerTwoCard.GetComponent<Image>().sprite = waterSprite;
                }
            } else {
                if (playerTwoChosenElement == "AIR") {
                    playerTwoCard.GetComponent<Image>().sprite = airSprite;
                } else if (playerTwoChosenElement == "FIRE") {
                    playerTwoCard.GetComponent<Image>().sprite = fireSprite;
                } else if (playerTwoChosenElement == "EARTH") {
                    playerTwoCard.GetComponent<Image>().sprite = earthSprite;
                } else if (playerTwoChosenElement == "WATER") {
                    playerTwoCard.GetComponent<Image>().sprite = waterSprite;
                }
            }
        } else {
            if (mm.playerElementNW == "AIR") {
                playerTwoCard.GetComponent<Image>().sprite = airSprite;
            } else if (mm.playerElementNW == "FIRE") {
                playerTwoCard.GetComponent<Image>().sprite = fireSprite;
            } else if (mm.playerElementNW == "EARTH") {
                playerTwoCard.GetComponent<Image>().sprite = earthSprite;
            } else if (mm.playerElementNW == "WATER") {
                playerTwoCard.GetComponent<Image>().sprite = waterSprite;
            }
        }

        Debug.Log("It's a battle between P1: " + playerOneChosenElement + " and P2: " + playerTwoChosenElement);

        switch (playerElement) {
            case Elements.AIR:
                playerOneCard.GetComponent<Image>().sprite = airSprite;
                break;
            case Elements.FIRE:
                playerOneCard.GetComponent<Image>().sprite = fireSprite;
                break;
            case Elements.EARTH:
                playerOneCard.GetComponent<Image>().sprite = earthSprite;
                break;
            case Elements.WATER:
                playerOneCard.GetComponent<Image>().sprite = waterSprite;
                break;
        }

        // Determine the winner
        DetermineWinner(msg);
    }


    // Start new round
    private IEnumerator StartNewRound() {

        // Start countdown
        int counter = 3;
        while (counter > 0) {
            statusText.text = "Next round in " + counter + "...";
            yield return new WaitForSeconds (1);
            counter--;
        }  

        // Set player choices back to none
        playerElement = Elements.NONE;

        // Net implementation
        NetMakeMove mm = new NetMakeMove();
        mm.playerElementNW = playerElement.ToString();
        mm.teamId = currentTeam;
        Client.Instance.SendToServer(mm);

        // Save score
        if (mm.teamId == 0) {
            StartCoroutine(Main.Instance.web.SaveScore(Main.Instance.userInfo.UserId, Main.Instance.userInfo.Score + playerOneScore));
        } else {
            StartCoroutine(Main.Instance.web.SaveScore(Main.Instance.userInfo.UserId, Main.Instance.userInfo.Score + playerTwoScore));
        }

        // Reset outlines
        airCardButton.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0f);
        fireCardButton.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0f);
        earthCardButton.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0f);
        waterCardButton.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0f);

        // Change to game window
        chooseWindow.SetActive(true);
        versusWindow.SetActive(false);
        
        headerText.text = "";
        headerText.color = new Color32(255, 255, 255, 255);
        playerOneCard.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        playerTwoCard.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

        statusText.gameObject.SetActive(false);

        playerOneCard.GetComponent<Image>().sprite = unknownSprite;
        playerTwoCard.GetComponent<Image>().sprite = unknownSprite;
        playerOneCardBorder.GetComponent<Image>().color = new Color32(76, 176, 80, 0);
        playerTwoCardBorder.GetComponent<Image>().color = new Color32(76, 176, 80, 0);
        playerOneText.GetComponent<Text>().color = new Color32(255, 255, 255, 255);
        playerTwoText.GetComponent<Text>().color = new Color32(255, 255, 255, 255);
    }
    

    // Show element details on hover (win/draw/lose)
    public void ShowElementDetails() {
        foreach(GameObject obj in infoDetails) {
            obj.SetActive(true);
        }

        airCard.GetComponent<Image>().color = new Color(255, 255, 255, 0.2f);
        fireCard.GetComponent<Image>().color = new Color(255, 255, 255, 0.2f);
        earthCard.GetComponent<Image>().color = new Color(255, 255, 255, 0.2f);
        waterCard.GetComponent<Image>().color = new Color(255, 255, 255, 0.2f);
    }


    // Hide element details on exit hover (win/draw/lose)
    public void HideElementDetails() {
        foreach(GameObject obj in infoDetails) {
            obj.SetActive(false);
        }

        airCard.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
        fireCard.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
        earthCard.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
        waterCard.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
    }


    // Scale UI element on mouse hover (pop)
    public void ScaleObjectOnHoverEnter(GameObject sender) {
        StartCoroutine(Scale(sender));
    }


    // Unscale UI element on mouse hover exit (return to normal size)
    public void UnScaleObjectOnHoverExit(GameObject sender) {
        StartCoroutine(UnScale(sender));
    }


    private IEnumerator Scale(GameObject sender) {
        float scaleDuration = 0.1f;
        Vector3 actualScale = sender.GetComponent<RectTransform>().localScale;
        Vector3 targetScale = new Vector3 (1.2f, 1.2f, 1.2f);

        for(float t = 0; t < 1; t += Time.deltaTime / scaleDuration ) {
            sender.GetComponent<RectTransform>().localScale = Vector3.Lerp(actualScale, targetScale, t);
            yield return null;
        }
    }


    private IEnumerator UnScale(GameObject sender) {
        float scaleDuration = 0.1f;
        Vector3 actualScale = sender.GetComponent<RectTransform>().localScale;
        Vector3 targetScale = new Vector3 (1.1f, 1.1f, 1.1f);

        for(float t = 0; t < 1; t += Time.deltaTime / scaleDuration ) {
            sender.GetComponent<RectTransform>().localScale = Vector3.Lerp(actualScale, targetScale, t);
            yield return null;
        }
    }


    #region Manage Events
    private void RegisterEvents() {
        // Server events
        NetUtility.S_WELCOME += OnWelcomeServer;
        NetUtility.S_MAKE_MOVE += OnMakeMoveServer;
        NetUtility.S_QUIT_GAME += OnQuitGameServer;
        
        // Client events
        NetUtility.C_WELCOME += OnWelcomeClient;
        NetUtility.C_START_GAME += OnStartGameClient;
        NetUtility.C_MAKE_MOVE += OnMakeMoveClient;
        NetUtility.C_QUIT_GAME += OnQuitGameClient;

    }

    private void UnregisterEvents() {
        // Server events
        NetUtility.S_WELCOME -= OnWelcomeServer;
        NetUtility.S_MAKE_MOVE -= OnMakeMoveServer;
        NetUtility.S_QUIT_GAME -= OnQuitGameServer;
        
        // Client events
        NetUtility.C_WELCOME -= OnWelcomeClient;
        NetUtility.C_START_GAME -= OnStartGameClient;
        NetUtility.C_MAKE_MOVE -= OnMakeMoveClient;
        NetUtility.C_QUIT_GAME -= OnQuitGameClient;
    }

    // Server
    private void OnWelcomeServer(NetMessage msg, NetworkConnection cnn) {
        // Client has connected, assign a team and return the message back to him
        NetWelcome nw = msg as NetWelcome;

        // Assign a team
        nw.AssignedTeam = ++playerCount;

        // Return back to the client
        Server.Instance.SendToClient(cnn, nw);

        // Check if server has 2 players, then start game
        if (playerCount == 1) {
            Server.Instance.Broadcast(new NetStartGame());
        }
    }

    private void OnMakeMoveServer(NetMessage msg, NetworkConnection cnn) {
        // Receive the message, broadcast it back
        NetMakeMove mm = msg as NetMakeMove;

        // Here do validation checks

        // Receive and broadcast it back to other client
        //Server.Instance.Broadcast(msg);
        Server.Instance.Broadcast(mm);
    }

    private void OnQuitGameServer(NetMessage msg, NetworkConnection cnn) {
        Server.Instance.Broadcast(msg);
    }


    // Client
    private void OnWelcomeClient(NetMessage msg) {
        // Receive the connection message
        NetWelcome nw = msg as NetWelcome;

        // Assign the team
        currentTeam = nw.AssignedTeam;

        Debug.Log($"My assigned team is {nw.AssignedTeam}");

        // Reset some stats
        playerOneScore = 0;
        playerTwoScore = 0;
        playerOneScoreText.text = "0";
        playerTwoScoreText.text = "0";

        // Reset outlines
        airCardButton.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0f);
        fireCardButton.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0f);
        earthCardButton.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0f);
        waterCardButton.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0f);

        headerText.text = "";
        headerText.color = new Color32(255, 255, 255, 255);
        playerOneCard.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        playerTwoCard.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

        statusText.gameObject.SetActive(false);

        playerOneCard.GetComponent<Image>().sprite = unknownSprite;
        playerTwoCard.GetComponent<Image>().sprite = unknownSprite;
        playerOneCardBorder.GetComponent<Image>().color = new Color32(76, 176, 80, 0);
        playerTwoCardBorder.GetComponent<Image>().color = new Color32(76, 176, 80, 0);
        playerOneText.GetComponent<Text>().color = new Color32(255, 255, 255, 255);
        playerTwoText.GetComponent<Text>().color = new Color32(255, 255, 255, 255);
    }

    private void OnStartGameClient(NetMessage msg) {
        // Show the start canvas (game window) - currently done in Lobby
        
    }

    private void OnMakeMoveClient(NetMessage msg) {
        NetMakeMove mm = msg as NetMakeMove;

        Debug.Log($"MM : {mm.teamId} : {mm.playerElementNW}");

        // Check if player one has made a move
        if (mm.teamId == 0 && mm.playerElementNW != "NONE") {
            playerOneChosenCard = true;
            username_player1 = mm.playerName.ToString();
            playerOneChosenElement = mm.playerElementNW;
            if (mm.teamId == currentTeam) {
                headerText.text = "Waiting for opponent to pick a card...";
                Debug.Log("Waiting for opponent to pick a card...");
            }
        }

        // Check if player two has made a move
        if (mm.teamId == 1 && mm.playerElementNW != "NONE") {
            playerTwoChosenCard = true;
            username_player2 = mm.playerName.ToString();
            playerTwoChosenElement = mm.playerElementNW;
            if (mm.teamId == currentTeam) {
                headerText.text = "Waiting for opponent to pick a card...";
                Debug.Log("Waiting for opponent to pick a card...");
            }
        }

        if (playerOneChosenCard == true && playerTwoChosenCard == true) {
            Debug.Log("Both players have chosen a card!");
            headerText.text = "";
            playerOneChosenCard = false;
            playerTwoChosenCard = false;
            Debug.Log("P1: " + username_player1 + " P2: " + username_player2);
            StartCoroutine(Battle(msg));
        }

    }

    private void OnQuitGameClient(NetMessage msg) {
        // Receive the connection message
        NetQuitGame qm = msg as NetQuitGame;

        Debug.Log("Player has left the game");
        Debug.Log(qm.hasQuit);
        
        // Activate UI (other player has left)
        if(qm.hasQuit != currentTeam) {
            quitWindow.SetActive(true);
        }

        // Send scores to database, and reset scores to 0
        playerOneScore = 0;
        playerTwoScore = 0;
        playerOneScoreText.text = "0";
        playerTwoScoreText.text = "0";
        playerCount = -1;
        currentTeam = -1;
    }

    private void ShutdownRelay() {
        Client.Instance.Shutdown();
        Server.Instance.Shutdown();
    }

    #endregion
}
