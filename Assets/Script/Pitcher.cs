using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pitcher : MonoBehaviour
{
    public enum PitcherState
    {
        Targeting,
        Preparing,
        Throwing,
    }
    [SerializeField] TutorialManager tutorialManager;
    [SerializeField] PowerBar powerBar;
    [SerializeField] private Animator animator;
    [SerializeField] private InputAction action;
    [SerializeField] PitcherState pitchercurrentState = PitcherState.Targeting;


    [SerializeField] bool isThrowing;

    public event Action OnTutorialTwoCount;
    public event Action OnTutorialFivecount;


    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        action.performed += _ => Throwing();
        action.Enable();

        tutorialManager.OnTutorialTwoProgress += Throwing;
        powerBar.OnBarStop += Throwing;
    }

    private void Update()
    {

    }

    public void Throwing()
    {
       
        if (tutorialManager.GetTutorial() == Tutorial.TutorialFive && pitchercurrentState == PitcherState.Preparing && !isThrowing)
        {
            pitchercurrentState = PitcherState.Throwing;
            tutorialManager.IncrementTutorialfiveCount();

            if (OnTutorialFivecount != null)
            {
                OnTutorialFivecount(); // for what?
            }
        }


        if (!isThrowing)
        {
            animator.SetTrigger("Throw");
            isThrowing = true;
        }

        if (tutorialManager.GetTutorial() == Tutorial.TutorialTwo)
        {
            if (OnTutorialTwoCount != null)
            {
                OnTutorialTwoCount();
            }
        }


        if (tutorialManager.GetTutorial() == Tutorial.TutorialFour)
        {
            tutorialManager.SetTutorial(Tutorial.TutorialFive);
        }

    }
    public void EnableInputAction()
    {
        action.Enable();
    }
    public void DisableInputAction()
    {
        action.Disable();
    }
    public void SetNotThrowing()
    {
        isThrowing = false;
    }

    public bool IsPitcherThrowing()
    {
        return isThrowing;
    }

    public PitcherState GetCurrentstate()
    {
        return pitchercurrentState;
    }

    public void ResetPitcherState()
    {
        pitchercurrentState = PitcherState.Targeting;
    }

    public void SetPitcherState(PitcherState stateOnGoing)
    {
        pitchercurrentState = stateOnGoing;
    }
}
