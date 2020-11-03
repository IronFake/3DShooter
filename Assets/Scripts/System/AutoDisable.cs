using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AutoDisable : MonoBehaviour
{

    public float time;

    private void OnEnable()
    {
        Invoke("Disable", time);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }

}
