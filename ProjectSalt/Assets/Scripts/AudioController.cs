using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour {

    public static string IS_AUDIO_ENABLE = "isAudioEnable";
    public static string IS_MUSIC_ENABLE = "isMusicEnable";

    public Image audioButton;
    public Image musicButton;

    public Sprite audioEnable;
    public Sprite audioDisable;
    public Sprite musicEnable;
    public Sprite musicDisable;

    private AudioSource audioSource;

    public static bool IsAudioEnable () {
        return PlayerPrefs.GetInt(IS_AUDIO_ENABLE) == 1;
    }

    public static bool IsMusicEnable () {
        return PlayerPrefs.GetInt(IS_MUSIC_ENABLE) == 1;
    }

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();

        if (!PlayerPrefs.HasKey(IS_AUDIO_ENABLE)) {
            PlayerPrefs.SetInt(IS_AUDIO_ENABLE, 1);
        }

        if (!PlayerPrefs.HasKey(IS_MUSIC_ENABLE)) {
            PlayerPrefs.SetInt(IS_MUSIC_ENABLE, 1);
        }

        CheckAudioButton();
        CheckMusicButton();
    }
	
	public void AudioToggle () {
        if (IsAudioEnable()) {
            PlayerPrefs.SetInt(IS_AUDIO_ENABLE, 0);
        }
        else {
            PlayerPrefs.SetInt(IS_AUDIO_ENABLE, 1);
        }

        CheckAudioButton();
    }

    public void MusicToggle () {
        if (IsMusicEnable()) {
            PlayerPrefs.SetInt(IS_MUSIC_ENABLE, 0);
        }
        else {
            PlayerPrefs.SetInt(IS_MUSIC_ENABLE, 1);
        }
        CheckMusicButton();
    }

    private void CheckAudioButton () {
        if (IsAudioEnable()) {
            audioButton.sprite = audioEnable;
        }
        else {
            audioButton.sprite = audioDisable;
        }

        
    }

    private void CheckMusicButton () {
        if (IsMusicEnable()) {
            musicButton.sprite = musicEnable;
            audioSource.Play();
        }
        else {
            musicButton.sprite = musicDisable;
            audioSource.Pause();
        }
    }
}
