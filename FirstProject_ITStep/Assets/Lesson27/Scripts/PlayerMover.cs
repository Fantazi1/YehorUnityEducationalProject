using Unity.Cinemachine;
using UnityEngine;

namespace CameraDemo
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMover : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float gravity = -9.81f;

        [Header("Impact Shake Settings")]
        [Tooltip("Мінімальна затримка між струсами (у секундах)")]
        [SerializeField] private float shakeCooldown = 0.4f;

        private CharacterController controller;
        [SerializeField] private CinemachineImpulseSource impulseSource;
        private Vector3 playerVelocity;
        private float lastShakeTime;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (controller.isGrounded && playerVelocity.y < 0)
            {
                playerVelocity.y = -2f;
            }

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.sqrMagnitude > 0.01f)
            {
                controller.Move(direction * (moveSpeed * Time.deltaTime));

                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            playerVelocity.y += gravity * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.normal.y < 0.5f && Time.time > lastShakeTime + shakeCooldown)
            {
                impulseSource.GenerateImpulse();
                lastShakeTime = Time.time;
            }
        }
    }
}
