using System.Collections;
using UnityEngine;

public class MatchOverUI : MonoBehaviour
{
    public static MatchOverUI Instance;

    [Header("Victory")]
    public GameObject victoryScreen;
    public AudioClip victoryMusic;

    [Header("Defeat")]
    public GameObject defeatScreen;
    public AudioClip defearMusic;

    [Header("Other")]
    public GameObject deathScreen;
    public SettingsUI settingsUI;
    public GameObject buttons;

    public float delayShowingScoreTable = 4f;
    public GameObject scoreTable;

    private AudioSource m_AudioSource;

    private void Awake()
    {
        Instance = this;
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void MatchOver(bool isWin)
    {
        if (isWin)
        {
            victoryScreen.SetActive(true);
            m_AudioSource.PlayOneShot(victoryMusic);
        }
        else
        {
            defeatScreen.SetActive(true);
            m_AudioSource.PlayOneShot(defearMusic);
        }
        deathScreen.SetActive(false);

        StartCoroutine(ShowScoreTable());
    }

    private IEnumerator ShowScoreTable()
    {
        yield return new WaitForSecondsRealtime(delayShowingScoreTable);
        scoreTable.SetActive(true);
        buttons.SetActive(true);
    }

    public void MainMenuButton()
    {
        GameManager.Instance.BackInTitleScreen();
    }

    public void RestartGameButton()
    {
        GameManager.Instance.RestartMatch();
    }

    public void Settings()
    {
        settingsUI.gameObject.SetActive(true);
        settingsUI.SetPreviousUI(gameObject);
        gameObject.SetActive(false);
    }
}
