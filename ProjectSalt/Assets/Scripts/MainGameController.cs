using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainGameController : MonoBehaviour {

    public int currentMoney { get; private set; }
    public int newDayHour = 6;

    private DateTime lastUpdateTime;
    private Farm farm;
    private bool isTodayGrowth;
    private UIController uiController;

	// Use this for initialization
	void Start () {
        farm = GameObject.FindObjectOfType<Farm>();
        uiController = GameObject.FindObjectOfType<UIController>();
        isTodayGrowth = false;
        currentMoney = 100;
        uiController.UpdateMoneyText();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addMoney (int value) {
        currentMoney += value;
        uiController.UpdateMoneyText();
    }

    public void reduceMoney (int value) {
        currentMoney -= value;
        if(currentMoney < 0) {
            currentMoney = 0;
        }
        uiController.UpdateMoneyText();
    }

    private void UpdateTime () {
        DateTime currentTime = DateTime.Now;
    }
}
