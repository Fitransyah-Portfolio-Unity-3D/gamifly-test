using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBar : MonoBehaviour
{
    [SerializeField] Batter batter;
    [SerializeField] TutorialManager tutorialManager;
    [SerializeField] RectTransform topLimit;
    [SerializeField] RectTransform downLimit;
    [SerializeField] RectTransform pointer;

    float bottomLocalY;
    float topLocalY;
    float destination;

    bool barStop = false;

    public event Action OnBarStop;

    private void Awake()
    {
        RunPowerBar();

        if (batter == null)
        {
            GameObject.FindGameObjectWithTag("Batter").GetComponent<Batter>();
        }
    }

    private void Start()
    {
        topLocalY = topLimit.anchoredPosition.y;
        bottomLocalY = downLimit.anchoredPosition.y;
        destination = topLocalY;
        barStop = false;
    }

    private void Update()
    {
        if (barStop == true) return;
        
        pointer.localPosition += new Vector3 (0, destination, 0) * Time.deltaTime ;
        
       if (pointer.anchoredPosition.y > topLocalY)
        {
            destination = bottomLocalY;
        }
       else if (pointer.anchoredPosition.y < bottomLocalY)
        {
            destination = topLocalY;
        }
    }

    public void StopPowerBar()
    {
        barStop = true;

        if (tutorialManager.GetTutorial() == Tutorial.TutorialFive)
        {
            batter.HitAction();
        }

        if (OnBarStop != null)
        {
            OnBarStop();
        }

       
    }

    public void RunPowerBar()
    {
        barStop = false;
    }

    public bool IsPowerBarStop()
    {
        return barStop;
    }

}
