using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoduleController : MonoBehaviour
{
    private List<GameObject> nodules = new List<GameObject>();

    public void AddNodule(GameObject nod)
    {
        nodules.Add(nod);
        float startingPos = -0.5f * nodules.Count;
        foreach (var obj in nodules)
        {
            var pos = obj.transform.position;
            pos.y = startingPos;
            startingPos += 1f;
        }
    }

    public void RemoveNodule(GameObject obj)
    {
        nodules.Remove(obj);
        DestroyImmediate(obj);
    }
}
