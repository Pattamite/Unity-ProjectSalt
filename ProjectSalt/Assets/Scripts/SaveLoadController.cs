using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SaveLoadController : MonoBehaviour {

    private static string PATH_FARM_SLOTS = "/pfs.gd";
    private static string PATH_MAIN_DATA = "/pmd.gd";

    private Farm farm;
    private MainGameController mainGameController;

	// Use this for initialization
	void Start () {
        farm = GameObject.FindObjectOfType<Farm>();
        mainGameController = GameObject.FindObjectOfType<MainGameController>();

    }



    // Update is called once per frame
    void Update () {
		
	}

    public void SaveGame () {
        SaveFarmSlots();
        SaveMainData();
    }

    public void LoadGame () {
        LoadFarmSlots();
        LoadMainData();
    }

    private void SaveFarmSlots () {
        List<FarmSlotSave> farmSlotSaves = new List<FarmSlotSave>();
        int subFarmIndex = 0;
        int farmSlotIndex = 0;
        foreach (Transform child in farm.transform) {
            farmSlotIndex = 0;
            if (child.gameObject.GetComponent<SubFarm>()) {
                foreach (Transform grandChild in child.gameObject.transform) {
                    FarmSlot farmSlot = grandChild.gameObject.GetComponent<FarmSlot>();
                    if (farmSlot) {
                        if (farmSlot.plantModel) {
                            FarmSlotSave farmSlotSave = new FarmSlotSave();
                            farmSlotSave.subFarmIndex = subFarmIndex;
                            farmSlotSave.farmSlotIndex = farmSlotIndex;
                            farmSlotSave.plantModelID = farmSlot.plantModel.id;
                            farmSlotSave.isRotten = farmSlot.isRotten;
                            farmSlotSave.currentState = farmSlot.currentState;
                            farmSlotSave.currentDayPass = farmSlot.currentDayPass;
                            farmSlotSaves.Add(farmSlotSave);
                        }
                        farmSlotIndex++;
                    }
                }
                subFarmIndex++;
            }
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + PATH_FARM_SLOTS);
        bf.Serialize(file, farmSlotSaves);
        file.Close();
    }

    private void SaveMainData () {
        DateTime currentTime = DateTime.Now;
        MainDataSave mainDataSave = new MainDataSave();
        mainDataSave.currentMoney = mainGameController.currentMoney;
        mainDataSave.year = currentTime.Year;
        mainDataSave.month = currentTime.Month;
        mainDataSave.day = currentTime.Day;
        mainDataSave.hour = currentTime.Hour;
        mainDataSave.minute = currentTime.Minute;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + PATH_MAIN_DATA);
        bf.Serialize(file, mainDataSave);
        file.Close();
    }

    private void LoadFarmSlots () {
        if (File.Exists(Application.persistentDataPath + PATH_FARM_SLOTS)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + PATH_FARM_SLOTS, FileMode.Open);
            List<FarmSlotSave> farmSlotSaves = (List<FarmSlotSave>)bf.Deserialize(file);
            file.Close();

            foreach(FarmSlotSave farmSlot in farmSlotSaves) {
                Debug.Log(string.Format("FarmSlot : SubFarm #{0}, FarmSlot #{1}, PlantModelID #{2}," +
                    " IsRotten? {3}, CurrentState {4}, CurrentDayPass {5}",
                    farmSlot.subFarmIndex, farmSlot.farmSlotIndex, farmSlot.plantModelID,
                    farmSlot.isRotten, farmSlot.currentState, farmSlot.currentDayPass));
            }
        }
        else {
            Debug.LogWarning("SaveLoadController.LoadFarmSlot : " + Application.persistentDataPath + PATH_FARM_SLOTS + " not found.");
        }
    }

    private void LoadMainData () {
        if (File.Exists(Application.persistentDataPath + PATH_MAIN_DATA)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + PATH_MAIN_DATA, FileMode.Open);
            MainDataSave mainDataSave = (MainDataSave)bf.Deserialize(file);
            file.Close();
            Debug.Log(string.Format("MainData : Money {0}, {1}:{2} {3}/{4}/{5}",
                mainDataSave.currentMoney, mainDataSave.hour, mainDataSave.minute,
                mainDataSave.day, mainDataSave.minute, mainDataSave.year));
        }
        else {
            Debug.LogWarning("SaveLoadController.LoadMainData : " + Application.persistentDataPath + PATH_MAIN_DATA + " not found.");
        }
    }
}
