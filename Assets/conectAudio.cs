using UnityEngine;
using System.Collections;

public class AudioSequencePlayer : MonoBehaviour
{
    public AudioSource firstAudioSource;  // Asigna el primer AudioSource en el Inspector
    public AudioSource secondAudioSource; // Asigna el segundo AudioSource en el Inspector
    public float delayBetweenAudios = 30f; // Tiempo de espera entre los audios en segundos

    public void StartAudios()
    {
        if (firstAudioSource != null)
        {
            firstAudioSource.Play();
            StartCoroutine(WaitForFirstAudioToEnd());
        }
        else
        {
            Debug.LogWarning("No se ha asignado el primer Audiosource.");
        }
    }

    private IEnumerator WaitForFirstAudioToEnd()
    {
        // Esperar hasta que el primer audio termine de reproducirse
        while (firstAudioSource.isPlaying)
        {
            yield return null;
        }

        // Esperar el tiempo especificado
        yield return new WaitForSeconds(delayBetweenAudios);

        // Reproducir el segundo audio
        if (secondAudioSource != null)
        {
            secondAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("No se ha asignado el segundo AudioSource.");
        }
    }
}