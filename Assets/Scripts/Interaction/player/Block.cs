using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interaction.Level_Elements;
using UnityEngine;
using UnityEngine.InputSystem;

public class Block : PlayerCreationInteraction
{
    private Dictionary<int, PressurePad> _pads = new Dictionary<int, PressurePad>();

    private Collider2D _collider2D;

    private void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
    }

    public override void ReCreated()
    {
        ClearPads();
    }

    public override void OnPlaced(InputAction.CallbackContext ctx)
    {
        
        Debug.Log("Placed");
        
        var cols = Physics2D.OverlapBoxAll(_collider2D.bounds.center, _collider2D.bounds.extents, 0);
        foreach (var col in cols.Where(col => col.GetComponent<PressurePad>()))
        {
            AddPad(col.GetComponent<PressurePad>());
            Debug.Log(col.name);
        }
        
        //_ability.GetAction("Place").performed -= OnPlaced;
    }

    public void AddPad(PressurePad pad)
    {
        if (_pads.ContainsKey(pad.GetInstanceID())) return;
        _pads.Add(pad.GetInstanceID(), pad);
        pad.AddObjectOnPad(gameObject);
    }

    public void RemovePad(PressurePad pad)
    {
        _pads.Remove(pad.GetInstanceID());
        pad.RemoveObjectOnPad(this.instanceID);
    }

    private void ClearPads()
    {
        var itemsToRemove = _pads.ToArray();
        foreach (var item in itemsToRemove)
            RemovePad(item.Value);
        _ability.GetAction("Place").performed -= OnPlaced;
    }

    private void OnDestroy()
    {
        ClearPads();
    }
}
