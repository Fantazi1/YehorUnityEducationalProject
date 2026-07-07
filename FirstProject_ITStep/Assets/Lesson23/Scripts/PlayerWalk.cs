using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerWalk : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Rigidbody rb;
    private float inputX;
    private float inputZ;
    private Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal"); 
        inputZ = Input.GetAxisRaw("Vertical");

        moveDirection = (-transform.forward * inputZ + -transform.right * inputX).normalized;
    }

    void FixedUpdate()
    {
        MovePlayer();
        rb.angularVelocity = Vector3.zero;
    }

    private void MovePlayer()
    {
        Vector3 targetVelocity = moveDirection * speed;

        targetVelocity.y = rb.linearVelocity.y;

        rb.linearVelocity = targetVelocity;
    }
}
