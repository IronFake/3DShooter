using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    private GameObject callingUI;

    public Slider masterVolSlider;
    public Slider SFXVolSlider;
    public Slider musicVolSlider;

    private InputHandler m_input;

    private void Awake()
    {
        m_input = InputHandler.Instance;
    }

    private void Start()
    {
        masterVolSlider.value = PlayerPrefs.GetFloat("masterVol");
        musicVolSlider.value = PlayerPrefs.GetFloat("musicVol");
        SFXVolSlider.value = PlayerPrefs.GetFloat("SFXVol");
    }

    public void Update()
    {
        if (m_input.TogglePause)
        {
            callingUI.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void SetPreviousUI(GameObject ui)
    {
        callingUI = ui;
    }

    public void Exit()
    {
        callingUI.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SetMasterVolume(float soundLevel)
    {
        AudioManager.Instance.SetMasterVolume(soundLevel);
    }

    public void SetMusicVolume(float soundLevel)
    {
        AudioManager.Instance.SetMusicVolume(soundLevel);
    }

    public void SetSFXVolume(float soundLevel)
    {
        AudioManager.Instance.SetSFXVolume(soundLevel);
    }
}
