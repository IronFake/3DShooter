using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    public static Scoreboard Instance;

    
    public GameObject scoreboardEntryTemplate;
    public Transform entryContainer;

    private Dictionary<GameObject, ScoreboardEntryData> entryDataList = new Dictionary<GameObject, ScoreboardEntryData>();
    private List<ScoreboardEntryUI> entryUIList = new List<ScoreboardEntryUI>();

    private int numberOfBots = 1;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

    }

    public void Initialize(int lines)
    {
        for (int i = 1; i < lines + 1; i++)
        {
            var scoreboardEntryUI = Instantiate(scoreboardEntryTemplate, entryContainer).
                GetComponent<ScoreboardEntryUI>();
            switch(i)
            {
                default:
                    scoreboardEntryUI.ChangePositionText(i + "TH");
                    break;
                case 1:
                    scoreboardEntryUI.ChangePositionText("1ST");
                    break;
                case 2:
                    scoreboardEntryUI.ChangePositionText("2ND");
                    break;
                case 3:
                    scoreboardEntryUI.ChangePositionText("3RD");
                    break;
            }
           
            entryUIList.Add(scoreboardEntryUI);
            
        }
        CreateScoreboardEntryData();
    }

    private void CreateScoreboardEntryData()
    {
        var actors = GameSystem.Instance.ActorsKills.Keys;

        foreach (var actor in actors)
        {
            ScoreboardEntryData sed = new ScoreboardEntryData();
            if (actor.CompareTag("Player"))
            {
                sed.nickname = "YOU";
            }
            else
            {
                sed.nickname = "BOT " + numberOfBots;
                numberOfBots++;
            }
            entryDataList.Add(actor, sed);
        }
    }

    public void UpdateScoreBoard()
    {
        //Sort dictionaty by kills 
        var items =  from pair in GameSystem.Instance.ActorsKills
                     orderby pair.Value descending
                     select pair;

        int i = 0;
        foreach(KeyValuePair<GameObject, int> pair in items)
        {
            ScoreboardEntryData sed = entryDataList[pair.Key];
            sed.kills = pair.Value;
            sed.death = GameSystem.Instance.ActorsDeaths[pair.Key];
   
            if (pair.Key.CompareTag("Player"))
            {
                entryUIList[i].UpdateInfo(sed, true);
            }
            else
            {
                entryUIList[i].UpdateInfo(sed, false);
            }
            i++;
        }


    }
}
