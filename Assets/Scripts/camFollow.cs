using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camFollow : MonoBehaviour
{
    [SerializeField]
    public float xMin;

    [SerializeField]
    public float xMax;

    [SerializeField]
    public float yMin;

    [SerializeField]
    public float yMax;

    public Transform focus;
    // Use this for initialization
    void Start()
    {
        focus = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(focus.position.x, xMin, xMax), Mathf.Clamp(focus.position.y, yMin, yMax), transform.position.z);
    }
}
