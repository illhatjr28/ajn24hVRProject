using UnityEngine;

public class ChaosKaleidoscope : MonoBehaviour
{
    [Header("Speed Bounds")]
    public float minSpeed = 20f;
    public float maxSpeed = 300f;

    [Header("Axis Probability")]
    [Range(0, 1)] public float chanceToRotateX = 0.7f;
    [Range(0, 1)] public float chanceToRotateY = 0.7f;
    [Range(0, 1)] public float chanceToRotateZ = 0.7f;

    private Vector3 rotationSpeeds;

    void Start()
    {
        RandomizeRotation();
    }

    public void RandomizeRotation()
    {
        // This 'do-while' loop ensures it NEVER picks (0,0,0)
        do
        {
            rotationSpeeds.x = (Random.value < chanceToRotateX) ? Random.Range(minSpeed, maxSpeed) * RandomSign() : 0;
            rotationSpeeds.y = (Random.value < chanceToRotateY) ? Random.Range(minSpeed, maxSpeed) * RandomSign() : 0;
            rotationSpeeds.z = (Random.value < chanceToRotateZ) ? Random.Range(minSpeed, maxSpeed) * RandomSign() : 0;
        }
        while (rotationSpeeds == Vector3.zero);
    }

    void Update()
    {
        // Spinning in Place
        transform.Rotate(rotationSpeeds * Time.deltaTime, Space.Self);
    }

    float RandomSign() => Random.value < 0.5f ? 1f : -1f;
}