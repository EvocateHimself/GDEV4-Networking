using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfo : MonoBehaviour {

    [SerializeField]
    private string userId;
    public string UserId {
        get { return userId; }
        set { userId = value; }
    }

    [SerializeField]
    private string userName;
    public string UserName {
        get { return userName; }
        set { userName = value; }
    }

    [SerializeField]
    private string userPassword;
    public string UserPassword {
        get { return userPassword; }
        set { userPassword = value; }
    }

    [SerializeField]
    private int score;
    public int Score {
        get { return score; }
        set { score = value; }
    }


    // Fetch the player's username and password from the database and save it in a global variable
    public void SetCredentials(string username, string password) {
        UserName = username;
        UserPassword = password;
    }


    // Fetch the player's user id from the database and save it in a global variable
    public void SetID(string id) {
        UserId = id;
    }


    // Fetch the player's score from the database and save it in a global variable
    public void SetScore(int myScore) {
        Score = myScore;
    }

}
