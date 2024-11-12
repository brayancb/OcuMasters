using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections;
using UnityEngine;

public class RotateAndJump : MonoBehaviour
{
    // Velocidad de rotación
    public float rotationSpeed = 30f;
    
    // Aumentamos el multiplicador para hacer el salto más fuerte
    public float jumpMultiplier = 50f;

    // Array para almacenar los datos del espectro de la música
    private float[] spectrumData = new float[256];

    // Referencia al AudioSource llamado "Loca"
    private AudioSource Loca;

    void Start()
    {
        // Encuentra el AudioSource con el nombre "Loca"
        Loca = GameObject.Find("Loca").GetComponent<AudioSource>();
    }

    void Update()
    {
        // Rotar el objeto alrededor del eje Z
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // Verifica si el AudioSource está asignado
        if (Loca != null)
        {
            // Obtener los datos del espectro de la música
            Loca.GetSpectrumData(spectrumData, 0, FFTWindow.Rectangular);

            // Utiliza varias bandas de baja frecuencia para hacer el rebote más fuerte
            float lowFreqAverage = (spectrumData[0] + spectrumData[1] + spectrumData[2]) / 3;
            float jumpValue = lowFreqAverage * jumpMultiplier;

            // Actualiza la posición del objeto en el eje Y según el espectro de la música
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, jumpValue, Time.deltaTime * 5f), transform.position.z);
        }
        else
        {
            Debug.LogWarning("No se encontró el AudioSource llamado 'Loca'. Asegúrate de que el nombre del objeto es correcto.");
        }
    }
}
