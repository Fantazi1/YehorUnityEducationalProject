using UnityEngine;
using System.Collections;

public class TileLesson22 : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteRenderer _highlightRenderer;
    [SerializeField] private ParticleSystem _destroyEffect;

    private int _column;
    private int _row;
    private TileType _type;
    private Board _board;
    private Vector3 _targetPosition;
    private bool _isMoving;
    [SerializeField] private float _moveSpeed = 12f;

    public int Column => _column;
    public int Row => _row;
    public TileType Type => _type;
    public bool IsMoving => _isMoving;

    public event System.Action<TileLesson22> Clicked;

    public void Initialize(Board board, int column, int row, TileType type, Sprite sprite, Color color)
    {
        _board = board;
        _column = column;
        _row = row;
        _type = type;
        _spriteRenderer.sprite = sprite;
        _spriteRenderer.color = color;
        _targetPosition = transform.position;
        _isMoving = false;
        SetHighlight(false);
    }

    public void SetGridPosition(int column, int row)
    {
        _column = column;
        _row = row;
    }

    public void MoveToPosition(Vector3 position)
    {
        _targetPosition = position;

        if (Vector3.Distance(transform.position, _targetPosition) < 0.001f)
        {
            transform.position = _targetPosition;
            _isMoving = false;
            return;
        }

        _isMoving = true;
        StopAllCoroutines();
        StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        while (Vector3.Distance(transform.position, _targetPosition) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = _targetPosition;
        _isMoving = false;
    }

    public void SetHighlight(bool state)
    {
        if (_highlightRenderer != null)
        {
            _highlightRenderer.gameObject.SetActive(state);
        }
    }

    public void PlayDestroyEffect()
    {
        if (_destroyEffect != null)
        {
            ParticleSystem effect = Instantiate(_destroyEffect, transform.position, Quaternion.identity);
            Destroy(effect.gameObject, effect.main.duration);
        }
    }

    private void OnMouseDown()
    {
        if (_isMoving || (_board != null && _board.IsProcessing)) return;
        Clicked?.Invoke(this);
    }
}