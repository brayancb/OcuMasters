using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AgarrarAudio : MonoBehaviour
{
    public AudioClip audioClip;
    private AudioSource audioSource;
    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        // Obtener componentes necesarios
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = audioClip;
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Suscribirse a eventos
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (audioSource != null && audioClip != null)
        {
            // Reproducir audio
            audioSource.Play();
        }
    }
}