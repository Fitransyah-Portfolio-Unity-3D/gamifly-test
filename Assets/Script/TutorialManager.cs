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
    public event Action<Tutorial> OnTutorialUpdateFromTutorialManager;
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

        if (OnTutorialUpdateFromTutorialManager != null)
        {
            OnTutorialUpdateFromTutorialManager(currentTutorial);
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
            if (OnTutorialUpdateFromTutorialManager != null)
            {
                OnTutorialUpdateFromTutorialManager(currentTutorial);
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
            if (OnTutorialUpdateFromTutorialManager != null)
            {
                OnTutorialUpdateFromTutorialManager(currentTutorial);
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

            if (isTutorialTwoOn && !pitcher.IsPitcherThrowing() && throwingTime <= 0f)
            {
                if (OnTutorialTwoProgress != null)
                {
                    OnTutorialTwoProgress();
                }

                throwingTime = 7f;
            }

            throwingTime -= Time.deltaTime;
            pitcher.gameObject.SetActive(true);
            pitcher.DisableInputAction();
            cameraState.Play("TutorialTwo");
        }
        else if (currentTutorial == Tutorial.TutorialThree)
        {
            if (OnTutorialUpdateFromTutorialManager != null)
            {
                OnTutorialUpdateFromTutorialManager(currentTutorial);
            }

            cameraState.Play("TutorialThree");
            batter.gameObject.SetActive(false);
            if (!targetingSystem.activeSelf) targetingSystem.gameObject.SetActive(true);
            
        }
        else if (currentTutorial == Tutorial.TutorialFour)
        {
            if (OnTutorialUpdateFromTutorialManager != null)
            {
                OnTutorialUpdateFromTutorialManager(currentTutorial);
            }

            targetingSystem.gameObject.SetActive(false);
            cameraState.Play("TutorialFour");
            powerBarSystem.gameObject.SetActive(true);  

            if (currentTutorial == Tutorial.TutorialFour && 
                powerBar.IsPowerBarStop() == true && 
                pitcher.IsPitcherThrowing() == false)
            {
                currentTutorial = Tutorial.TutorialFive;
            }

        }
        else if (currentTutorial == Tutorial.TutorialFive)
        {
            SetTutorialFiveCameraSetting(pitcher.GetCurrentstate());

            if (OnTutorialUpdateFromTutorialManager != null)
            {
                OnTutorialUpdateFromTutorialManager(currentTutorial);
            }

            if (pitcher.GetCurrentstate() == Pitcher.PitcherState.None)
            {
                pitcher.SetPitcherState(Pitcher.PitcherState.Targeting);
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

            if (tutorialFiveCount >= 3 && powerBar.IsPowerBarStop() == true && pitcher.IsPitcherThrowing() == false)
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
            if (OnTutorialUpdateFromTutorialManager != null)
            {
                OnTutorialUpdateFromTutorialManager(currentTutorial);
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

    private void SetTutorialFiveCameraSetting(Pitcher.PitcherState pitcherState)
    {
        switch (pitcherState)
        {
            case Pitcher.PitcherState.None:
                cameraState.Play("TutorialFive");
                break;
            case Pitcher.PitcherState.Targeting:
                cameraState.Play("TutorialThree");
                break;
            case Pitcher.PitcherState.Preparing:
                cameraState.Play("TutorialFour");
                break;
            case Pitcher.PitcherState.Throwing:
                cameraState.Play("TutorialFive");
                break;
        }
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
