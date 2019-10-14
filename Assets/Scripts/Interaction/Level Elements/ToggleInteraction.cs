using System;
using System.Collections;
using System.Collections.Generic;
using Interactions;
using UnityEngine;

namespace Interaction.Level_Elements
{
    public class ToggleInteraction : EnvironmentInteraction
    {

        [SerializeField] private List<Vector2> positions = new List<Vector2>();
        [SerializeField] private List<Vector3> rotation = new List<Vector3>();

        private int _toggled = 0;
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
            if(positions.Count > 0)
                _transform.localPosition = positions[0];
            if(rotation.Count > 0)
                _transform.rotation = Quaternion.Euler(rotation[0]);
        }

        public override void Interact()
        {
            Toggle();
        }

        private void Toggle()
        {
            _toggled += 1;
            _toggled %= 2;
            if(_toggled < positions.Count)
                _transform.localPosition = positions[_toggled];
            if(_toggled < rotation.Count)
                _transform.rotation = Quaternion.Euler(rotation[_toggled]);
        }
    }

    /*public class Toggle
    {
        private Vector2 position;
        private Quaternion rotation;

        public Toggle(Vector2 _position, Quaternion _quaternion)
        {
            position = _position;
            rotation = _quaternion;
        }

    }*/
}