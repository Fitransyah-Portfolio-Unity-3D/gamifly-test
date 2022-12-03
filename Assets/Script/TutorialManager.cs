using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tutorial
{
    LoadingScreen,
    TutorialOne,
    TutorialTwo,
    TutorialThree,
    TutorialFour,
    TutorialFive,
    TutorialEnd
}
public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Batter batter;
    [SerializeField] private Pitcher pitcher;

    [SerializeField] private Tutorial currentTutorial;
    [SerializeField] private Animator cameraState;

    private void Start()
    {
        currentTutorial = Tutorial.LoadingScreen;

        batter.gameObject.SetActive(false);
        pitcher.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (currentTutorial == Tutorial.TutorialOne)
        {
            // batsman gameobject active
            // hit UI active
            // camera focus on batsman
            // hit UI is interactable
            // effect on hit button is active
            // tutorial text one welcome...
            // tutorial text two taps the ...

            batter.gameObject.SetActive(true);
            if (pitcher.gameObject.activeSelf == true) pitcher.gameObject.SetActive(false);
        }
        else if (currentTutorial == Tutorial.TutorialTwo)
        {
            // bowler gameobject active
            // camera focus on batsman and bowler
            // ball UI appear
            // tutorial text Come on...
            // there is ball hit mechanic to define hit
            // if no hit twxt Try again ...
            // every hit ball UI change color

            pitcher.gameObject.SetActive(true);
        }
        else if (currentTutorial == Tutorial.TutorialThree)
        {
            // camera move to bowler back
            // batsman gameobject not active
            // target marker gameobjcet appear (UI)
            // crosshair gameobject appear (UI)
            // tutorial text Great ...
            // tutorial text dragthe marker here...

            batter.gameObject.SetActive(false);
        }
        else if (currentTutorial == Tutorial.TutorialFour)
        {
            // tutorial text Good job ...
            // crosshair UI not active
            // power bar UI active with animation
            // if power bar stop in red text Not enough power ...
            // if power bar stop in white text Great ...

        }
        else if (currentTutorial == Tutorial.TutorialFive)
        {
            // camera fadeaway
            // batsman gameobject active
            // crosshair gameobject active
            // ball UI appear
            // every succesfull throw ball UI change color

            batter.gameObject.SetActive(true);
        }
        else if (currentTutorial == Tutorial.TutorialEnd)
        {
            // pick team UI pop up
            // if team pick 
            // exit app

            batter.gameObject.SetActive(false);
            pitcher.gameObject.SetActive(false);
        }
    }

    public void SetTutorial(Tutorial activeTutorial)
    {
        currentTutorial = activeTutorial;
    }
}
