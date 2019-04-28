using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    //https://medium.com/@mattThousand/basic-2d-screen-shake-in-unity-9c27b56b516
    // Transform of the GameObject you want to shake
    // The initial position of the GameObject
    private Vector3 initialPosition;
    private Vector3 targetPosition;

    // Desired duration of the shake effect
    public float shakeDuration = 0f;

    // A measure of magnitude for the shake. Tweak based on your preference
    public float shakeMagnitude = 0.7f;

    // A measure of how quickly the shake effect should evaporate
    public float dampingSpeed = 1.0f;
    public float maxFrameDelta = 0.001f;
    public float directionChance = 0.05f;

    void Awake()
    {
    }

    void OnEnable()
    {
        initialPosition = transform.position;
    }
    void Update()
    {
        if (shakeDuration > 0)
        {
            targetPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, maxFrameDelta);

            shakeDuration -= Time.unscaledDeltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            transform.position = initialPosition;
        }
    }

    public void TriggerShake(float duration) {
        shakeDuration = duration;
        targetPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
    }

}
