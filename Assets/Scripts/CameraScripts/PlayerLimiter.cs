using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerLimiter : MonoBehaviour
{
    private Camera _camera;
    public CinemachineVirtualCamera vCam;

    [SerializeField] private Rigidbody2D _player;

    private float _size;
    private float _ratio;
    [SerializeField] float range;

    void Start()
    {
        _camera = Camera.main;
        _size = vCam.m_Lens.OrthographicSize;
        _ratio = vCam.m_Lens.Aspect;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        
        //_ratio = _camera.aspect;
        Vector2 position = Vector2.zero;
        position.x = Mathf.Clamp(_player.position.x, this.transform.position.x -_size * _ratio * range, this.transform.position.x + _size * _ratio * range);
        position.y = Mathf.Clamp(_player.position.y, this.transform.position.y -_size, this.transform.position.y + _size * 2);
        _player.position = position;
    }
}
