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
        None
    }
    [SerializeField] TutorialManager tutorialManager;
    [SerializeField] PowerBar powerBar;
    [SerializeField] private Animator animator;
    [SerializeField] private InputAction action;
    [SerializeField] PitcherState pitchercurrentState;


    [SerializeField] bool isThrowing;

    public event Action OnTutorialTwoCount;
    public event Action OnTutorialFivecount;

    private void Awake()
    {
        pitchercurrentState = PitcherState.None;
    }
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

    public void Throwing()
    {
       
        if (!isThrowing)
        {
            animator.SetTrigger("Throw");
            isThrowing = true;

            if (OnTutorialFivecount != null && tutorialManager.GetTutorial() == Tutorial.TutorialFive)
            {
                OnTutorialFivecount(); // for what?
            }

            //if (tutorialManager.GetTutorial() == Tutorial.TutorialFive && pitchercurrentState == PitcherState.Preparing && !isThrowing)
            //{
            //    pitchercurrentState = PitcherState.Throwing;
            //    tutorialManager.IncrementTutorialfiveCount();
            //}

            if (tutorialManager.GetTutorial() == Tutorial.TutorialTwo)
            {
                if (OnTutorialTwoCount != null)
                {
                    OnTutorialTwoCount();
                }
            }

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
        pitchercurrentState = PitcherState.None;
    }

    public void SetPitcherState(PitcherState stateOnGoing)
    {
        pitchercurrentState = stateOnGoing;
    }
}
