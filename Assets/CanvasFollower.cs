using UnityEngine;
using UnityEngine.UI;

public class CanvasFollower : MonoBehaviour
{
    [Header("Références")]
    public Transform targetObject;     // L'objet autour duquel le canvas se place
    public Camera playerCamera;        // Caméra du joueur
    public Renderer visibilityRenderer;// Renderer de l’objet pour savoir si on le regarde

    [Header("Offsets & paramètres")]
    public float distanceToRight = 1f;    // distance à droite de l'objet
    public float heightOffset = 1f;       // hauteur du canvas
    public float appearDelay = 2f;        // délai avant apparition
    public float followSpeed = 5f;        // douceur du mouvement du canvas

    private CanvasGroup canvasGroup;      // pour gérer visibilité
    private float visibilityTimer = 0f;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f; // invisible au début
    }

    void Update()
    {
        if (playerCamera == null || targetObject == null || visibilityRenderer == null)
            return;

        HandleVisibility();
        UpdatePosition();
        BillboardTowardsCamera();
    }

    void HandleVisibility()
    {
        if (visibilityRenderer.isVisible)
        {
            visibilityTimer += Time.deltaTime;

            if (visibilityTimer >= appearDelay)
                canvasGroup.alpha = 1f;   // apparaît
        }
        else
        {
            visibilityTimer = 0f;
            canvasGroup.alpha = 0f;       // disparaît instantanément
        }
    }

    void UpdatePosition()
    {
        // Calcul de la droite de l'objet par rapport à la caméra
        Vector3 rightDir = Vector3.Cross(Vector3.up, playerCamera.transform.forward).normalized;

        // Nouvelle position du canvas
        Vector3 targetPos = targetObject.position
                            + rightDir * distanceToRight
                            + Vector3.up * heightOffset;

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
    }

    void BillboardTowardsCamera()
    {
        transform.LookAt(playerCamera.transform);

        // Si tu veux que le canvas soit lisible (pas à l’envers)
        transform.rotation = Quaternion.LookRotation(transform.position - playerCamera.transform.position);
    }
}
