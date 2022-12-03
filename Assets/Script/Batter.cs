using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Batter : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private InputAction action;

    private void Start()
    {
        if (animator == null)
        {
           animator =  GetComponent<Animator>();
        }

        action.performed += _ => Hitting();
        //action.Enable();
    }

    private void Hitting()
    {
        animator.SetTrigger("Hit");
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
