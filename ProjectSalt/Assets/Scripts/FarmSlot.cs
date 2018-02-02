using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmSlot : MonoBehaviour {

    static private Color defaultColor = Color.white;
    private bool isRotten;
    private int currentState = 0;
    private int currentDayPass = 0;
    private PlantButtonsController plantButtonsController;
    private Text text;
    private string plantImageName = "PlantImage";
    private Image plantImage;
    private PlantModel plantModel;

    // Use this for initialization
    void Awake (){
        plantImage = transform.Find(plantImageName).GetComponent<Image>();
    }

    void Start () {
        plantButtonsController = GameObject.FindObjectOfType<PlantButtonsController>();
        text = GetComponentInChildren<Text>();
        UpdatePlant();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick () {
        if (PlantButtonsController.currentSelectedButton) {
            PlantModel plant = PlantButtonsController.currentSelectedButton.GetComponent<PlantModel>();
            if (plant) {
                AddPlant(plant);
            }
                
        }
    }


    private void AddPlant (PlantModel plant) {
        if (plantModel) {
            Debug.Log("This slot already has plant.");
        }
        else {
            plantModel = plant;
            currentState = 0;
            currentDayPass = 0;
            isRotten = false;
        }
        UpdatePlant();
    }

    public void RemovePlant () {
        plantModel = null;
        currentState = 0;
        currentDayPass = 0;
        isRotten = false;
        UpdatePlant();
    }

    public void PlantGrowth (int dayPass) {
        if (plantModel) {
            currentDayPass += dayPass;
            UpdatePlant();
        }
    }

    private void UpdatePlant () {
        if (plantModel) {
            if (!isRotten) {
                while (currentDayPass >= plantModel.daysToNextState[currentState] && currentDayPass >= 0) {
                    currentDayPass -= plantModel.daysToNextState[currentState];
                    currentState++;
                    if (currentState >= plantModel.stateCount) {
                        break;
                    }
                }

                if (currentState >= plantModel.stateCount) {
                    isRotten = true;
                    plantImage.sprite = plantModel.rottenImage;
                }
                else {
                    plantImage.sprite = plantModel.stateImage[currentState];
                }
                
            }
            else {
                plantImage.sprite = plantModel.rottenImage;
            }   

            text.text = currentDayPass.ToString(); //for testing Only
        }
        else {
            text.text = " "; //for testing Only
        }
    }
}
