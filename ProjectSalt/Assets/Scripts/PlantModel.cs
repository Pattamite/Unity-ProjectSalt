using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantModel : MonoBehaviour {
    public string plantName;
    [Header("Don't change this if it's not necessary")]
    public int id;
    [Header("States (Final state can be harvested.) ")]
    public int stateCount;
    public int[] daysToNextState;
    public Sprite[] stateImage;
    [Header("Rotten State")]
    public Sprite rottenImage;
    [Header("Price")]
    public int buyPrice;
    public int sellPrice;
    public int sellPriceRandomRange;

    
    private string plantImageName = "PlantButtonImage";
    private string plantTextName = "PriceText";
    private Image plantImage;
    private Text priceText;
    private MainGameController mainGameController;

    void Awake () {
        if(transform.Find(plantImageName)) {
            plantImage = transform.Find(plantImageName).GetComponent<Image>();
            plantImage.sprite = stateImage[stateCount - 1];
        }
        else {
            Debug.LogWarning("PlantModel.Awake : Child '" + plantImageName + "' not found");
        }

        if (transform.Find(plantTextName)) {
            priceText = transform.Find(plantTextName).GetComponent<Text>();
        }
        else {
            Debug.LogWarning("PlantModel.Awake : Child '" + plantTextName + "' not found");
        }
    }

    private void OnValidate () {
        if (stateCount < 1){
            stateCount = 1;
        }

        if (daysToNextState.Length != stateCount){
            Array.Resize<int>(ref daysToNextState, stateCount);
        }
        if (stateImage.Length != stateCount)
        {
            Array.Resize<Sprite>(ref stateImage, stateCount);
        }
    }

    public int GetSellPrice () {
        return sellPrice + UnityEngine.Random.Range(0, sellPriceRandomRange + 1);
    }


    // Use this for initialization
    void Start () {
        mainGameController = GameObject.FindObjectOfType<MainGameController>();
        priceText.text = buyPrice.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        CheckPriceText();
	}

    private void CheckPriceText () {
        if (mainGameController.currentMoney >= buyPrice) {
            priceText.color = Color.black;
        }
        else {
            priceText.color = Color.red;
        }
    }
}
