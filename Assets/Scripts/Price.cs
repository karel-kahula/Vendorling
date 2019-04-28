using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Price : MonoBehaviour
{
    public int Amount;
    public TMPro.TextMeshProUGUI Text;

    // Update is called once per frame
    void Update()
    {
        UpdateAmount();
    }

    void OnValidate() {
        UpdateAmount();
    }

    void UpdateAmount() {
        Text.text = $"${Amount / 100.0:F2}";
    }
}
