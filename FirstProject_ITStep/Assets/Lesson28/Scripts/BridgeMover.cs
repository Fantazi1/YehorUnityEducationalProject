using System.Collections;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    [SerializeField] private GameObject _bridgeObj; 
    [SerializeField] private GameObject _leverObj;

    [Header("Settings")]
    [SerializeField] private float _bridgeDuration = 2f;
    [SerializeField] private float _leverDuration = 0.5f;

    [SerializeField] private Vector3 _bridgeRotationAngle = new Vector3(90f, 0f, 0f);
    [SerializeField] private Vector3 _leverRotationAngle = new Vector3(-60f, 0f, 0f);

    private bool _isActive;
    private Coroutine _bridgeCoroutine;

    public void Activate()
    {
        if (_isActive)
        {
            return;
        }

        _isActive = true;

        if (_leverObj != null)
        {
            StartCoroutine(LowerLeverRoutine());
        }

        if (_bridgeObj != null)
        {
            if (_bridgeCoroutine != null)
            {
                StopCoroutine(_bridgeCoroutine);
            }

            _bridgeCoroutine = StartCoroutine(LowerBridgeRoutine());
        }
    }

    private IEnumerator LowerLeverRoutine()
    {
        Quaternion startRotation = _leverObj.transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(_leverRotationAngle);

        float elapsedTime = 0f;

        while (elapsedTime < _leverDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _leverDuration;

            _leverObj.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null;
        }

        _leverObj.transform.rotation = endRotation;
    }

    private IEnumerator LowerBridgeRoutine()
    {
        Quaternion startRotation = _bridgeObj.transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(_bridgeRotationAngle);

        float elapsedTime = 0f;

        while (elapsedTime < _bridgeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _bridgeDuration;

            _bridgeObj.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null;
        }

        _bridgeObj.transform.rotation = endRotation;
    }
}