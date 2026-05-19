using UnityEngine;

public class BarrelMoving : MonoBehaviour
{
    [SerializeField] private float movingSpeed = 5f;
    [SerializeField] private float speedScaler = 10f;

    private Vector3 startPosition;
    private void Start()
    {
        startPosition = transform.position;
    }
    void FixedUpdate()
    {
        transform.Translate(new Vector3(0, 0, -1 * movingSpeed * speedScaler * Time.deltaTime), Space.World);
    }
}
