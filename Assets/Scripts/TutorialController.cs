using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    [System.Serializable]
    public class TutorialStep {
        public GameObject Sprite;
        public UnityEvent Event;

    }

    public TutorialStep[] Steps;
    private int CurrentStep = 0;
    void Start()
    {

        foreach(var step in Steps)
        {
            step.Sprite.SetActive(false);
        }

        Steps[0].Sprite.SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0)) {
            if (CurrentStep < Steps.Length - 1)
            {
                Steps[CurrentStep].Sprite.SetActive(false);
                CurrentStep++;
                Steps[CurrentStep].Sprite.SetActive(true);
                Steps[CurrentStep].Event.Invoke();
            } 
            else
            {
                SceneManager.LoadScene("Menu");
            }
        }
    }
}
