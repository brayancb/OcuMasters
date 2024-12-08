using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlpacaSound : MonoBehaviour
{
    public AudioClip[] soundClips; // Array para los sonidos
    private AudioSource audioSource; // El AudioSource de la alpaca
    public float minTimeBetweenSounds = 1f; // Tiempo mínimo entre sonidos
    public float maxTimeBetweenSounds = 5f; // Tiempo máximo entre sonidos

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Obtiene el AudioSource del objeto
        InvokeRepeating("PlayRandomSound", 0f, Random.Range(minTimeBetweenSounds, maxTimeBetweenSounds)); // Llama a PlayRandomSound aleatoriamente
    }

    void PlayRandomSound()
    {
        if (soundClips.Length > 0) // Asegúrate de que haya sonidos en el array
        {
            // Selecciona un sonido aleatorio
            int randomIndex = Random.Range(0, soundClips.Length);
            audioSource.clip = soundClips[randomIndex]; // Asigna el sonido aleatorio al AudioSource
            audioSource.Play(); // Reproduce el sonido
        }
    }
}
