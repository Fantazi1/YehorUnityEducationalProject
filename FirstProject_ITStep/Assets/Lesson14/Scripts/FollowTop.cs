using Unity.VisualScripting;
using UnityEngine;

public class FollowTop : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _height = 40f;
    [SerializeField] private bool _followRotation = false;

    private void LateUpdate()
    {
        if (_player == null)
        {
            return;
        }

        transform.position = _player.position + Vector3.up * _height;

        if (_followRotation)
        {
            transform.rotation = Quaternion.Euler(90, _player.eulerAngles.y, 0f);
        }
    }
}
