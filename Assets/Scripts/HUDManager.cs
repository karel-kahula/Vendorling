using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public HUDScore ScoreElement;
    public Price PriceElement;
    public HealthMeter HealthMeter;
    public Animator BGAnimator;

    public int Score;
    public int Price;
    public float Health;
    
    void Update()
    {
       ScoreElement.Score = Score;
       PriceElement.Amount = Price;
       HealthMeter.Health = Health; 
    }

    public void ShowGameOverScreen() {
        HealthMeter.gameObject.SetActive(false);
    }

    public void TriggerSuccess() {
        BGAnimator.SetTrigger("Success");
    }

    public void TriggerFailure() {
        BGAnimator.SetTrigger("Fail");
    }
}
