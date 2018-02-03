using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmSlot : MonoBehaviour {

    static private Color hasPlantColor = Color.white;
    static private Color dontHasPlantColor = new Color(0, 0, 0, 0);

    public AudioClip plantSound;
    public AudioClip harvestSound;
    public AudioClip removeSound;

    public Image buttonImage;
    public Color normalColor;
    public Color harvestColor;


    public bool isRotten { get; private set; }
    public int currentState { get; private set; }
    public int currentDayPass { get; private set; }
    private MainGameController mainGameController;
    private string plantImageName = "PlantImage";
    private Image plantImage;
    public PlantModel plantModel { get; private set; }
    private static GameObject plantPanel;
    private static SaveLoadController saveLoadController;
    private AudioSource audioSource;

    // Use this for initialization
    void Awake () {
        if (transform.Find(plantImageName)) {
            plantImage = transform.Find(plantImageName).GetComponent<Image>();
        }
        else {
            Debug.LogWarning("FarmSlot.Awake : Child '" + plantImageName + "' not found");
        }
        plantPanel = GameObject.Find("Plant Panel");
        saveLoadController = GameObject.FindObjectOfType<SaveLoadController>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start () {
        mainGameController = GameObject.FindObjectOfType<MainGameController>();
        UpdatePlant();
    }

    // Update is called once per frame
    void Update () {

    }

    public void SetPlantFromSave (int plantModelID, bool isRotten, int currentState, int currentDayPass){
        foreach(Transform child in plantPanel.transform) {
            PlantModel plant = child.gameObject.GetComponent<PlantModel>();
            if (plant) {
                if(plant.id == plantModelID) {
                    this.plantModel = plant;
                    break;
                }
            }
        }

        this.isRotten = isRotten;
        this.currentState = currentState;
        this.currentDayPass = currentDayPass;
        UpdatePlant();
    }

    public void OnClick () {
        if (PlantButtonsController.GetPlantModel()) {
            BuyPlant(PlantButtonsController.GetPlantModel());
        }
        else if (ToolsButtonController.GetCommand() != null) {
            Command(ToolsButtonController.GetCommand());
        }
    }

    private void Command (string command) {
        if (command == ToolsButtonController.COMMAND_REMOVE) {
            RemovePlant(true);
        }
        else if (command == ToolsButtonController.COMMAND_HARVEST) {
            HarvestPlant();
        }
        else {
            Debug.LogWarning("FarmSlot.Command : Unknown Command -> " + command);
        }
    }

    public void BuyPlant (PlantModel plant) {
        if(mainGameController.currentMoney >= plant.buyPrice && !plantModel) {
            mainGameController.ReduceMoney(plant.buyPrice);
            AddPlant(plant);
            audioSource.clip = plantSound;
            if (AudioController.IsAudioEnable()) audioSource.Play();
        }
        else {
            Debug.LogWarning("Not Enough Money.");
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
        saveLoadController.SaveFarmSlots();
    }

    public void HarvestPlant () {
        if (currentState == (plantModel.stateCount - 1) && !isRotten) {
            mainGameController.AddMoney(plantModel.GetSellPrice());
            RemovePlant(false);
        }

        audioSource.clip = harvestSound;
        if (AudioController.IsAudioEnable()) audioSource.Play();
    }

    public void RemovePlant (bool isPlaySound) {
        if (isPlaySound && plantModel) {
            audioSource.clip = removeSound;
            if (AudioController.IsAudioEnable()) audioSource.Play();
        }

        plantModel = null;
        currentState = 0;
        currentDayPass = 0;
        isRotten = false;
        UpdatePlant();
        saveLoadController.SaveFarmSlots();
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

                if (currentState == (plantModel.stateCount - 1)) {
                    buttonImage.color = harvestColor;
                }
                else {
                    buttonImage.color = normalColor;
                }

            }
            else {
                plantImage.sprite = plantModel.rottenImage;
            }

            plantImage.color = hasPlantColor;
            //text.text = currentDayPass.ToString(); //for testing Only
        }
        else {
            //text.text = " "; //for testing Only
            plantImage.sprite = null;
            plantImage.color = dontHasPlantColor;
            buttonImage.color = normalColor;
        }
    }
}
