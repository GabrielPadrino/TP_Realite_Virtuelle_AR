using System.Collections;
using UnityEngine;

public class ObjectAnimator : MonoBehaviour
{
    public float floatHeight = 0.1f;
    public float floatSpeed = 2f;
    public float rotateSpeed = 50f;

    public float maxRotation = 180f;

    private float currentRotation = 0f;
    private Vector3 startPos;
    private Renderer rend;

    private float visibilityTimer = 0f;
    private const float delayBeforeAnim = 2f;

    void Start()
    {
        rend = GetComponent<Renderer>();
        startPos = transform.position;
        StartCoroutine(FloatAndRotate());
    }

    IEnumerator FloatAndRotate()
    {
        while (true)
        {
            if (rend.isVisible)
            {
                // Incrémente le timer quand on voit l'objet
                visibilityTimer += Time.deltaTime;

                // Lance l'animation seulement après 2 sec de visibilité
                if (visibilityTimer >= delayBeforeAnim)
                {
                    // Lévitation
                    float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
                    transform.position = new Vector3(transform.position.x, newY, transform.position.z);

                    // Rotation
                    currentRotation += rotateSpeed * Time.deltaTime;
                    if (currentRotation >= maxRotation)
                        currentRotation = 0f;

                    transform.rotation = Quaternion.Euler(0, currentRotation, 0);
                }
            }
            else
            {
                // On ne voit plus l’objet → reset total
                visibilityTimer = 0f;
                currentRotation = 0f;

                // Reset position et rotation
                transform.position = startPos;
                transform.rotation = Quaternion.identity;
            }

            yield return null;
        }
    }
}
