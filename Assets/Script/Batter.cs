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
    [SerializeField] private AudioClip hitAudioClip;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] bool isHitting;

    public event Action OnTutorialOneCount;
    public event Action OnTutorialTwoCount;
    private void Start()
    {
        if (animator == null)
        {
           animator =  GetComponent<Animator>();
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        action.performed += _ => Hitting();
    }

    public void Hitting()
    {
        // currently there is no hit mechanic and ball physics simulation
        if (tutorialManager.GetTutorial() == Tutorial.TutorialTwo && !isHitting) // and ball is hitting (for next development)
        {
            if (OnTutorialTwoCount != null)
            {
                OnTutorialTwoCount();
            }
        }

        if (tutorialManager.GetTutorial() == Tutorial.TutorialOne && !isHitting)
        {
            if (OnTutorialOneCount != null)
            {
                OnTutorialOneCount();
            }
        }

        if (!isHitting)
        {
            animator.SetTrigger("Hit");
            isHitting = true;
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

    public void PlayHitSound()
    {
        audioSource.PlayOneShot(hitAudioClip);
    }
}
