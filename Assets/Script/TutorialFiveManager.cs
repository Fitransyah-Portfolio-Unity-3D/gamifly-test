using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Pitcher;

public class TutorialFiveManager : MonoBehaviour
{
    [SerializeField] TutorialManager tutorialManager;
    [SerializeField] Pitcher pitcher;
    [SerializeField] GameObject targetingSystem;
    [SerializeField] GameObject powerBarSystem;
    [SerializeField] Move crosshair;
    [SerializeField] PowerBar powerBar;


    public event Action<PitcherState> OnTutorialUpdateForButton;

    private void Start()
    {
        crosshair.OnTargetLock += LockPitcherTarget;
    }

    private void OnDestroy()
    {
        crosshair.OnTargetLock -= LockPitcherTarget;
    }

    private void Update()
    {
        if (tutorialManager.GetTutorial() != Tutorial.TutorialFive || tutorialManager.GetTutorial() == Tutorial.TutorialFour) return;


        if (pitcher.GetCurrentstate() == Pitcher.PitcherState.Targeting)
        {
            if (!targetingSystem.activeSelf) targetingSystem.SetActive(true);
            powerBarSystem.SetActive(false);

            if (OnTutorialUpdateForButton != null)
            {
                OnTutorialUpdateForButton(pitcher.GetCurrentstate());
            }
        }
        else if(pitcher.GetCurrentstate() == Pitcher.PitcherState.Preparing)
        {
            targetingSystem.SetActive(false);
            if ( !powerBarSystem.activeSelf) powerBarSystem.SetActive(true);

            if (OnTutorialUpdateForButton != null)
            {
                OnTutorialUpdateForButton(pitcher.GetCurrentstate());
            }
        }
        else if(pitcher.GetCurrentstate()== Pitcher.PitcherState.Throwing)
        {
            targetingSystem.SetActive(false);
            powerBarSystem.SetActive(false);

            if (OnTutorialUpdateForButton != null)
            {
                OnTutorialUpdateForButton(pitcher.GetCurrentstate());
            }
        }
    }

    private void LockPitcherTarget()
    {
        pitcher.SetPitcherState(Pitcher.PitcherState.Preparing);
    }
}
