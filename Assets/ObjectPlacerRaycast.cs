using UnityEngine;

public class ObjectPlacerRaycast : MonoBehaviour
{
    public int rayCount = 200;            // Nombre de rayons dans la sphère
    public float rayDistance = 5f;        // Distance maximale du Raycast
    public LayerMask platformLayer;

    void Start()
    {
        ScanSphereAndPlace();
    }

    void ScanSphereAndPlace()
    {
        float bestHeight = -Mathf.Infinity;
        Vector3 bestPoint = Vector3.zero;

        for (int i = 0; i < rayCount; i++)
        {
            // Génère un vecteur direction aléatoire uniforme sur une sphère
            Vector3 dir = Random.onUnitSphere;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, dir, out hit, rayDistance, platformLayer))
            {
                float height = hit.point.y;

                if (height > bestHeight)
                {
                    bestHeight = height;
                    bestPoint = hit.point;
                }
            }
        }

        if (bestHeight > -Mathf.Infinity)
        {
            transform.position = bestPoint + Vector3.up * 1f;
        }
        else
        {
            Debug.LogWarning("Aucune plateforme détectée dans toutes les directions");
        }
    }


    // Affichage des rayons dans l’éditeur
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        for (int i = 0; i < 100; i++)
        {
            Vector3 dir = Random.onUnitSphere;
            Gizmos.DrawRay(transform.position, dir * 2f);
        }
    }
}
