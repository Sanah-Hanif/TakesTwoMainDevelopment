using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ParallaxingTwo : MonoBehaviour
{
    [SerializeField] private Transform middlePoint;
    [SerializeField] private GameObject layer1;
    [SerializeField] private GameObject layer12;
    [SerializeField] private GameObject layer13;

    float imageSize;
    Vector2 midPointPos, layer1pos, layer12pos, layer13pos;
    [SerializeField] float layer1speed;

    Vector2 oldMidpointPos;

    // Start is called before the first frame update
    void Start()
    {
        midPointPos = new Vector2(middlePoint.position.x, middlePoint.position.y);
        layer1pos= new Vector2(layer1.transform.position.x, layer1.transform.position.y);
        layer12pos = new Vector2(layer12.transform.position.x, layer12.transform.position.y);
        layer12pos = new Vector2(layer13.transform.position.x, layer13.transform.position.y);
        //print("staring layer 1 pos is " + layer1pos);
        imageSize = layer1.GetComponent<SpriteRenderer>().bounds.size.x;
        //print(imageSize);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 currentMidPointPos = new Vector2(middlePoint.position.x, middlePoint.position.y);
        //print(Vector2.Distance(currentMidPointPos, midPointPos));
        Vector3 newLayer1Pos = new Vector3(layer1pos.x + (-currentMidPointPos.x + midPointPos.x) * layer1speed, layer1pos.y + (-currentMidPointPos.y + midPointPos.y) * layer1speed, 0);
        Vector3 newLayer12Pos = new Vector3(layer12pos.x + (-currentMidPointPos.x + midPointPos.x) * layer1speed, layer12pos.y + (-currentMidPointPos.y + midPointPos.y) * layer1speed, 0);
        Vector3 newLayer13Pos = new Vector3(layer13pos.x + (-currentMidPointPos.x + midPointPos.x) * layer1speed, layer13pos.y + (-currentMidPointPos.y + midPointPos.y) * layer1speed, 0);


        //layer1.transform.DOMove(newLayer1Pos, 0.01);
        layer1.transform.position = newLayer1Pos;
        layer12.transform.position = newLayer12Pos;
        layer13.transform.position = newLayer13Pos;

        int newTimes = (int)((-currentMidPointPos.x + midPointPos.x) / imageSize);
        int oldTimes = (int)((-oldMidpointPos.x + midPointPos.x) / imageSize);
        print(currentMidPointPos.x);
        //print("old times = " + (-currentMidPointPos.x + midPointPos.x));
        //print("new times = " + (-oldMidpointPos.x + midPointPos.x));

        oldMidpointPos = new Vector2(currentMidPointPos.x, currentMidPointPos.y);
    }
}
