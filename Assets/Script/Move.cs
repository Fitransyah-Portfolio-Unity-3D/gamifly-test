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
    [SerializeField] private LayerMask layerToHit;
    private void Update()
    {
        screenPosition = Touchscreen.current.position.ReadValue();
        //screenPosition = Mouse.current.position.ReadValue();

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hitData, 100, 1<<6))
        {
            worldPosition = hitData.point;
        }
        transform.position = new Vector3(
            worldPosition.x ,
            worldPosition.y + 0.5f,
            worldPosition.z 
            );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Target")
        {
            Debug.LogWarning("Colliding");
            Application.Quit();
        }
    }
}
