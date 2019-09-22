using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic2dParalax : MonoBehaviour
{
    private float span, startPos;
    public float paralax;
    public GameObject MainCamera;
    
    
    void Start() {
        startPos = transform.position.x;
        span = GetComponent<SpriteRenderer>().bounds.size.x;


                 }

    void FixedUpdate() {
  
        float dist = (MainCamera.transform.position.x * paralax);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

       
    }
}
