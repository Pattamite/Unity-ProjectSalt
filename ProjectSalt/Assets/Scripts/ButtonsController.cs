using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsController : MonoBehaviour {

    // Use this for initialization
    public GameObject currentSelectedButton;
    private bool isActive;
    public bool isMainButtonSet;

    private static float activePosition = 69.4f;
    private static float notActivePosition = -500f;

	void Start () {
        if (!isMainButtonSet) {
            Deactivate();
        }
        else {
            Activate();
        }
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

    public void DeSelectAllButton () {
        foreach(Transform child in transform) {
            MarkAsNotSelect(child.gameObject);
        }
        currentSelectedButton = null;
    }

    private void MarkAsSelect (GameObject gameObject) {
        Color color = gameObject.GetComponent<Image>().color;
        gameObject.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0.5f);
    }

    private void MarkAsNotSelect (GameObject gameObject) {
        Color color = gameObject.GetComponent<Image>().color;
        gameObject.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1f);
    }

    public void Activate () {
        isActive = true;
        Vector3 pos = GetComponent<RectTransform>().anchoredPosition;
        GetComponent<RectTransform>().anchoredPosition = new Vector3(pos.x, activePosition, pos.z);
    }

    public void Deactivate () {
        isActive = false;
        Vector3 pos = GetComponent<RectTransform>().anchoredPosition;
        GetComponent<RectTransform>().anchoredPosition = new Vector3(pos.x, notActivePosition, pos.z);
        DeSelectAllButton();
    }

    public bool GetIsActive () {
        return isActive;
    }
}
