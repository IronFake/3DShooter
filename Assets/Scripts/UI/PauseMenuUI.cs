using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    public static PauseMenuUI Instance { get; private set; }

    public SettingsUI settingsUI;

    private InputHandler m_input;

    private void Awake()
    {
        Instance = this;
        m_input = InputHandler.Instance;
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (m_input.TogglePause && GameSystem.Instance.IsPause)
        {
            Resume();
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        GameSystem.Instance.IsPause = false;
        GameSystem.Instance.LockCursor();
    }

    public void Restart()
    {
        GameManager.Instance.RestartMatch();
    }

    public void Settings()
    {
        settingsUI.gameObject.SetActive(true);
        settingsUI.SetPreviousUI(gameObject);
        gameObject.SetActive(false);
    }

    public void BackTitleScreen()
    {
        GameManager.Instance.BackInTitleScreen();
    }
}
