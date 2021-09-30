using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour {
    
    private string playerOneChoice, playerTwoChoice;
    private GameManager gameManager;
    
    private void Awake() {
        gameManager = GetComponent<GameManager>();
    }

    // Check which element the player has chosen and send it to the server
    public void GetChoice(GameObject sender) {
        string choiceName = UnityEngine.EventSystems.
        EventSystem.current.currentSelectedGameObject.name;

        Debug.Log("Player selected: " + choiceName);

        Elements selectedElement = Elements.NONE;

        switch(choiceName) {

            case "Air":
                selectedElement = Elements.AIR;
                break;

            case "Fire":
                selectedElement = Elements.FIRE;
                break;

            case "Earth":
                selectedElement = Elements.EARTH;
                break;

            case "Water":
                selectedElement = Elements.WATER;
                break;
        }

        gameManager.SetPlayerOneChoice(selectedElement);
    }
}
