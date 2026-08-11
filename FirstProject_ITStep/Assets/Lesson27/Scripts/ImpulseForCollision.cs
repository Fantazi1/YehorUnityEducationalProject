using UnityEngine;
using Unity.Cinemachine; // Для Cinemachine 3.x (або "using Cinemachine;" для 2.x)

[RequireComponent(typeof(CinemachineImpulseSource))]
public class ImpulseForCollision: MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        impulseSource.GenerateImpulse();
    }
}