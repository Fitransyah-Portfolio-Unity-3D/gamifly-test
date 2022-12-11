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

    bool isTutorialFiveStart;

    public event Action<PitcherState> OnTutorialUpdateFromTutorialFive;

    private void Start()
    {
        crosshair.OnTargetLock += LockPitcherTarget;
        tutorialManager.OnTutorialFiveStarted += StartTutorialFiveRoutine;
    }

    private void OnDestroy()
    {
        crosshair.OnTargetLock -= LockPitcherTarget;
        tutorialManager.OnTutorialFiveStarted -= StartTutorialFiveRoutine;
    }

    private void Update()
    {
        if (!isTutorialFiveStart) return;


        if (pitcher.GetCurrentstate() == Pitcher.PitcherState.Targeting)
        {
            if (!targetingSystem.activeSelf) targetingSystem.SetActive(true);
            powerBarSystem.SetActive(false);

            if (OnTutorialUpdateFromTutorialFive != null)
            {
                OnTutorialUpdateFromTutorialFive(pitcher.GetCurrentstate());
            }
        }
        else if(pitcher.GetCurrentstate() == Pitcher.PitcherState.Preparing)
        {
            targetingSystem.SetActive(false);
            if ( !powerBarSystem.activeSelf) powerBarSystem.SetActive(true);

            if (OnTutorialUpdateFromTutorialFive != null)
            {
                OnTutorialUpdateFromTutorialFive(pitcher.GetCurrentstate());
            }
        }
        else if(pitcher.GetCurrentstate()== Pitcher.PitcherState.Throwing)
        {
            targetingSystem.SetActive(false);
            powerBarSystem.SetActive(false);

            if (OnTutorialUpdateFromTutorialFive != null)
            {
                OnTutorialUpdateFromTutorialFive(pitcher.GetCurrentstate());
            }

            pitcher.SetPitcherState(PitcherState.Targeting);
        }
    }

    private void LockPitcherTarget()
    {
        if (!isTutorialFiveStart) return;
        pitcher.SetPitcherState(Pitcher.PitcherState.Preparing);
        powerBar.RunPowerBar();
        tutorialManager.IncrementTutorialfiveCount();
    }

    private void StartTutorialFiveRoutine()
    {
        isTutorialFiveStart = true;
    }
}
