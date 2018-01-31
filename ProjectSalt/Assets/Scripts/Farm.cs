using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Farm : MonoBehaviour {
    [Header("Left Right")]
    public Button leftButton;
    public Button rightButton;

    private int subfarmCount = 0;
    private GameObject[] subFarms;
    private Vector2 activePosition;
    private Vector2 notActivePosition = new Vector2(-1000, 0);
    public int currentActiveSubFarmIndex { get; private set; }
    // Use this for initialization
    void Start () {
        subFarms = new GameObject[transform.childCount];
        checkSubFarmCount();

        currentActiveSubFarmIndex = 0;
        SetSubFarmActive(0);
    }

    private void checkSubFarmCount () {
        foreach (Transform child in transform) {
            if (child.gameObject.GetComponent<SubFarm>()) {
                subFarms[subfarmCount] = child.gameObject;
                subfarmCount++;
            }
        }

        activePosition = subFarms[0].gameObject.GetComponent<RectTransform>().anchoredPosition;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void NextDay () {
        foreach (Transform miniFarm in transform) {
            if (miniFarm.gameObject.GetComponent<SubFarm>()) {
                miniFarm.gameObject.GetComponent<SubFarm>().NextDay();
            }
        }
    }

    public void LeftButtonClick () {
        SetSubFarmActive(Mathf.Clamp(currentActiveSubFarmIndex - 1, 0, subfarmCount - 1));
    }

    public void RightButtonClick () {
        SetSubFarmActive(Mathf.Clamp(currentActiveSubFarmIndex + 1, 0, subfarmCount - 1));
    }

    public void SetSubFarmActive (int value) {
        if(value < subfarmCount && value >= 0) {
            subFarms[currentActiveSubFarmIndex].gameObject.GetComponent<RectTransform>().anchoredPosition = notActivePosition;

            subFarms[value].gameObject.GetComponent<RectTransform>().anchoredPosition = activePosition;
            currentActiveSubFarmIndex = value;
        }
        else {
            Debug.LogError("Farm.SetSubFarmActive : Subfarm #" + value.ToString() + " is not exist.");
        }

        CheckLeftRightButton();
    }

    private void CheckLeftRightButton () {
        if (currentActiveSubFarmIndex <= 0) {
            leftButton.interactable = false;
        }
        else {
            leftButton.interactable = true;
        }

        if (currentActiveSubFarmIndex >= subfarmCount - 1) {
            rightButton.interactable = false;
        }
        else {
            rightButton.interactable = true;
        }
    }
}
