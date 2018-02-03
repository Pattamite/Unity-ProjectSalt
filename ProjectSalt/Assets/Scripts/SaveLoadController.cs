using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SaveLoadController : MonoBehaviour {

    private static string PATH_FARM_SLOTS = "/pfs.gd";
    private static string PATH_MAIN_DATA = "/pmd.gd";
    private static string PATH_FARM_DATA = "/pfd.gd";

    private Farm farm;
    private MainGameController mainGameController;

	// Use this for initialization
	void Start () {
        farm = GameObject.FindObjectOfType<Farm>();
        mainGameController = GameObject.FindObjectOfType<MainGameController>();

        LoadGame();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void SaveGame () {
        SaveFarmSlots();
        SaveMainData();
        SaveFarmData();
    }

    public void LoadGame () {
        LoadFarmSlots();
        LoadMainData();
        LoadFarmData();
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

    private void SaveFarmData () {
        FarmSave farmSave = new FarmSave();
        farmSave.playerSubFarmCount = farm.playerSubFarmCount;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + PATH_FARM_DATA);
        bf.Serialize(file, farmSave);
        file.Close();
    }

    private void LoadFarmSlots () {
        if (File.Exists(Application.persistentDataPath + PATH_FARM_SLOTS)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + PATH_FARM_SLOTS, FileMode.Open);
            List<FarmSlotSave> farmSlotSaves = (List<FarmSlotSave>)bf.Deserialize(file);
            file.Close();

            int iter = 0;
            int farmSlotIndex = 0;
            int subFarmIndex = 0;
            int farmSlotSavesCount = farmSlotSaves.Count;

            /*foreach (FarmSlotSave farmSlot in farmSlotSaves) {
                Debug.Log(string.Format("FarmSlot : SubFarm #{0}, FarmSlot #{1}, PlantModelID #{2}," +
                    " IsRotten? {3}, CurrentState {4}, CurrentDayPass {5}",
                    farmSlot.subFarmIndex, farmSlot.farmSlotIndex, farmSlot.plantModelID,
                    farmSlot.isRotten, farmSlot.currentState, farmSlot.currentDayPass));
            }*/

            foreach (Transform child in farm.transform) {
                farmSlotIndex = 0;
                if (child.gameObject.GetComponent<SubFarm>()) {
                    foreach (Transform grandChild in child.gameObject.transform) {
                        FarmSlot farmSlot = grandChild.gameObject.GetComponent<FarmSlot>();
                        if (farmSlot) {
                            if (iter < farmSlotSavesCount && farmSlotSaves[iter].farmSlotIndex == farmSlotIndex 
                                && farmSlotSaves[iter].subFarmIndex == subFarmIndex) {
                                farmSlot.SetPlantFromSave(farmSlotSaves[iter].plantModelID, farmSlotSaves[iter].isRotten,
                                    farmSlotSaves[iter].currentState, farmSlotSaves[iter].currentDayPass);
                                iter++;
                            }
                            else {
                                farmSlot.RemovePlant();
                            }
                            farmSlotIndex++;
                        }
                    }
                    subFarmIndex++;
                }
            }
        }
        else {
            //Debug.LogWarning("SaveLoadController.LoadFarmSlot : " + Application.persistentDataPath + PATH_FARM_SLOTS + " not found.");
        }
    }

    private void LoadMainData () {
        if (File.Exists(Application.persistentDataPath + PATH_MAIN_DATA)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + PATH_MAIN_DATA, FileMode.Open);
            MainDataSave mainDataSave = (MainDataSave)bf.Deserialize(file);
            file.Close();
            /*Debug.Log(string.Format("MainData : Money {0}, {1}:{2} {3}/{4}/{5}",
                mainDataSave.currentMoney, mainDataSave.hour, mainDataSave.minute,
                mainDataSave.day, mainDataSave.month, mainDataSave.year));*/

            mainGameController.SetMoney(mainDataSave.currentMoney);
            mainGameController.UpdateTime(true, new DateTime(mainDataSave.year, mainDataSave.month,
                mainDataSave.day, mainDataSave.hour, mainDataSave.minute, 0));
        }
        else {
            //Debug.LogWarning("SaveLoadController.LoadMainData : " + Application.persistentDataPath + PATH_MAIN_DATA + " not found.");
            mainGameController.UpdateTime(true, DateTime.Now);
        }
    }

    private void LoadFarmData () {
        if (File.Exists(Application.persistentDataPath + PATH_FARM_DATA)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + PATH_FARM_DATA, FileMode.Open);
            FarmSave farmSave = (FarmSave)bf.Deserialize(file);
            file.Close();

            print(farmSave.playerSubFarmCount);
            farm.SetPlayerSubFarmCount(farmSave.playerSubFarmCount);
        }
        else {
        }
    }
}
