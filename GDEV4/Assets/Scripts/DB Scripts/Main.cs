using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    public static Main Instance;
    
    public Web web;
    public UserInfo userInfo;
    public Login login;
    public Register register;
    public Lobby lobby;

    public GameObject userProfile;
    public bool canCheck = false;

    // Start is called before the first frame update
    private void Start() {
        Instance = this;
        web = GetComponent<Web>();
        userInfo = GetComponent<UserInfo>();
    }


}
