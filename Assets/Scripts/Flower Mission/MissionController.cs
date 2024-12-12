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

    private List<GameObject> spawnedFlowers = new();
    private bool isMissionActive = false;
    private int totalFlowers;
    private int flowersCollected = 0;

    private void Start()
    {
        this.totalFlowers = flowerPrefabs.Count;
        StartMission();
    }

    private void SpawnFlowers()
    {
        int spawnIndex = 0;

        foreach (var flowerPrefab in flowerPrefabs)
        {
            GameObject flower = Instantiate(flowerPrefab);
            flower.transform.position = spawnPositions[spawnIndex++].position;
            FlowerCollect flowerCollect = flower.GetComponent<FlowerCollect>();
            flowerCollect.missionController = this;
            spawnedFlowers.Add(flower);
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

        FlowerCollect flowerData = flower.GetComponent<FlowerCollect>();
        if (flowerData != null)
        {
            PopUpManager.Instance.ShowFlowerInfo(
                flowerData.flowerName,
                flowerData.height,
                flowerData.description,
                flowerData.image
            );
        }

        Renderer renderer = flower.GetComponent<Renderer>();
        if (renderer != null)
        {
            var materialHolder = flower.GetComponent<OriginalMaterialHolder>();
            if (materialHolder != null && materialHolder.originalMaterial != null)
            {
                renderer.material = materialHolder.originalMaterial;
            }
        }

        AudioSource audioSource = flower.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.enabled = false;
        }

        spawnedFlowers.Remove(flower);
        ++flowersCollected;
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
                ApplyShineEffect(flower);
            }
        }
    }

    private void ApplyShineEffect(GameObject flower)
    {
        Renderer renderer = flower.GetComponent<Renderer>();
        if (renderer == null || shineMaterial == null) return;

        Material originalMaterial = renderer.material;

        var materialHolder = flower.GetComponent<OriginalMaterialHolder>() ?? flower.AddComponent<OriginalMaterialHolder>();
        materialHolder.originalMaterial = originalMaterial;

        Material tempShineMaterial = new Material(shineMaterial);

        if (tempShineMaterial.HasProperty("_EmissionColor"))
        {
            Color emissionColor = tempShineMaterial.GetColor("_EmissionColor");
            tempShineMaterial.SetColor("_EmissionColor", emissionColor * 0.5f);
        }

        renderer.material = tempShineMaterial;

        AudioSource audioSource = flower.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.enabled = true;
        }
    }
}

public class OriginalMaterialHolder : MonoBehaviour
{
    public Material originalMaterial;
}
