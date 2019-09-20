using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementLimiter : MonoBehaviour
{
    public Camera cam;
    private float size, ratio;

    public GameObject[] players;

    // Start is called before the first frame update
    void Start()
    {
        size = cam.orthographicSize;
        ratio = cam.aspect;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        size = cam.orthographicSize;
        Vector2 position = Vector2.zero;
        
        foreach ( GameObject p in players)
        {
            position.x = Mathf.Clamp(p.transform.position.x, ratio * size * -1, ratio * size);
            position.y = Mathf.Clamp(p.transform.position.y, size, size);
            p.transform.position = position;
        }
    }
}
