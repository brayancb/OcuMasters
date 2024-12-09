using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class CrouchController : MonoBehaviour
{
    public CharacterController characterController;
    public Transform xrCamera;  // La cámara de la XR Rig
    public float normalHeight = 2.0f; // Altura normal del personaje
    public float crouchHeight = 1.0f; // Altura al agacharse

    private InputDevice leftController;
    private InputDevice rightController;

    void Start()
    {
        InitializeControllers();
    }

    void InitializeControllers()
    {
        var leftHandDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandDevices);
        if (leftHandDevices.Count > 0)
            leftController = leftHandDevices[0];

        var rightHandDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandDevices);
        if (rightHandDevices.Count > 0)
            rightController = rightHandDevices[0];
    }

    void Update()
    {
        bool isCrouching = CheckCrouchButton();

        if (isCrouching)
        {
            // Ajusta la altura del CharacterController a la altura agachada
            characterController.height = crouchHeight;
        }
        else
        {
            // Ajusta la altura del CharacterController a la altura normal
            characterController.height = normalHeight;
        }

        // Mantén el centro del CharacterController en el medio del cuerpo del jugador
        Vector3 center = characterController.center;
        center.y = characterController.height / 2;
        characterController.center = center;
    }

    bool CheckCrouchButton()
    {
        bool crouchPressed = false;

        if (leftController.isValid)
        {
            // Verifica si se presiona el botón primario del controlador izquierdo
            leftController.TryGetFeatureValue(CommonUsages.primaryButton, out bool leftPrimaryButton);
            crouchPressed |= leftPrimaryButton;
        }

        if (rightController.isValid)
        {
            // Verifica si se presiona el botón primario del controlador derecho
            rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool rightPrimaryButton);
            crouchPressed |= rightPrimaryButton;
        }

        return crouchPressed;
    }
}