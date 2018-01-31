using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public Text currentMiniFarmText;

    private Farm farm;
    private MainGameController mainGameController;

	// Use this for initialization
	void Start () {
        farm = GameObject.FindObjectOfType<Farm>();
        mainGameController = GameObject.FindObjectOfType<MainGameController>();

    }
	
	// Update is called once per frame
	void Update () {
        currentMiniFarmText.text = (farm.currentActiveSubFarmIndex + 1).ToString();

    }
}
