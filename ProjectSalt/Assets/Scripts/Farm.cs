using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void NextDay () {
        foreach (Transform miniFarm in transform) {
            miniFarm.gameObject.GetComponent<SubFarm>().NextDay();
        }
    }
}
