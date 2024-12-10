using UnityEngine;
using System.Collections;  // Necesario para IEnumerator

public class DesertSoundLoop : MonoBehaviour
{
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public float fadeDuration = 3.0f;  // Duración del crossfade
    private bool isFading = false;

    void Start()
    {
        audioSource1.Play();
        StartCoroutine(CheckAudioEnd(audioSource1));
    }

    IEnumerator CheckAudioEnd(AudioSource currentAudio)
    {
        while (true)
        {
            // Verifica si quedan menos de "fadeDuration" segundos para que termine el audio
            if (currentAudio.isPlaying && currentAudio.time >= currentAudio.clip.length - fadeDuration && !isFading)
            {
                StartCoroutine(FadeAudio(currentAudio, (currentAudio == audioSource1) ? audioSource2 : audioSource1));
            }
            yield return new WaitForSeconds(0.5f);  // Chequeo cada medio segundo
        }
    }

    IEnumerator FadeAudio(AudioSource from, AudioSource to)
    {
        isFading = true;
        to.Play();
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            from.volume = Mathf.Lerp(1, 0, time / fadeDuration);  // Fade Out
            to.volume = Mathf.Lerp(0, 1, time / fadeDuration);    // Fade In
            yield return null;
        }

        isFading = false;
        StartCoroutine(CheckAudioEnd(to));  // Cambia la verificación al nuevo audio
        
    }
}
