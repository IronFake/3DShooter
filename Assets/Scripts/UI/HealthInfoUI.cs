using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthInfoUI : MonoBehaviour
{
    public static HealthInfoUI Instance { get; private set; }

    public Text healthValueText;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateHealthValue(int value)
    {
        healthValueText.text = value.ToString();
    }
}
