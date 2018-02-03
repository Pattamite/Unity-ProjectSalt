using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsPanelController : MonoBehaviour {

    // Use this for initialization
    public static GameObject currentSelectedButton;
    public bool isActive { get; private set; }
    public bool isMainButtonSet;

    [Header("All Button")]
    public int buttonCount;
    public GameObject[] buttonObjects;
    [Header("Active Button position")]
    public int buttonOnPanelCount;
    private Vector2[] buttonOnPanelPositions = new Vector2[1];
    [Header("Left Right")]
    public Button leftButton;
    public Button rightButton;
    

    private static float buttonNotInPanelPosition = -1000f;
    private static float activePosition = 69.4f;
    private static float notActivePosition = -1000f;
    private int firstButtonIndexInPanel = 0;

    private void OnValidate () {
        CheckButtonCount();
        CheckButtonOnPanelCount();
    }
    

    void Start () {
        if (!isMainButtonSet) {
            Deactivate();
        }
        else {
            Activate();
        }

        SaveButtonOnPanelPositions();
        MoveButtonToPanel(0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    

    private void SaveButtonOnPanelPositions () {
        for(int i = 0; i < buttonOnPanelCount; i++) {
            buttonOnPanelPositions[i] = buttonObjects[i].GetComponent<RectTransform>().anchoredPosition;
        }
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

    private void CheckButtonCount () {
        if (buttonCount < 0) {
            buttonCount = 0;
        }

        if (buttonObjects.Length != buttonCount) {
            Array.Resize<GameObject>(ref buttonObjects, buttonCount);
        }
    }

    private void CheckButtonOnPanelCount () {
        if (buttonOnPanelCount < 0) {
            buttonOnPanelCount = 0;
        }
        if(buttonOnPanelCount > buttonCount) {
            buttonOnPanelCount = buttonCount;
        }

        if (buttonOnPanelPositions.Length != buttonOnPanelCount) {
            Array.Resize<Vector2>(ref buttonOnPanelPositions, buttonOnPanelCount);
        }
    }

    public void LeftButtonClick () {
        MoveButtonToPanel(Mathf.Clamp(firstButtonIndexInPanel - buttonOnPanelCount, 0, buttonCount - 1));
    }

    public void RightButtonClick () {
        MoveButtonToPanel(Mathf.Clamp(firstButtonIndexInPanel + buttonOnPanelCount, 0, buttonCount - 1));
    }

    public void MoveButtonToPanel (int startIndex) {
        //set out of panel
        for(int i = 0 ; i < buttonOnPanelCount && i + firstButtonIndexInPanel < buttonCount; i++) {
            Vector2 pos = buttonObjects[i + firstButtonIndexInPanel].GetComponent<RectTransform>().anchoredPosition;
            buttonObjects[i + firstButtonIndexInPanel].GetComponent<RectTransform>().anchoredPosition = new Vector2(buttonNotInPanelPosition, pos.y);
        }

        //set in panel
        for (int i = 0; i < buttonOnPanelCount && i + startIndex < buttonCount; i++) {
            buttonObjects[i + startIndex].GetComponent<RectTransform>().anchoredPosition = buttonOnPanelPositions[i];
        }

        firstButtonIndexInPanel = startIndex;
        CheckLeftRightButton();
    }

    private void CheckLeftRightButton () {
        if (firstButtonIndexInPanel <= 0) {
            leftButton.interactable = false;
        }
        else {
            leftButton.interactable = true;
        }

        if (firstButtonIndexInPanel + buttonOnPanelCount >= buttonCount) {
            rightButton.interactable = false;
        }
        else {
            rightButton.interactable = true;
        }
    }
}
