using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        if (AudioController.IsMusicEnable()) audioSource.Play();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadLevel (int value) {
        audioSource.Stop();
        SceneManager.LoadScene(value);
    }
}
