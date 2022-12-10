using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    [SerializeField] private GameObject targetingSystem;
    [SerializeField] private Move crosshairObject;
    [SerializeField] private GameObject powerBarSystem;
    [SerializeField] private PowerBar powerBar;

    [SerializeField] private Tutorial currentTutorial;
    [SerializeField] private Animator cameraState;

    [SerializeField] float throwingTime = 10f;

    [SerializeField] int tutorialOneCount;
    [SerializeField] int tutorialTwoCount;
    [SerializeField] int tutorialFiveCount;

    bool isTutorialTwoOn;
    bool targetLocked;

    public event Action OnTutorialStart;
    public event Action OnLoadingScreenComplete;
    public event Action OnTutorialOneComplete;
    public event Action OnTutorialTwoProgress;
    public event Action OntutorialTwoComplete;
    public event Action OnTutorialFiveStarted;
    public event Action OnTutorialFiveComplete;
    public event Action<Tutorial> OnTutorialUpdateForButton;
    private void Start()
    {
        currentTutorial = Tutorial.LoadingScreen;
       

        cameraState = GetComponent<Animator>();

        batter.gameObject.SetActive(false);
        pitcher.gameObject.SetActive(false);

        tutorialOneCount = 0;
        tutorialTwoCount = 0;

        batter.OnTutorialOneCount += IncrementTutorialOneCount;
        pitcher.OnTutorialTwoCount += IncrementTutorialTwoCount;
        crosshairObject.OnTargetLock += LockingTarget;
        powerBar.OnBarStop += DisablePowerBar;

        StartCoroutine(LoadingScreenRoutine());

        if (OnTutorialUpdateForButton != null)
        {
            OnTutorialUpdateForButton(currentTutorial);
        }
    }

    private void OnDestroy()
    {
        // TO DO
        batter.OnTutorialOneCount -= IncrementTutorialOneCount;
        pitcher.OnTutorialTwoCount -= IncrementTutorialTwoCount;
        crosshairObject.OnTargetLock -= LockingTarget;
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
            if (OnTutorialUpdateForButton != null)
            {
                OnTutorialUpdateForButton(currentTutorial);
            }
            
            
            batter.gameObject.SetActive(true);
            if (pitcher.gameObject.activeSelf == true) pitcher.gameObject.SetActive(false);
            batter.EnableInputAction();
            cameraState.Play("TutorialOne");

            if (tutorialOneCount >= 3 && !batter.IsBatterHitting())
            {
                SetTutorial(Tutorial.TutorialTwo);
                isTutorialTwoOn = true;

                if (OnTutorialOneComplete != null)
                {
                    OnTutorialOneComplete();
                }
            }           
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
            if (OnTutorialUpdateForButton != null)
            {
                OnTutorialUpdateForButton(currentTutorial);
            }

            if (tutorialTwoCount >= 3 && !pitcher.IsPitcherThrowing())
            {
                SetTutorial(Tutorial.TutorialThree);
                isTutorialTwoOn = false;

                if (OntutorialTwoComplete != null)
                {
                    OntutorialTwoComplete();
                }
            }

            throwingTime -= Time.deltaTime;

            if (isTutorialTwoOn && !pitcher.IsPitcherThrowing() && throwingTime <= 0f)
            {
                if (OnTutorialTwoProgress != null)
                {
                    OnTutorialTwoProgress();
                }

                throwingTime = 10f;
            }

            pitcher.gameObject.SetActive(true);
            pitcher.DisableInputAction();
            cameraState.Play("TutorialTwo");
        }
        else if (currentTutorial == Tutorial.TutorialThree)
        {
            // camera move to bowler back
            // batsman gameobject not active
            // target marker gameobjcet appear (UI)
            // crosshair gameobject appear (UI)
            // tutorial text Great ...
            // tutorial text dragthe marker here...
            if (OnTutorialUpdateForButton != null)
            {
                OnTutorialUpdateForButton(currentTutorial);
            }

            cameraState.Play("TutorialThree");
            batter.gameObject.SetActive(false);
            if (!targetingSystem.activeSelf) targetingSystem.gameObject.SetActive(true);
            
        }
        else if (currentTutorial == Tutorial.TutorialFour)
        {
            // tutorial text Good job ...
            // crosshair UI not active
            // power bar UI active with animation
            // if power bar stop in red text Not enough power ...
            // if power bar stop in white text Great ...
            if (OnTutorialUpdateForButton != null)
            {
                OnTutorialUpdateForButton(currentTutorial);
            }

            targetingSystem.gameObject.SetActive(false);
            cameraState.Play("TutorialFour");
            powerBarSystem.gameObject.SetActive(true);  

        }
        else if (currentTutorial == Tutorial.TutorialFive)
        {
            // camera fadeaway
            // batsman gameobject active
            // crosshair gameobject active
            // ball UI appear
            // every succesfull throw ball UI change color

            if (!pitcher.IsPitcherThrowing())
            {
                cameraState.Play("TutorialThree");
                batter.gameObject.SetActive(true);
            }
            

            if (tutorialFiveCount == 0)
            {
                batter.DisableInputAction();

                if (OnTutorialFiveStarted != null)
                {
                    OnTutorialFiveStarted();
                }

                Debug.LogWarning("Tutorial five Just started");
            }

            if (tutorialFiveCount >= 3 && !pitcher.IsPitcherThrowing())
            {
                if (OnTutorialFiveComplete != null)
                {
                    OnTutorialFiveComplete();
                }

                SetTutorial(Tutorial.TutorialEnd);
            }
        }
        else if (currentTutorial == Tutorial.TutorialEnd)
        {
            // pick team UI pop up
            // if team pick 
            // exit app
            if (OnTutorialUpdateForButton != null)
            {
                OnTutorialUpdateForButton(currentTutorial);
            }

            batter.gameObject.SetActive(false);
            pitcher.gameObject.SetActive(false);
            targetingSystem.SetActive(false);
            powerBarSystem.SetActive(false);

            cameraState.Play("TutorialEnd");
        }
    }

    private void IncrementTutorialOneCount()
    {
        tutorialOneCount++;
    }
    private void IncrementTutorialTwoCount()
    {
        tutorialTwoCount++;
    }


    private IEnumerator LoadingScreenRoutine()
    {
       if (OnTutorialStart != null)
       {
           OnTutorialStart();
       }
       yield return new WaitForSeconds(3f);

       SetTutorial(Tutorial.TutorialOne);
        if (OnLoadingScreenComplete != null)
        {
            OnLoadingScreenComplete();
        }
    }

    private void LockingTarget()
    {
        // if ball throwing mechanic exist :
        // get the vector3 target
        // and use this as position as ball destination
        if (currentTutorial == Tutorial.TutorialThree)
        {
            targetingSystem.gameObject.SetActive(false);
            SetTutorial(Tutorial.TutorialFour);
        }
    }

    private void DisablePowerBar()
    {
        powerBarSystem.gameObject.SetActive(false);
    }

    public void SetTutorial(Tutorial activeTutorial)
    {
        currentTutorial = activeTutorial;
    }

    public Tutorial GetTutorial()
    {
        return currentTutorial;
    }

    public void IncrementTutorialfiveCount()
    {
        tutorialFiveCount++;
    }

}
