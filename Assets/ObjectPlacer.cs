using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    public string platformTag = "Platform";

    void Start()
    {
        PlaceOnHighestPlatform();
    }

    void PlaceOnHighestPlatform()
    {
        GameObject[] platforms = GameObject.FindGameObjectsWithTag(platformTag);
        GameObject bestPlatform = null;
        float bestScore = float.MinValue;

        foreach (GameObject platform in platforms)
        {
            float distance = Vector3.Distance(transform.position, platform.transform.position);
            float height = platform.transform.position.y;

            float score = height - distance * 0.1f;

            if (score > bestScore)
            {
                bestScore = score;
                bestPlatform = platform;
            }
        }

        if (bestPlatform != null)
        {
            Vector3 targetPos = bestPlatform.transform.position + Vector3.up * 1f;
            transform.position = targetPos;
        }
    }
}
