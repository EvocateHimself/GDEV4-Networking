using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class TabInputField : MonoBehaviour {

    // Tab to change input fields
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
           if (Input.GetKey(KeyCode.LeftShift)) {
                if (EventSystem.current.currentSelectedGameObject != null) {
                    Selectable selectable = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
                    if (selectable != null) {
                        selectable.Select();
                    }     
                }
            } else {
                if (EventSystem.current.currentSelectedGameObject != null) {
                    Selectable selectable = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
                    if (selectable != null) {
                        selectable.Select();
                    }
                }
            }
        }
    }
}
