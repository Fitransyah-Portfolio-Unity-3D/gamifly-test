using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Batter : MonoBehaviour
{
    [SerializeField] TutorialManager tutorialManager;
    [SerializeField] private Animator animator;
    [SerializeField] private InputAction action;

    [SerializeField] bool isHitting;

    public event Action OnTutorialOneCount;
    public event Action OnTutorialTwoCount;
    private void Start()
    {
        if (animator == null)
        {
           animator =  GetComponent<Animator>();
        }

        action.performed += _ => Hitting();
    }

    private void Hitting()
    {
        if (!isHitting)
        {
            animator.SetTrigger("Hit");
            isHitting = true;
        }

        if (tutorialManager.GetTutorial() == Tutorial.TutorialOne)
        {
            if (OnTutorialOneCount != null)
            {
                OnTutorialOneCount();
            }   
        }

        // currently there is no hit mechanic and ball physics simulation
        if (tutorialManager.GetTutorial() == Tutorial.TutorialTwo) // and ball is hitting (for next development)
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

    public void SetNotHitting()
    {
        isHitting = false;
    }
    public bool IsBatterHitting()
    {
        return isHitting;
    }
}
