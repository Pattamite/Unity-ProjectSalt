using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FarmSlotSave{
    public int subFarmIndex;
    public int farmSlotIndex;
    public int plantModelID;
    public bool isRotten;
    public int currentState;
    public int currentDayPass;
}

[System.Serializable]
public class MainDataSave {
    public int currentMoney;

    public int year;
    public int month;
    public int day;
    public int hour;
    public int minute;
}

[System.Serializable]
public class FarmSave {
    public int playerSubFarmCount;
}