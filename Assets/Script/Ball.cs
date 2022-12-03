using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject, 10f);
    }
}
