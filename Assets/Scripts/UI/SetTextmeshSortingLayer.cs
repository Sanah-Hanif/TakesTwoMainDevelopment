using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTextmeshSortingLayer : MonoBehaviour
{
    [SerializeField] private int sortingLayerVal;
    [SerializeField] private string whatLayer;
    private MeshRenderer _rend;
    // Start is called before the first frame update
    void Start()
    {
        _rend = this.GetComponent<MeshRenderer>();
        _rend.sortingOrder = sortingLayerVal;
        if (whatLayer != "")
            _rend.sortingLayerName = whatLayer;
    }

}
