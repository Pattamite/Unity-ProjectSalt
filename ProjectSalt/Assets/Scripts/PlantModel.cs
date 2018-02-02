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

    private void OnValidate () {
        if (stateCount < 0){
            stateCount = 0;
        }

        if (daysToNextState.Length != stateCount){
            Array.Resize<int>(ref daysToNextState, stateCount);
        }
        if (stateImage.Length != stateCount)
        {
            Array.Resize<Sprite>(ref stateImage, stateCount);
        }
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
