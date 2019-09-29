using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLimiter : MonoBehaviour
{
    private Camera _camera;

    [SerializeField] private Rigidbody2D _player;

    private float _size;
    private float _ratio;

    void Start()
    {
        _camera = Camera.main;
        _size = _camera.orthographicSize;
        _ratio = _camera.aspect;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _size = _camera.orthographicSize;
        _ratio = _camera.aspect;
        Vector2 position = Vector2.zero;
        position.x = Mathf.Clamp(_player.position.x, _ratio * _size * -1, _ratio * _size);
        position.y = Mathf.Clamp(_player.position.y, -_size, _size);
        _player.position = position;
    }
}
