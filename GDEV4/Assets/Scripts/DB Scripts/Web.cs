using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Web : MonoBehaviour {

    // Login and send to database
    public IEnumerator Login(string username, string password) {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using (UnityWebRequest www = UnityWebRequest.Post("https://www.kevinabsen.com/login.php", form)) {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log(www.error);
            } else {
                Debug.Log(www.downloadHandler.text);
                

                // If we receive an error message
                if (www.downloadHandler.text.Contains("Wrong credentials") || www.downloadHandler.text.Contains("Username does not exist")) {
                    Debug.Log("Try Again");
                    Main.Instance.login.errorBox.text = www.downloadHandler.text;
                } else { 
                    // If we logged in correctly
                    
                    Main.Instance.userInfo.SetCredentials(username, password);
                    Main.Instance.userInfo.SetID(www.downloadHandler.text);

                    Main.Instance.userProfile.SetActive(true);
                    Main.Instance.login.gameObject.SetActive(false);
                    //StartCoroutine(GetUserInfo(Main.Instance.userInfo.UserId)); // Fetch user info
                }
            }
        }
    }


    // Register and send to database
    public IEnumerator Register(string username, string password, string repeatPassword) {
        WWWForm form = new WWWForm();
        form.AddField("registerUser", username);
        form.AddField("registerPass", password);
        form.AddField("registerPassRepeat", repeatPassword);

        using (UnityWebRequest www = UnityWebRequest.Post("https://www.kevinabsen.com/register.php", form)) {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log(www.error);
            } else {
                Debug.Log(www.downloadHandler.text);
                //Main.Instance.userInfo.SetCredentials(username, password);
                //Main.Instance.userInfo.SetID(www.downloadHandler.text);

                if (www.downloadHandler.text.Contains("Passwords don't match") || www.downloadHandler.text.Contains("has to be")) {
                    Main.Instance.register.errorBox.text = www.downloadHandler.text;
                } else {
                    // If we logged in correctly
                    Main.Instance.register.errorBox.text = www.downloadHandler.text;
                    //Main.Instance.userProfile.SetActive(true);
                    //Main.Instance.register.gameObject.SetActive(false);
                }
            }
        }
    }
    

    // Save the current score for each player
    public IEnumerator SaveScore(string userID, int userScore) {
        WWWForm form = new WWWForm();
        form.AddField("userID", userID);
        form.AddField("userScore", userScore);

        Debug.Log("My userId is " + userID + " with score " + userScore);

        using (UnityWebRequest www = UnityWebRequest.Post("https://www.kevinabsen.com/save_score.php", form)) {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log(www.error);
            } else {
                Debug.Log(www.downloadHandler.text);
                // Echo score here?
                //Main.Instance.userInfo.SetScore(int.Parse(www.downloadHandler.text));
            }
        }
    }


    // Save the game/match scores to the highscores
    public IEnumerator SaveGame(string user_p1, int score_p1, string user_p2, int score_p2) {
        WWWForm form = new WWWForm();
        form.AddField("username_player1", user_p1);
        form.AddField("score_player1", score_p1);
        form.AddField("username_player2", user_p2);
        form.AddField("score_player2", score_p2);

        using (UnityWebRequest www = UnityWebRequest.Post("https://www.kevinabsen.com/save_game.php", form)) {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log(www.error);
            } else {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }


    // Fetch the user info in a json array
    public IEnumerator GetUserInfo(string userID, System.Action<string> callback) {
        WWWForm form = new WWWForm();
        form.AddField("userID", userID);

        using (UnityWebRequest www = UnityWebRequest.Post("https://www.kevinabsen.com/user_info.php", form)) {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log(www.error);
            } else {
                Debug.Log(www.downloadHandler.text);
                string jsonArray = www.downloadHandler.text;

                callback(jsonArray);
            }
        }
    }

}