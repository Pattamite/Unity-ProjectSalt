using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsController : MonoBehaviour {

    // Use this for initialization
    public GameObject currentSelectedButton;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ButtonSelect (GameObject selectedButton) {
        if (currentSelectedButton != selectedButton) {
            if (currentSelectedButton) {
                MarkAsNotSelect(currentSelectedButton);
            }
            currentSelectedButton = selectedButton;
            MarkAsSelect(selectedButton);
        }
        else {
            MarkAsNotSelect(selectedButton);
            currentSelectedButton = null;
        }
    }

    private void MarkAsSelect (GameObject gameObject) {
        Color color = gameObject.GetComponent<Image>().color;
        gameObject.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0.5f);
    }

    private void MarkAsNotSelect (GameObject gameObject) {
        Color color = gameObject.GetComponent<Image>().color;
        gameObject.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1f);
    }
}
