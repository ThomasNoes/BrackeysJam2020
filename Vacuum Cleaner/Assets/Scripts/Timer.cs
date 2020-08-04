using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    enum TimerType { countDown, countUp};

    [SerializeField] private TimerType timerType;
    [SerializeField] private float startTime;
    [Tooltip("The speed at which the timer counts. 1 = once per second")]
    [SerializeField] private float timerSpeed = 1f;
    [SerializeField] private bool stopOnZero;

    private float _currentTime;
    public delegate void TimerIsZero();
    public event TimerIsZero OnTimerIsZero;

    private void Start()
    {
        _currentTime = startTime;
        switch (timerType)
        {
            case TimerType.countDown:
                stopOnZero = true;
                break;
            case TimerType.countUp:
                stopOnZero = false;
                break;
            default:
                stopOnZero = false;
                break;
        }
    }

    private void Update()
    {
        if (stopOnZero)
        {
            if (CheckTimerZero())
            {
                OnTimerIsZero();
                return;
            }
        }

        switch (timerType)
        {
            case TimerType.countDown:

                break;
            case TimerType.countUp:

                break;
            default:
                //pause
                break;
        }
    }

    private void CountUp()
    {
        _currentTime += Time.deltaTime * timerSpeed;
    }

    private void CountDown()
    {
        _currentTime -= Time.deltaTime * timerSpeed;
    }

    private bool CheckTimerZero()
    {
        if(_currentTime <= 0f)
        {
            return true;
        }
        return false;
    }
}
