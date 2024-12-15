using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissionController : MonoBehaviour
{
    public NotificationManager notificationManager;
    public SubtitleManager subtitleManager;
    public TimeTracker timeTracker;
    public DiaNocheCiclo dayCicle;
    public TextMeshProUGUI flowersCollectedText;
    public float secondsBeforeShine = 180f;
    public Material shineMaterial;
    public float startMeditationAfter = 15f;

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
        this.flowersCollectedText.enabled = false;
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
        flowersCollectedText.enabled = true;
        flowersCollected = 0;
        timeTracker.StartTimer(secondsBeforeShine, ShineRemainingFlowers);
        notificationManager.ShowNotification("Misión iniciada: Recolecta las flores del Desierto de Atacama!");
        flowersCollectedText.text = $"Flores: {flowersCollected}/{totalFlowers}";
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
        flowersCollectedText.text = $"Flores: {flowersCollected}/{totalFlowers}";
        Destroy(flower, 2f);

        if (flowersCollected == totalFlowers)
        {
            CompleteMission();
        }
    }

    public void CompleteMission()
    {
        if (!isMissionActive) return;

        isMissionActive = false;
        timeTracker.StopTimer();
        notificationManager.ShowNotification("Misión completada. Todas las flores fueron recolectadas!");
        flowersCollectedText.enabled = false;

        if (subtitleManager == null) return;

        dayCicle.SetNight();
        Invoke(nameof(StartMeditation), startMeditationAfter);
    }

    public void StopMission()
    {
        if (!isMissionActive) return;

        isMissionActive = false;
        timeTracker.StopTimer();
        notificationManager.ShowNotification($"Misión detenida. Recolectaste {flowersCollected}/{totalFlowers} flores.");
        flowersCollectedText.enabled = false;

        if (subtitleManager == null) return;

        dayCicle.SetNight();
        Invoke(nameof(StartMeditation), startMeditationAfter);
    }

    public bool IsMissionActive()
    {
        return isMissionActive;
    }

    private void StartMeditation()
    {
        if (subtitleManager == null) return;

        subtitleManager.StartPlayback();
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