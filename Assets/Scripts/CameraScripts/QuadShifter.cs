using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadShifter : MonoBehaviour
{
    [SerializeField] Transform followingPoint;
    Vector2 currentPos, prevPos;
    float offsetx, offsety;

    [SerializeField] float speedx, speedy;
    Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = this.GetComponent<Renderer>();
        currentPos = new Vector2(followingPoint.position.x, followingPoint.position.y);
        prevPos = currentPos;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentPos = new Vector2(followingPoint.position.x, followingPoint.position.y);
        float dist = currentPos.x - prevPos.x;
        float dist2 = currentPos.y - prevPos.y;
        offsetx += dist * speedx * Time.deltaTime;
        offsety += dist2 * speedy * Time.deltaTime;
        rend.material.SetTextureOffset("_MainTex", new Vector2(offsetx, offsety));
        rend.material.SetTextureOffset("_BaseMap", new Vector2(offsetx, offsety));

        prevPos = currentPos;
        
    }
}
