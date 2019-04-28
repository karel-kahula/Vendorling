using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDScore : MonoBehaviour
{
    public int Score;
    public TMPro.TextMeshProUGUI Text;

    // Update is called once per frame
    void Update()
    {
        UpdateScore();
    }

    void OnValidate() {
        UpdateScore();
    }

    void UpdateScore() {
        Text.text = $"{Score}";
    }
}