using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLimiterTwo : MonoBehaviour
{
    [SerializeField] private Transform midPoint;
    [SerializeField] private float maxDistanceX, maxDistanceY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //_ratio = _camera.aspect;
        Vector2 position = Vector2.zero;
        position.x = Mathf.Clamp(this.transform.position.x, midPoint.position.x - maxDistanceX, midPoint.position.x + maxDistanceX);
        position.y = Mathf.Clamp(this.transform.position.y, midPoint.position.y - maxDistanceY, midPoint.position.y + maxDistanceY);
        this.transform.position = position;
    }
}
