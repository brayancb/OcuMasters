using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FlowerCollect : MonoBehaviour
{
    public MissionController missionController;

    [Header("Flower Information")]
    public string flowerName;
    public string description;
    public string height;
    public Sprite image;

    private void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable == null) return;

        grabInteractable.selectEntered.AddListener(OnFlowerGrabbed);
    }

    private void OnFlowerGrabbed(SelectEnterEventArgs args)
    {
        if (missionController == null) return;

        missionController.CollectFlower(gameObject);
    }

    private void OnDestroy()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnFlowerGrabbed);
        }
    }
}