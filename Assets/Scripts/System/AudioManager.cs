using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioMixer mainMixer;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        mainMixer.SetFloat("masterVol", PlayerPrefs.GetFloat("masterVol"));
        mainMixer.SetFloat("musicVol", PlayerPrefs.GetFloat("musicVol"));
        mainMixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SFXVol"));
    }

    public void SetMasterVolume(float soundLevel)
    {
        mainMixer.SetFloat("masterVol", soundLevel);
        PlayerPrefs.SetFloat("masterVol", soundLevel);
    }

    public void SetMusicVolume(float soundLevel)
    {
        mainMixer.SetFloat("musicVol", soundLevel);
        PlayerPrefs.SetFloat("musicVol", soundLevel);
    }

    public void SetSFXVolume(float soundLevel)
    {
        mainMixer.SetFloat("SFXVol", soundLevel);
        PlayerPrefs.SetFloat("SFXVol", soundLevel);
    }

}
