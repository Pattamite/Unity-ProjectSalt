using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public Text currentMiniFarmText;
    public Text currentMoneyText;

    private Farm farm;
    private MainGameController mainGameController;

    // Use this for initialization
    private void Awake () {
        farm = GameObject.FindObjectOfType<Farm>();
        mainGameController = GameObject.FindObjectOfType<MainGameController>();
    }

    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void UpdateFarmText () {
        currentMiniFarmText.text = (farm.currentActiveSubFarmIndex + 1).ToString();
    }

    public void UpdateMoneyText () {
        print(mainGameController.currentMoney);
        currentMoneyText.text = mainGameController.currentMoney.ToString();
    }
}
