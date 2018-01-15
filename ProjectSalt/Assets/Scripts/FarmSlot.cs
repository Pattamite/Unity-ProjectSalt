using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmSlot : MonoBehaviour {

    static private Color defaultColor = Color.white;
    private bool hasPlant = false;
    private int currentState = 0;
    private PlantButtonsController plantButtonsController;
    private Text text;

	// Use this for initialization
	void Start () {
        plantButtonsController = GameObject.FindObjectOfType<PlantButtonsController>();
        text = GetComponentInChildren<Text>();

        UpdateText();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick () {
        if (plantButtonsController.GetIsActive() && plantButtonsController.currentSelectedButton) {
            AddPlant(plantButtonsController.currentSelectedButton);
        }
    }


    private void AddPlant (GameObject plant) {
        Color plantColor = plant.GetComponent<Image>().color;

        if (hasPlant) {
            Debug.Log("This slot already has plant.");
        }
        else {
            gameObject.GetComponent<Image>().color = new Color(plantColor.r, plantColor.g, plantColor.b, 1f); ;
            hasPlant = true;
        }
        UpdateText();
    }

    public void RemovePlant () {
        gameObject.GetComponent<Image>().color = defaultColor;
        currentState = 0;
        hasPlant = false;
        UpdateText();
    }

    public void PlantGrowth () {
        if (hasPlant) {
            currentState++;
            UpdateText();
        }
    }

    private void UpdateText () {
        if (hasPlant) {
            text.text = currentState.ToString();
        }
        else {
            text.text = " ";
        }
    }
}
