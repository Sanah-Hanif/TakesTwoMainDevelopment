using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadShifter : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = this.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            float offset = Time.time * speed;
            rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
            rend.material.SetTextureOffset("_BaseMap", new Vector2(offset, 0));
        }
    }
}
