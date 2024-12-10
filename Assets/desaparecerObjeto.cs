using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class DesaparecerAlSoltar : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private MeshRenderer meshRenderer; // Para controlar la visibilidad
    private Collider objectCollider;   // Para desactivar las colisiones
    public float fadeDuration = 1.0f;  // Duración del desvanecimiento

    void Awake()
    {
        // Obtener componentes necesarios
        meshRenderer = GetComponent<MeshRenderer>();
        objectCollider = GetComponent<Collider>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Suscribirse a eventos
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnDestroy()
    {
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        Debug.Log("Objeto soltado, iniciando desvanecimiento");
        // Iniciar desvanecimiento
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        if (meshRenderer != null)
        {
            Material material = meshRenderer.material;
            Color initialColor = material.color;
            float elapsedTime = 0f;

            // Desactivar colisiones e interacción
            if (objectCollider != null)
                objectCollider.enabled = false;
            
            grabInteractable.enabled = false;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
                material.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
                yield return null;
            }

            // Asegurarse de que el objeto esté completamente invisible
            material.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
            meshRenderer.enabled = false;

            Debug.Log("Desvanecimiento completado, objeto invisible");
        }
    }
}