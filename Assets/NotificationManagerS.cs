using TMPro;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    public Transform cameraTransform;
    public Canvas notificationCanvas;
    public TextMeshProUGUI notificationText;
    public float distanceFromCamera = 2f;

    private void Start()
    {
        if (notificationCanvas == null) return;

        notificationCanvas.enabled = false;
    }

    private void Update()
    {
        if (notificationCanvas.renderMode != RenderMode.WorldSpace) return;
        
        notificationCanvas.transform.position = cameraTransform.position + cameraTransform.forward * distanceFromCamera;
        notificationCanvas.transform.LookAt(cameraTransform);
        notificationCanvas.transform.Rotate(0, 180f, 0);
    }

    public void ShowNotification(string message, float duration = 5f)
    {
        if (notificationText == null) return;

        notificationText.text = message;
        notificationCanvas.enabled = true;
        Invoke(nameof(HideNotification), duration);
    }

    private void HideNotification()
    {
        if (notificationCanvas != null)
        {
            notificationCanvas.enabled = false;
        }
    }
}
