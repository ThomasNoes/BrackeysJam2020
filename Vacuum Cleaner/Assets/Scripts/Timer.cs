using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public enum TimerType { countDown, countUp, puase };
public class Timer : MonoBehaviour
{

    [SerializeField] private TimerType timerType;
    [SerializeField] private float startTime;
    [Tooltip("The speed at which the timer counts. 1 = once per second")]
    [SerializeField] private float timerSpeed = 1f;
    [SerializeField] private bool stopOnZero;

    private float _currentTime;
    public event Action TimerReachedZero;
    private bool InvokedZeroEvent = false;

    private void Start()
    {
        InvokedZeroEvent = false;
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
            if (CheckTimerZero() && !InvokedZeroEvent)
            {
                TimerReachedZero?.Invoke();
                InvokedZeroEvent = true;
                return;
            }
        }

        switch (timerType)
        {
            case TimerType.countDown:
                CountDown();
                break;
            case TimerType.countUp:
                CountUp();
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

    public float GetCurrentTime()
    {
        return _currentTime;
    }

    /// <summary>
    /// updates a the text of a text field with the current time of the timer, displayed with decimalCount decimals.
    /// </summary>
    /// <param name="tex"></param>
    /// <param name="decimalCount"></param>
    public void UpdateText(Text tex, int decimalCount)
    {
        tex.text = GetCurrentTime().ToString("F"+decimalCount);
    }
    /// <summary>
    /// updates a the text of a text mesh pro field with the current time of the timer, displayed with decimalCount decimals.
    /// </summary>
    /// <param name="tex"></param>
    /// <param name="decimalCount"></param>
    public void UpdateText(TextMeshProUGUI tex, int decimalCount)
    {
        tex.text = GetCurrentTime().ToString("F"+decimalCount);
    }

    public void PauseTimer()
    {
        timerType = TimerType.puase;
    }

    public void StartTimer(TimerType type)
    {
        timerType = type;
    }
}
