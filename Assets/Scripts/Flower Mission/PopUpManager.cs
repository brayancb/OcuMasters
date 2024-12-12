using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour
{
    public static PopUpManager Instance { get; private set; }

    public GameObject popUpPanel; // The pop-up UI panel
    public TextMeshProUGUI flowerNameText;
    public TextMeshProUGUI flowerSizeText;
    public TextMeshProUGUI flowerDescriptionText;
    public Image flowerImage;

    public float distanceFromPlayer = 2.0f; // Distance in front of the player

    public Camera playerCamera;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        popUpPanel.SetActive(false);
    }

    private void Update() 
    { 
        if (popUpPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape)) 
        { 
            ClosePopUp(); 
        } 
    }

    public void ShowFlowerInfo(string name, string size, string description, Sprite image)
    {
        flowerNameText.text = name;
        flowerDescriptionText.text = description;
        flowerImage.sprite = image;
        flowerSizeText.text = size;

        popUpPanel.SetActive(true);

        Invoke(nameof(ClosePopUp), 10f);
    }

    public void ClosePopUp()
    {
        popUpPanel.SetActive(false);
    }

}
