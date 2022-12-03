using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private Transform launcherTip;
    [SerializeField] private float forwardForce;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject instance =  Instantiate(ball, launcherTip.position, launcherTip.rotation);
            Rigidbody ballRb = instance.GetComponent<Rigidbody>();
            ballRb.AddForce(launcherTip.forward * forwardForce);
        }
    }
}
