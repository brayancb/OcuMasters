using UnityEngine;
using UnityEngine.SpatialTracking;

public class CameraLock : MonoBehaviour
{
    private TrackedPoseDriver trackedPoseDriver;
    private bool isLocked = false;

    private Transform cameraTransform;
    private Vector3 lockedPosition;
    private Quaternion lockedRotation;

    void Start()
    {
        // Get the main camera's transform and TrackedPoseDriver
        cameraTransform = Camera.main.transform;
        trackedPoseDriver = Camera.main.GetComponent<TrackedPoseDriver>();

        if (trackedPoseDriver == null)
        {
            Debug.LogError("TrackedPoseDriver not found on the main camera. Please check your XR setup.");
        }
    }

    void LateUpdate()
    {
        if (isLocked)
        {
            // Keep the camera in the locked position and rotation
            cameraTransform.position = lockedPosition;
            cameraTransform.rotation = lockedRotation;
        }
    }

    public void LockCamera()
    {
        if (trackedPoseDriver != null)
        {
            trackedPoseDriver.enabled = false; // Disable XR tracking
        }

        // Save the current camera position and rotation
        lockedPosition = cameraTransform.position;
        lockedRotation = cameraTransform.rotation;

        isLocked = true;
    }

    public void UnlockCamera()
    {
        if (trackedPoseDriver != null)
        {
            trackedPoseDriver.enabled = true; // Re-enable XR tracking
        }

        isLocked = false;
    }
}
