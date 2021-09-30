using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class jsonDataClass {
    public string id;
    public string username;
    public string password;
    public int score;
}

public class GetUserInfo : MonoBehaviour {

    // UI variables
    [SerializeField] Text ScoreText;
    [SerializeField] Text usernameText;
    [SerializeField] Button editProfileButton;
    [SerializeField] Button highscoresButton;
    [SerializeField] Button logoutButton;
    [SerializeField] GameObject loginWindow;
    [SerializeField] Button playButton;
    [SerializeField] GameObject lobbyWindow;

    private string userId;
    private string userName;
    private string userPassword;
    private int currentScore;

    Action<string> _createUserInfoCallback;


    private void Start() {

        userId = Main.Instance.userInfo.UserId;
        userName = Main.Instance.userInfo.UserName;
        userPassword = Main.Instance.userInfo.UserPassword;
        currentScore = Main.Instance.userInfo.Score;
        
        _createUserInfoCallback = (jsonArray) => {
            StartCoroutine(CreateUserInfo(jsonArray));
        };

        // Wait 5 seconds to update userInfo
        StartCoroutine(FetchUserInfo(5f)); // Stop coroutine once game begins?

        // Open new window url to edit profile
        editProfileButton.onClick.AddListener(() => {
            Application.OpenURL("https://kevinabsen.com/edit_profile.php?id=" + userId + "&username=" + userName + "&password=" + Main.Instance.userInfo.UserPassword);
        });

        // Open new window url to view highscores
        highscoresButton.onClick.AddListener(() => {
            Application.OpenURL("https://kevinabsen.com/highscores.php");
        });

        // Logout here, disconnect from data?
        logoutButton.onClick.AddListener(() => {
            Main.Instance.canCheck = false;

            loginWindow.SetActive(true);
            this.gameObject.SetActive(false);

            StopCoroutine(FetchUserInfo(5f));

            // Reset user info
            userId = "";
            userName = "";
            userPassword = "";
            currentScore = 0;
            
            Main.Instance.userInfo.SetID(userId);
            Main.Instance.userInfo.SetCredentials(userName, userPassword);
            Main.Instance.userInfo.SetScore(currentScore);
        });

        playButton.onClick.AddListener(() => {
            lobbyWindow.SetActive(true);
            this.gameObject.SetActive(false);
        });
    }


    // Check profile when someone logs in
    private void Update() {
        if (this.gameObject.activeInHierarchy && Main.Instance.canCheck == false) {
            Main.Instance.canCheck = true;

            userId = Main.Instance.userInfo.UserId;
            userName = Main.Instance.userInfo.UserName;
            userPassword = Main.Instance.userInfo.UserPassword;
            currentScore = Main.Instance.userInfo.Score;
            
            _createUserInfoCallback = (jsonArray) => {
                StartCoroutine(CreateUserInfo(jsonArray));
            };

            StartCoroutine(Main.Instance.web.GetUserInfo(userId, _createUserInfoCallback));

            // Wait 5 seconds to update userInfo
            StartCoroutine(FetchUserInfo(5f)); // Stop coroutine once game begins?
        }
    }


    // Get + Update user info
    private IEnumerator FetchUserInfo(float time) {
        while(true) {
            StartCoroutine(Main.Instance.web.GetUserInfo(userId, _createUserInfoCallback));
            yield return new WaitForSeconds(time);
        }
    }


    // Decode json array and create new user info
    private IEnumerator CreateUserInfo(string jsonArray) {

        jsonDataClass jsnData = JsonUtility.FromJson<jsonDataClass>(jsonArray);

        Main.Instance.userInfo.SetCredentials(jsnData.username, jsnData.password);

        usernameText.text = "Welcome, " + jsnData.username + "!";
        Main.Instance.userInfo.SetScore(jsnData.score);
        ScoreText.text = jsnData.score.ToString();
    
        yield return null;
    } 
}
