using System;
using System.Collections;
using UnityEngine;

public class TimeTracker : MonoBehaviour
{
    private Coroutine timerCoroutine;

    public void StartTimer(float duration, Action onTimeElapsed)
    {
        StopTimer(); // Ensure only one timer runs at a time.
        timerCoroutine = StartCoroutine(TimerCoroutine(duration, onTimeElapsed));
    }

    public void StopTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }

    private IEnumerator TimerCoroutine(float duration, Action onTimeElapsed)
    {
        yield return new WaitForSeconds(duration);
        onTimeElapsed?.Invoke();
    }
}
