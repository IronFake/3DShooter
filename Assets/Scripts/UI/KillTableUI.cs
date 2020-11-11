using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTableUI : MonoBehaviour
{
    public static KillTableUI Instance { get; private set; }

    public GameObject cardPrefab;

    [Header("Player card")]
    public Sprite playerIcon;
    public Color playerTextColor;

    [Header("Enemy card")]
    public Sprite enemyIcon;
    public Color enemyTextColor;

    Dictionary<GameObject, ScoreCardUI> scoreTable = new Dictionary<GameObject, ScoreCardUI>();

    private void Awake()
    {
        Instance = this;
    }

    public void InitScoreTable()
    {
        var actors = GameSystem.Instance.ActorsKills;
        foreach (var actor in actors)
        {
            ScoreCardUI scUI = Instantiate(cardPrefab, transform).GetComponent<ScoreCardUI>();
            if (actor.Key.gameObject.tag == "Player")
            {
                scUI.score.text = "0";
                scUI.score.color = playerTextColor;
                scUI.icon.sprite = playerIcon;
            }
            else
            {
                scUI.score.text = "0";
                scUI.score.color = enemyTextColor;
                scUI.icon.sprite = enemyIcon;
            }

            scoreTable.Add(actor.Key, scUI);
        }
    }

    public void UpdateScore(GameObject go, int value)
    {
        if (scoreTable.ContainsKey(go))
        {
            scoreTable[go].score.text = value.ToString();
        }
    }
}
