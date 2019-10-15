using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentParallax : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private float minZoom = 10f;
    [SerializeField] private float maxZoom = 15f;

    private Vector3 _scale = Vector3.one;

    // Update is called once per frame
    void Update()
    {
        var orthoSize = camera.orthographicSize;

        Debug.Log(orthoSize);
        var newScale = orthoSize / minZoom;
        Debug.Log(newScale);

        _scale.x = newScale;
        _scale.y = newScale;
        transform.localScale = Vector3.Lerp(transform.localScale, _scale, Time.deltaTime);
    }
}
