using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenUI : MonoBehaviour
{
    public SettingsUI settingsUI;


    public void StartGame()
    {
        GameManager.Instance.LoadScene();
    }

    public void Settings()
    {
        settingsUI.gameObject.SetActive(true);
        settingsUI.SetPreviousUI(gameObject);
        gameObject.SetActive(false);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
