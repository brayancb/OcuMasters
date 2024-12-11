using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionController : MonoBehaviour
{
    public NotificationManager notificationManager;
    public TimeTracker timeTracker;
    public float secondsBeforeShine = 180f;
    public float shineDuration = 10f;
    public Material shineMaterial;


    [Header("Flower Spawning Settings")]
    public List<GameObject> flowerPrefabs;
    public List<Transform> spawnPositions;

    private List<GameObject> spawnedFlowers = new();
    private bool isMissionActive = false;
    private int totalFlowers;
    private int flowersCollected = 0;

    private void Start()
    {
        this.totalFlowers = flowerPrefabs.Count * 2;
        StartMission();
    }

    private void SpawnFlowers()
    {
        int spawnIndex = 0;

        foreach (var flowerPrefab in flowerPrefabs)
        {
            for(int i = 0; i<2; ++i)
            {
                GameObject flower = Instantiate(flowerPrefab);
                flower.transform.position = spawnPositions[spawnIndex++].position;
                FlowerCollect flowerCollect = flower.GetComponent<FlowerCollect>();
                flowerCollect.missionController = this;
                spawnedFlowers.Add(flower);
            }
        }
    }

    public void StartMission()
    {
        if (isMissionActive) return;

        SpawnFlowers();
        isMissionActive = true;
        flowersCollected = 0;
        notificationManager.ShowNotification("Misión iniciada: Recolecta las flores del Desierto de Atacama!");
        timeTracker.StartTimer(secondsBeforeShine, ShineRemainingFlowers);
    }

    public void StopMission()
    {
        if (!isMissionActive) return;

        isMissionActive = false;
        notificationManager.ShowNotification($"Misión detenida. Recolectaste {flowersCollected}/{totalFlowers} flores.");
        timeTracker.StopTimer();
    }

    public void CollectFlower(GameObject flower)
    {
        if (!isMissionActive) return;

        notificationManager.ShowNotification($"Flores recolectadas: {++flowersCollected}/{totalFlowers}");
        spawnedFlowers.Remove(flower);
        Destroy(flower, 2f);

        if (flowersCollected >= totalFlowers)
        {
            CompleteMission();
        }
    }

    private void CompleteMission()
    {
        isMissionActive = false;
        notificationManager.ShowNotification("Misión completada. Todas las flores fueron recolectadas!");
        timeTracker.StopTimer();
    }

    private void ShineRemainingFlowers()
    {
        if (!isMissionActive || flowersCollected >= totalFlowers) return;

        notificationManager.ShowNotification("¡Las flores restantes están brillando para ayudarte!");

        foreach (var flower in spawnedFlowers)
        {
            if (flower != null)
            {
                StartCoroutine(ApplyShineEffect(flower));
            }
        }
    }

    private IEnumerator ApplyShineEffect(GameObject flower)
    {
        Renderer renderer = flower.GetComponent<Renderer>();
        if (renderer == null || shineMaterial == null) yield break;

        Material originalMaterial = renderer.material;
        renderer.material = shineMaterial;

        yield return new WaitForSeconds(shineDuration);

        renderer.material = originalMaterial;
    }
}
