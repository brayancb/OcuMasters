using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaNocheCiclo : MonoBehaviour
{
    [Range(0.0f, 24f)] public float Hora = 12;
    public Transform Sol;
    public float DuracionDelDiaEnMinutos = 1;
    public Material skyboxDia;
    public Material skyboxNoche;
    public Material skyboxAmanecer;
    public Material skyboxAtardecer;
    public Material skyboxCrepusculo; // Nuevo skybox para el crepúsculo
    public ParticleSystem estrellas;
    private float Solx;
    private Light solLuz;

    private void Start()
    {
        // Asegúrate de que el componente Light esté asignado
        solLuz = Sol.GetComponent<Light>();
    }

    private void Update()
    {
        Hora += Time.deltaTime * (24 / (60 * DuracionDelDiaEnMinutos));
    
        if (Hora >= 24)
        {
            Hora = 0;
        }
        
        RotacionSol();
        CambiarSkybox();
        ActivarParticulas();
    }

    void RotacionSol()
    {
        Solx = 15 * Hora;
        Sol.localEulerAngles = new Vector3(Solx, 0, 0);
        
        if (Hora < 6 || Hora > 20) // Noche
        {
            solLuz.intensity = 0;
        }
        else if (Hora < 8) // Amanecer (6 AM a 8 AM)
        {
            solLuz.intensity = Mathf.Lerp(0, 1, (Hora - 6) / 2);
        }
        else if (Hora > 18) // Atardecer (6 PM a 8 PM)
        {
            solLuz.intensity = Mathf.Lerp(1, 0, (Hora - 18) / 2);
        }
        else // Día
        {
            solLuz.intensity = 1;
        }
    }

    void CambiarSkybox()
    {
        if (Hora < 6 || Hora >= 21) // Noche
        {
            RenderSettings.skybox = skyboxNoche;
        }
        else if (Hora >= 6 && Hora < 8) // Amanecer
        {
            RenderSettings.skybox = skyboxAmanecer;
        }
        else if (Hora >= 18 && Hora < 20) // Atardecer
        {
            RenderSettings.skybox = skyboxAtardecer;
        }
        else if (Hora >= 20 && Hora < 21) // Crepúsculo
        {
            RenderSettings.skybox = skyboxCrepusculo;
        }
        else // Día
        {
            RenderSettings.skybox = skyboxDia;
        }
    }

    void ActivarParticulas()
    {
        // Activar partículas solo en la noche
        if (Hora < 5 || Hora > 22)
        {
            if (!estrellas.isPlaying)
                estrellas.Play();
        }
        else
        {
            if (estrellas.isPlaying)
                estrellas.Stop();
        }
    }
}
