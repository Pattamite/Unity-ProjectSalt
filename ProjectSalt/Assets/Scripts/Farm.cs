using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Farm : MonoBehaviour {

    [Header("SubFarm")]
    public int subFarmBasePrice = 50;
    [Header("Buy More Frm Button")]
    public Button buyFarmButton;
    public Text buyFarmText;
    public Text buyFarmPriceText;
    [Header("Left Right")]
    public Button leftButton;
    public Button rightButton;

    public int playerSubFarmCount { get; private set; }
    private int subfarmCount = 0;
    private GameObject[] subFarms;
    private Vector2 activePosition;
    private Vector2 notActivePosition = new Vector2(-1000, 0);
    public int currentActiveSubFarmIndex { get; private set; }
    private UIController uiController;
    private MainGameController mainGameController;
    private SaveLoadController saveLoadController;
    // Use this for initialization
    void Start () {
        playerSubFarmCount = 1;
        subFarms = new GameObject[transform.childCount];
        uiController = GameObject.FindObjectOfType<UIController>();
        mainGameController = GameObject.FindObjectOfType<MainGameController>();
        saveLoadController = GameObject.FindObjectOfType<SaveLoadController>();

        CheckSubFarmCount();

        currentActiveSubFarmIndex = 0;
        SetSubFarmActive(0);
    }

    private void CheckSubFarmCount () {
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
        CheckBuyFarm();
    }

    public void CheckBuyFarm () {
        if (playerSubFarmCount >= subfarmCount) {
            buyFarmButton.interactable = false;
            buyFarmText.text = "Farm Area are Sold out.";
            buyFarmPriceText.text = "";
        }
        else {
            buyFarmPriceText.text = GetSubFarmPrice().ToString();
            if (mainGameController.currentMoney < GetSubFarmPrice()) {
                buyFarmPriceText.color = Color.red;
            }
            else {
                buyFarmPriceText.color = Color.black;
            }
        }
    }

    public void NextDay (int dayPass) {
        foreach (Transform miniFarm in transform) {
            if (miniFarm.gameObject.GetComponent<SubFarm>()) {
                miniFarm.gameObject.GetComponent<SubFarm>().NextDay(dayPass);
            }
        }
        saveLoadController.SaveFarmSlots();
    }

    public void LeftButtonClick () {
        SetSubFarmActive(Mathf.Clamp(currentActiveSubFarmIndex - 1, 0, subfarmCount - 1));
    }

    public void RightButtonClick () {
        SetSubFarmActive(Mathf.Clamp(currentActiveSubFarmIndex + 1, 0, subfarmCount - 1));
    }

    public void SetSubFarmActive (int value) {
        //print("set" + value);
        if(value < subfarmCount && value >= 0) {
            subFarms[currentActiveSubFarmIndex].gameObject.GetComponent<RectTransform>().anchoredPosition = notActivePosition;

            subFarms[value].gameObject.GetComponent<RectTransform>().anchoredPosition = activePosition;
            currentActiveSubFarmIndex = value;
        }
        else {
            Debug.LogError("Farm.SetSubFarmActive : Subfarm #" + value.ToString() + " is not exist.");
        }

        CheckLeftRightButton();
        uiController.UpdateFarmText();
    }

    private void CheckLeftRightButton () {
        if (currentActiveSubFarmIndex <= 0) {
            leftButton.interactable = false;
        }
        else {
            leftButton.interactable = true;
        }

        if (currentActiveSubFarmIndex >= (subfarmCount - 1)
            || currentActiveSubFarmIndex >= (playerSubFarmCount - 1)) {
            rightButton.interactable = false;
        }
        else {
            rightButton.interactable = true;
        }
    }

    public bool CanBuySubFarm () {
        return playerSubFarmCount < subfarmCount;
    }

    public int GetSubFarmPrice () {
        return subFarmBasePrice * playerSubFarmCount * playerSubFarmCount;
    }

    public void BuySubFarm () {
        int currentPrice = GetSubFarmPrice();
        if (mainGameController.currentMoney >= currentPrice) {
            mainGameController.ReduceMoney(currentPrice);
            AddSubFarm();
        }
        else {
            Debug.LogWarning("Not Enough Money.");
        }
    }

    private void AddSubFarm () {
        playerSubFarmCount += 1;
        CheckLeftRightButton();
        saveLoadController.SaveFarmData();
    }

    public void SetPlayerSubFarmCount (int value) {
        playerSubFarmCount = value;
        if(playerSubFarmCount < 1) {
            playerSubFarmCount = 1;
        }
        CheckLeftRightButton();
    }
}
