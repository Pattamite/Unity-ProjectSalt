using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainGameController : MonoBehaviour {

    public int currentMoney { get; private set; }
    public int newDayHour = 6;
    public bool isLoadFinish = false;

    private DateTime lastUpdateTime;
    private Farm farm;
    private bool isTodayGrowth;
    private UIController uiController;
    private DateTime temp;

	// Use this for initialization
	void Start () {
        farm = GameObject.FindObjectOfType<Farm>();
        uiController = GameObject.FindObjectOfType<UIController>();
        isTodayGrowth = false;
        currentMoney = 100;
        temp = DateTime.Now;

        uiController.UpdateMoneyText();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateTime(false, temp);
    }

    public void AddMoney (int value) {
        currentMoney += value;
        uiController.UpdateMoneyText();
    }

    public void ReduceMoney (int value) {
        currentMoney -= value;
        if(currentMoney < 0) {
            currentMoney = 0;
        }
        uiController.UpdateMoneyText();
    }

    public void SetMoney (int value) {
        currentMoney = value;
        if (currentMoney < 0) {
            currentMoney = 0;
        }
        uiController.UpdateMoneyText();
    }

    public void UpdateTime (bool isForceUpdate, DateTime lastSaveTime) {
        if (isLoadFinish || isForceUpdate) {
            if (isForceUpdate) {
                lastUpdateTime = lastSaveTime;
                isLoadFinish = true;
            }

            DateTime currentTime = DateTime.Now;

            int growthTime = (currentTime - lastUpdateTime).Days;

            if ((lastUpdateTime.Hour < newDayHour || lastUpdateTime.Day != currentTime.Day) && currentTime.Hour >= newDayHour) {
                growthTime++;
            }

            if(growthTime > 0) {
                farm.NextDay(growthTime);
            }

            lastUpdateTime = currentTime;
        }
    }
}
