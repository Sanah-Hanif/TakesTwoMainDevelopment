using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ParallaxingScript : MonoBehaviour
{
    [SerializeField] float length, startPos, parallaxEffect;
    [SerializeField] GameObject cam;


    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float dist = (cam.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startPos * dist, transform.position.y, transform.position.y);

    }
}
