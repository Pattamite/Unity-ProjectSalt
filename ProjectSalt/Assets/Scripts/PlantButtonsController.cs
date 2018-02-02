using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantButtonsController : ButtonsPanelController {
    public static PlantModel GetPlantModel () {
        if (currentSelectedButton) {
            return currentSelectedButton.GetComponent<PlantModel>();
        }
        return null;
    }
}
