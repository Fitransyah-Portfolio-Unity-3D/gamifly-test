using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pitcher : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private InputAction action;

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        action.performed += _ => Throwing();
        action.Enable();
    }

    private void Throwing()
    {
        animator.SetTrigger("Throw");
    }
    public void EnableInputAction()
    {
        action.Enable();
    }
    public void DisableInputAction()
    {
        action.Disable();
    }
}
