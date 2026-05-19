using UnityEngine;

public class CoinsAndCrystalsRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float speedScaler = 10f;
    [SerializeField] private float speedFly = 3f;
    [SerializeField] private float height = 0.5f;

    private Vector3 startPosition;
    private void Start()
    {
        startPosition = transform.position;
    }
    void FixedUpdate()
    {
        float offsetY = Mathf.Sin(Time.time * speedFly) * height;
        transform.position = new Vector3(startPosition.x, startPosition.y + offsetY, startPosition.z);
        transform.Rotate(Vector3.up * rotationSpeed * speedScaler * Time.fixedDeltaTime);
    }
}
