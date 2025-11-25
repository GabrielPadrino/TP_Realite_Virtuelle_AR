using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PieceData
{
    public Transform piece;        // La pièce à déplacer
    public Transform targetPoint;  // Son point cible
    [HideInInspector] public Vector3 originalPosition; // position initiale dans le monde
    [HideInInspector] public Quaternion originalRotation;
}

public class PiecesMover : MonoBehaviour
{
    [Header("Références")]
    public Renderer mainRenderer;          // Renderer principal pour détecter la visibilité
    public List<PieceData> piecesData;     // Liste des pièces et leurs targets

    [Header("Paramètres")]
    public float moveSpeed = 2f;           // vitesse de déplacement
    public float delayBeforeMove = 2f;     // délai avant de bouger
    public float floatHeight = 0.1f;       // amplitude flottement
    public float floatSpeed = 2f;          // vitesse flottement
    public float rotateSpeed = 50f;        // rotation des pièces

    private float visibilityTimer = 0f;
    private bool movingToTarget = false;

    void Start()
    {
        foreach (var pd in piecesData)
        {
            if (pd.piece != null)
            {
                pd.originalPosition = pd.piece.position;
                pd.originalRotation = pd.piece.rotation;
            }
        }
    }

    void Update()
    {
        // Vérification de visibilité
        if (mainRenderer != null && mainRenderer.isVisible)
        {
            visibilityTimer += Time.deltaTime;
            if (visibilityTimer >= delayBeforeMove)
                movingToTarget = true;
        }
        else
        {
            visibilityTimer = 0f;
            movingToTarget = false;
        }

        // Déplacement des pièces
        foreach (var pd in piecesData)
        {
            if (pd.piece == null || pd.targetPoint == null) continue;

            // Choix de la position cible
            Vector3 targetPos = movingToTarget ? pd.targetPoint.position : pd.originalPosition;

            // Flottement vertical
            float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
            Vector3 finalPos = new Vector3(targetPos.x, targetPos.y + yOffset, targetPos.z);

            // Lerp en position dans le monde
            pd.piece.position = Vector3.Lerp(pd.piece.position, finalPos, Time.deltaTime * moveSpeed);

            // Rotation
            if (movingToTarget)
                pd.piece.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.Self);
            else
                pd.piece.rotation = Quaternion.Lerp(pd.piece.rotation, pd.originalRotation, Time.deltaTime * moveSpeed);
        }
    }
}
