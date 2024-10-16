using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaNocheCiclo : MonoBehaviour
{
    [Range(0.0f, 24f)] public float Hora = 12;
    public Transform Sol;
    public float DuracionDelDiaEnMinutos = 1;
    private float Solx;

    private void Update()
    {
        Hora += Time.deltaTime * (24/(60*DuracionDelDiaEnMinutos));
    
        if(Hora >= 24 )
        {
            Hora = 0;
        }
        RotacionSol();
    }

    void RotacionSol()
    {
        Solx = 15 * Hora;
        Sol.localEulerAngles = new Vector3(Solx, 0, 0);
        
        
        
        
        
        
        
        
    }

}
