using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadShifter : MonoBehaviour
{
    [SerializeField] Transform followingPoint;
    Vector2 currentPos, prevPos;
    float offsetx, offsety;
    [SerializeField] bool isCloud;
    [SerializeField] bool isShader = false;

    [SerializeField] float speedx, speedy, cloudSpeed;
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

        if (isCloud)
        {
            offsetx += cloudSpeed * Time.deltaTime;
        }

        rend.material.SetTextureOffset("_MainTex", new Vector2(offsetx, offsety));
        if(!isShader)
            rend.material.SetTextureOffset("_BaseMap", new Vector2(offsetx, offsety));

        prevPos = currentPos;
        
    }
}
