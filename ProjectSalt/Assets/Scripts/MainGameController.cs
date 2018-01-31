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

	// Use this for initialization
	void Start () {
        farm = GameObject.FindObjectOfType<Farm>();
        isTodayGrowth = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addMoney (int value) {
        currentMoney += value;
    }

    public void reduceMoney (int value) {
        currentMoney -= value;
        if(currentMoney < 0) {
            currentMoney = 0;
        }
    }

    private void UpdateTime () {
        DateTime currentTime = DateTime.Now;
    }
}
