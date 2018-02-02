using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmSlot : MonoBehaviour {

    static private Color hasPlantColor = Color.white;
    static private Color dontHasPlantColor = new Color(0, 0, 0, 0);
    private bool isRotten;
    private int currentState = 0;
    private int currentDayPass = 0;
    private PlantButtonsController plantButtonsController;
    private ToolsButtonController toolsButtonController;
    private Text text;
    private string plantImageName = "PlantImage";
    private Image plantImage;
    private PlantModel plantModel;

    // Use this for initialization
    void Awake () {
        if (transform.Find(plantImageName)) {
            plantImage = transform.Find(plantImageName).GetComponent<Image>();
        }
        else {
            Debug.LogWarning("FarmSlot.Awake : Child '" + plantImageName + "' not found");
        }
    }

    void Start () {
        plantButtonsController = GameObject.FindObjectOfType<PlantButtonsController>();
        toolsButtonController = GameObject.FindObjectOfType<ToolsButtonController>();
        text = GetComponentInChildren<Text>();
        UpdatePlant();
    }

    // Update is called once per frame
    void Update () {

    }

    public void OnClick () {

        if (PlantButtonsController.GetPlantModel()) {
            AddPlant(PlantButtonsController.GetPlantModel());
        }
        else if (ToolsButtonController.GetCommand() != null) {
            Command(ToolsButtonController.GetCommand());
        }
    }

    private void Command (string command) {
        if (command == ToolsButtonController.COMMAND_REMOVE) {
            RemovePlant();
        }
        else {
            Debug.LogWarning("FarmSlot.Command : Unknown Command -> " + command);
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
        print("remove");
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

            plantImage.color = hasPlantColor;
            text.text = currentDayPass.ToString(); //for testing Only
        }
        else {
            text.text = " "; //for testing Only
            plantImage.sprite = null;
            plantImage.color = dontHasPlantColor;
        }
    }
}
