using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance { get; private set; }

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public int numberOfEnemies;
    public int maxPointsToWin;
    
    [Header("UI")]
    public GameObject scoretableContent;
    public GameObject pauseUI;

    public Dictionary<GameObject, int> ActorsKills { get; private set; }
    public Dictionary<GameObject, int> ActorsDeaths { get; private set; }
    public bool IsPause { get; set; }
    public bool IsMatchOver { get; private set; }

    private SpawnPointManager m_spawnPointManager;
    private InputHandler m_input;

    private void Awake()
    {
        Instance = this;
        m_input = InputHandler.Instance;

        ActorsKills = new Dictionary<GameObject, int>();
        ActorsDeaths = new Dictionary<GameObject, int>();
    }

    private void Start()
    {
        m_spawnPointManager = SpawnPointManager.Instance;
        StartMatch();

        Scoreboard.Instance.Initialize(ActorsKills.Count);
        Scoreboard.Instance.UpdateScoreBoard();
        KillTableUI.Instance.InitScoreTable();
    }

    private void Update()
    {
        if (IsMatchOver)
            return;

        if (m_input.TogglePause && !IsPause) 
        {
            UnlockCursor();
            pauseUI.SetActive(true);
            IsPause = true;
        }

        if (m_input.ToggleShowingScoreboard == true)
        {
            scoretableContent.SetActive(true);
        }
        else
        {
            scoretableContent.SetActive(false);
        }
    }

    public void StartMatch()
    {
        GameObject player = Instantiate(playerPrefab, m_spawnPointManager.GetRandomSpawnPoint(true), playerPrefab.transform.rotation);
        SceneManager.MoveGameObjectToScene(player, gameObject.scene);
        ActorsKills.Add(player, 0);
        ActorsDeaths.Add(player, 0);
        for (int i = 0; i < numberOfEnemies; i++)
        {
            GameObject go = Instantiate(enemyPrefab, m_spawnPointManager.GetRandomSpawnPoint(true), enemyPrefab.transform.rotation);
            SceneManager.MoveGameObjectToScene(go, gameObject.scene);
            ActorsKills.Add(go, 0);
            ActorsDeaths.Add(go, 0);
        }
    }

    public void UpdatePoints(GameObject kill, GameObject dead)
    {
        if (ActorsKills.ContainsKey(kill))
        {
            ActorsKills[kill] += 1;
            KillTableUI.Instance.UpdateScore(kill, ActorsKills[kill]);
            if (ActorsKills[kill] == maxPointsToWin)
            {
                MatchOver(kill);
            }

            ActorsDeaths[dead] += 1;
            Scoreboard.Instance.UpdateScoreBoard();
        }
    }

    private void MatchOver(GameObject winner)
    {
        IsMatchOver = true;
        UnlockCursor();
        
        if (winner.tag == "Player")
        {
            MatchOverUI.Instance.MatchOver(true);
        }
        else
        {
            MatchOverUI.Instance.MatchOver(false);
        }

        Time.timeScale = 0f;
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
