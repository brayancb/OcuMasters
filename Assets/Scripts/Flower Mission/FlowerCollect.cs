using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FlowerCollect : MonoBehaviour
{
    public MissionController missionController;

    private void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable == null) return;

        grabInteractable.selectEntered.AddListener(OnFlowerGrabbed);
    }

    private void OnFlowerGrabbed(SelectEnterEventArgs args)
    {
        if (missionController == null) return;

        missionController.CollectFlower();
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
