using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ParallaxingThree : MonoBehaviour
{
    [SerializeField] Transform playerFollowingPoint;
    float imageSize, spriteSize;
    Vector2 oldFollowingPoint;
    

    [SerializeField] Transform[] layer1Images;
    Vector2[] layer1Positions;
    Vector2 midPointPos;
    [SerializeField] float layer1speed;
    [SerializeField] Transform layer1Behind;

    // Start is called before the first frame update
    void Start()
    {
        midPointPos = new Vector2(playerFollowingPoint.position.x, playerFollowingPoint.position.y);
        imageSize = layer1Images[0].GetComponent<SpriteRenderer>().bounds.size.x;
        spriteSize = layer1Images[0].GetComponent<SpriteRenderer>().size.x;
        //print(imageSize);
        layer1Positions = new Vector2[3];//change this to 3 if you want 3 image

        int index = 0;

        foreach (Transform image in layer1Images)
        {
            layer1Positions[index] = new Vector2(image.position.x, image.position.y);
            index++;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 currentFollowingPoint = new Vector2(playerFollowingPoint.position.x, playerFollowingPoint.position.y);

        Vector3[] newLayer1Pos = new Vector3[3];//change to 3 if 3 images
        int index = 0;
        foreach (Vector2 vec in layer1Positions)
        {
            newLayer1Pos[index] = new Vector3(vec.x + (-currentFollowingPoint.x + midPointPos.x) * layer1speed, vec.y + (-currentFollowingPoint.y + midPointPos.y) * layer1speed, 0);
            //layer1Images[index].position = newLayer1Pos[index];
            layer1Images[index].DOMove(newLayer1Pos[index], 0.1f);
            index++;
        }

        //if (Input.GetKeyDown(KeyCode.P)) {
        //    if (layer1Behind == layer1Images[0])
        //    {
        //        Vector3 newPos = new Vector3(spriteSize * 6f, 0, 0);
        //        newPos = layer1Images[0].position + newPos;
        //        print(newPos);
        //        layer1Behind = layer1Images[1];
        //    } else
        //        layer1Behind = layer1Images[0];
        //}


    }

    private void LateUpdate()
    {
        
    }
}
