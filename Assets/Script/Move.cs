using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Debug = UnityEngine.Debug;

public class Move : MonoBehaviour
{
    [SerializeField] private Vector3 screenPosition;
    [SerializeField] private Vector3 worldPosition;
    private Vector3 targetLocked;
    [SerializeField] private LayerMask layerToHit;
    [SerializeField] private InputAction action;

    [SerializeField] bool onTarget = false;

    public event Action OnTargetLock;

    private void Start()
    {
        action.performed += _ => SubmitTarget();
    }

    private void Update()
    {
        TargetingMechanic();
    }

    private void TargetingMechanic()
    {
        //screenPosition = Mouse.current.position.ReadValue();
        screenPosition = Touchscreen.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hitData, 100, 1 << 6))
        {
            worldPosition = hitData.point;
        }
        transform.position = new Vector3(
            worldPosition.x,
            worldPosition.y + 0.5f,
            worldPosition.z
            );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Target")
        {
            Debug.LogWarning("Inside target area");
            onTarget = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Target")
        {
            Debug.LogWarning("Outside target area");
            onTarget= false;
        }
    }

    public Vector3 GetTargetLocked()
    {
        return targetLocked;
    }

    public bool GetTargetStatus()
    {
        return onTarget;
    }

    public void SubmitTarget()
    {
        if (onTarget)
        {
            // player press the button to lock
            if (OnTargetLock != null)
            {
                OnTargetLock();
                targetLocked = transform.position;
            }
        }
    }
}
