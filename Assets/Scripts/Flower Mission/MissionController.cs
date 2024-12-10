using System.Collections.Generic;
using UnityEngine;

public class MissionController : MonoBehaviour
{
    public NotificationManager notificationManager;

    public bool isMissionActive = true;
    private int flowersCollected = 0;
    public int totalFlowers = 6;

    [Header("Flower Spawning Settings")]
    public List<GameObject> flowerPrefabs;
    public List<Transform> spawnPositions;

    private void Start()
    {
        SpawnFlowers();
        StartMission();
    }

    private void Update()
    {

        if (isMissionActive && Input.GetKeyDown(KeyCode.Escape))
        {
            StopMission();
        }
    }

    private void SpawnFlowers()
    {
        int spawnIndex = 0;

        foreach (var flowerPrefab in flowerPrefabs)
        {
            GameObject flower = Instantiate(flowerPrefab);
            flower.transform.position = spawnPositions[spawnIndex].position;
            spawnIndex++;
            ShowNotification("Flor spawneada.");
        }
    }

    public void StartMission()
    {
        isMissionActive = true;
        flowersCollected = 0;
        //ShowNotification("Misión iniciada: Recolecta las flores del Desierto de Atacama!");
    }

    public void StopMission()
    {
        isMissionActive = false;
        ShowNotification($"Misión detenida. Recolectaste {flowersCollected}/{totalFlowers} flores.");
    }

    public void CollectFlower()
    {
        if (!isMissionActive) return;

        flowersCollected++;
        ShowNotification($"Flores recolectadas: {flowersCollected}/{totalFlowers}");

        if (flowersCollected >= totalFlowers)
        {
            CompleteMission();
        }
    }

    private void CompleteMission()
    {
        isMissionActive = false;
        ShowNotification("Misión completada. Todas las flores fueron recolectadas!");
        // Trigger mission completion events or rewards here
    }

    private void ShowNotification(string message)
    {
        notificationManager.ShowNotification(message);
    }
}
