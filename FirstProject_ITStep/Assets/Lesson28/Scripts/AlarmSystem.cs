using System.Collections;
using UnityEngine;

public class AlarmSystem : MonoBehaviour
{
    [SerializeField] private AudioSource _siren;
    [SerializeField] private Light[] _warningLights;
    [SerializeField] private float _flashInterval = 0.3f;

    private Coroutine _alarmCoroutine;
    private bool _isActive = false;

    public void AlarmPressed()
    {
        if (_isActive == false)
        {
            Activate();
        }
        else {
            Deactivate();
        }
    }

    public void Activate()
    {
        if (_isActive)
        {
            return;
        }

        _isActive = true;

        if (_siren != null)
        {
            _siren.Play();
        }

        _alarmCoroutine = StartCoroutine(FlashLights());
    }

    public void Deactivate()
    {
        if (!_isActive)
        {
            return;
        }

        _isActive = false;

        if (_siren != null)
        {
            _siren.Stop();
        }

        if (_alarmCoroutine != null)
        {
            StopCoroutine(_alarmCoroutine);
            _alarmCoroutine = null;
        }

        SetLights(false);
    }

    private IEnumerator FlashLights()
    {
        bool lightsEnabled = false;

        while (_isActive)
        {
            lightsEnabled = !lightsEnabled;
            SetLights(lightsEnabled);

            yield return new WaitForSeconds(_flashInterval);
        }
    }

    private void SetLights(bool state)
    {
        foreach (Light warningLight in _warningLights)
        {
            if (warningLight != null)
            {
                warningLight.enabled = state;
            }
        }
    }
}