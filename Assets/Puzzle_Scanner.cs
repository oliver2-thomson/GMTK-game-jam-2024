using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Scanner : MonoBehaviour
{
    [SerializeField] private TriggerEvents2DToggle[] patterns;
    [Space]
    [SerializeField] private UnityEngine.Events.UnityEvent OnComplete;
    [SerializeField] private UnityEngine.Events.UnityEvent OnCompleteOneShot;

    private bool oneshotComplete = false;
    private void Awake()
    {
        foreach(TriggerEvents2D trigger in patterns) 
        {
            trigger.OnTriggerEnter += OnTriggerEnter2D;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ValidateAllCollisions()) 
        {
            if (!oneshotComplete) 
            {
                oneshotComplete = true;
                OnCompleteOneShot?.Invoke();
            }

            OnComplete?.Invoke();
        }
    }

    private bool ValidateAllCollisions() 
    {
        bool Success = true;
        foreach(TriggerEvents2DToggle trigger in patterns) 
        {
            if (!trigger.Toggled) 
            {
                Success = false;
            }
        }

        return Success;
    }

}
