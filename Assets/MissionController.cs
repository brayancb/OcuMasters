using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionController : MonoBehaviour
{
    public NotificationManager notificationManager;
    public TimeTracker timeTracker;
    public float secondsBeforeShine = 180f;
    public Material shineMaterial;

    [Header("Flower Spawning Settings")]
    public List<GameObject> flowerPrefabs;
    public List<Transform> spawnPositions;

    public Canvas missionStartCanvas; // Asegura que esta variable sea pública o [SerializeField]

    private List<GameObject> spawnedFlowers = new();
    private bool isMissionActive = false;
    private int totalFlowers;
    private int flowersCollected = 0;

    private void Start()
    {
        totalFlowers = flowerPrefabs.Count;
        // No iniciamos la misión aquí
    }

    public void StartMission()
    {
        if (!isMissionActive)
        {
            isMissionActive = true;
            // Mostrar el Canvas de inicio de misión
            if (missionStartCanvas != null)
            {
                missionStartCanvas.gameObject.SetActive(true);
                // Iniciar la corrutina para esperar a que el Canvas se oculte
                StartCoroutine(WaitForCanvasToHide());
            }
            else
            {
                // Si no hay Canvas asignado, proceder directamente
                ProceedWithMission();
            }
        }
    }

    private IEnumerator WaitForCanvasToHide()
    {
        // Esperar hasta que el Canvas esté desactivado
        while (missionStartCanvas.gameObject.activeSelf)
        {
            yield return null; // Espera al siguiente frame
        }
        // Proceder con la misión
        ProceedWithMission();
    }

    private void ProceedWithMission()
    {
        SpawnFlowers();
        // Cualquier lógica adicional para iniciar la misión
    }

    private void SpawnFlowers()
    {
        // Tu lógica para generar las flores
    }

    // Resto del código existente...
}