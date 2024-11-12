using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private float timeElapsed = 0f;
    private bool firstAlertShown = false;
    private bool secondAlertShown = false;
    private bool finalMessageShown = false;

    public float timerDurationMinutes = 1f;
    public float firstAlertAt = 0.7f;
    public float secondAlertAt = 0.9f;
    private float timerDurationSeconds;
    public TextMeshProUGUI alertText;

    void Start()
    {
     
        timerDurationSeconds = timerDurationMinutes * 60f;
        if (alertText != null)
        {
            alertText.text = "";
        }
    }

    void Update()
    {
        if (finalMessageShown) return;

        timeElapsed += Time.deltaTime;

        if (!firstAlertShown && Mathf.RoundToInt(timeElapsed) == Mathf.RoundToInt(timerDurationSeconds * firstAlertAt))
        {
            ShowMessage((firstAlertAt * 100).ToString() + "%");
            firstAlertShown = true;
        }

        if (!secondAlertShown && Mathf.RoundToInt(timeElapsed) == Mathf.RoundToInt(timerDurationSeconds * secondAlertAt))
        {
            ShowMessage((secondAlertAt * 100).ToString() + "%");
            secondAlertShown = true;
        }

        if (timeElapsed >= timerDurationSeconds)
        {
            ShowMessage("Se acabó el tiempo.");
            finalMessageShown = true;
        }
    }

    private void ShowMessage(string message)
    {
        string formattedTime = FormatTime(timeElapsed);
        Debug.Log($"{message} - Transcurrido: {formattedTime}");
        if (alertText != null)
        {
            alertText.text = $"{message}\nTranscurrido: {formattedTime}";
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
}