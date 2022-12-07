using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pitcher : MonoBehaviour
{
    [SerializeField] TutorialManager tutorialManager;
    [SerializeField] private Animator animator;
    [SerializeField] private InputAction action;

    [SerializeField] bool isThrowing;

    public event Action OnTutorialTwoCount;

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        action.performed += _ => Throwing();
        action.Enable();

        tutorialManager.OnTutorialTwoProgress += Throwing;
    }

    private void Throwing()
    {
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
}
