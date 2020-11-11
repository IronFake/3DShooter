using UnityEngine;
using UnityEngine.UI;

public class ScoreboardEntryUI : MonoBehaviour
{
    public Color defaultColor;
    public Color highlightColor;

    public Text positionText;
    public Text nicknameText;
    public Text killsText;
    public Text deathText;

    public Image background;

    /*private void Awake()
    {
        background = GetComponent<Image>();
    }*/

    public void UpdateInfo(ScoreboardEntryData data, bool highlight = false)
    {
        nicknameText.text = data.nickname;
        killsText.text = data.kills.ToString();
        deathText.text = data.death.ToString();

        if (highlight)
            background.color = highlightColor;
        else
        {
            background.color = defaultColor;
        }
    }

    public void ChangePositionText(string number)
    {
        positionText.text = number;
    }
}
