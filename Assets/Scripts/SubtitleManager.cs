using UnityEngine;
using TMPro; // Importante si usas TextMeshPro
using System.Collections.Generic;

public class SubtitleManager : MonoBehaviour
{
    public AudioSource audioSource; // Arrastra tu AudioSource aquí
    public TextMeshProUGUI subtitleText; // Para mostrar los subtítulos
    public TextAsset subtitleFile; // Arrastra tu archivo .srt aquí

    private List<(float startTime, float endTime, string text)> subtitles = new List<(float, float, string)>();
    private int currentSubtitleIndex = 0;
    private bool isPlaying = false;

    void Start()
    {
        LoadSubtitles();
        Invoke(nameof(StartPlayback), 6f);
    }

    public void StartPlayback()
    {
        if (!isPlaying && audioSource != null && subtitleText != null)
        {
            isPlaying = true;
            audioSource.Play();
            StartCoroutine(ShowSubtitles());
        }
    }

    void LoadSubtitles()
    {
        string[] lines = subtitleFile.text.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("-->"))
            {
                string[] times = lines[i].Split(new string[] { " --> " }, System.StringSplitOptions.None);
                float startTime = ParseTime(times[0]);
                float endTime = ParseTime(times[1]);
                string text = lines[i + 1];
                subtitles.Add((startTime, endTime, text));
            }
        }
    }

    float ParseTime(string time)
    {
        string[] parts = time.Split(':', ',');
        return float.Parse(parts[0]) * 3600 + float.Parse(parts[1]) * 60 + float.Parse(parts[2]) + float.Parse(parts[3]) / 1000;
    }

    System.Collections.IEnumerator ShowSubtitles()
    {
        while (currentSubtitleIndex < subtitles.Count)
        {
            var subtitle = subtitles[currentSubtitleIndex];
            if (audioSource.time >= subtitle.startTime && audioSource.time <= subtitle.endTime)
            {
                subtitleText.text = subtitle.text;
            }
            else if (audioSource.time > subtitle.endTime)
            {
                subtitleText.text = "";
                currentSubtitleIndex++;
            }
            yield return null;
        }
        subtitleText.text = "";
        isPlaying = false;
    }
}