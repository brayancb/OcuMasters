using System.Collections.Generic;
using UnityEngine;

public class MissionController : MonoBehaviour
{
    public NotificationManager notificationManager;
    public int totalFlowers = 6;

    [Header("Flower Spawning Settings")]
    public List<GameObject> flowerPrefabs;
    public List<Transform> spawnPositions;

    private bool isMissionActive = false;
    private int flowersCollected = 0;

    private void Start()
    {
        StartMission();
    }

    private void SpawnFlowers()
    {
        int spawnIndex = 0;

        foreach (var flowerPrefab in flowerPrefabs)
        {
            for(int i = 0; i<2; ++i)
            {
                //if (spawnPositions.Count < spawnIndex + 1) return;

                GameObject flower = Instantiate(flowerPrefab);
                flower.transform.position = spawnPositions[spawnIndex++].position;

                FlowerCollect flowerCollect = flower.GetComponent<FlowerCollect>();

                flowerCollect.missionController = this;
            }
        }
    }

    public void StartMission()
    {
        if (isMissionActive) return;

        SpawnFlowers();
        isMissionActive = true;
        flowersCollected = 0;
        ShowNotification("Misión iniciada: Recolecta las flores del Desierto de Atacama!");
    }

    public void StopMission()
    {
        if (!isMissionActive) return;

        isMissionActive = false;
        ShowNotification($"Misión detenida. Recolectaste {flowersCollected}/{totalFlowers} flores.");
    }

    public void CollectFlower(GameObject flower)
    {
        if (!isMissionActive) return;

        ShowNotification($"Flores recolectadas: {++flowersCollected}/{totalFlowers}");
        Destroy(flower, 1f);

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

    public void ShowNotification(string message)
    {
        notificationManager.ShowNotification(message);
    }
}
