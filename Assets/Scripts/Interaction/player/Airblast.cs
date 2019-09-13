using System;
using System.Collections.Generic;
using Interaction.Level_Elements;
using Interactions;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace Interaction.player
{
    public class Airblast : PlayerCreationInteraction
    {

        [SerializeField] private float velocity = 3f;
        [SerializeField] private float destroyTime = 2f;

        private Collider2D[] cols;
        private List<Rigidbody2D> _rigidbody2D;
        private bool initialized = false;
    
        public override void Interact()
        {
            _rigidbody2D = new List<Rigidbody2D>();
            Collider2D col = GetComponent<Collider2D>();
            cols = Physics2D.OverlapBoxAll(transform.position, col.bounds.size, 
                transform.rotation.eulerAngles.z);
            foreach (var collider in cols)
            {
                var rb = collider.GetComponent<Rigidbody2D>();
                if (rb)
                {
                    _rigidbody2D.Add(rb);
                }

                var _switch = collider.GetComponent<Switch>();
                if(_switch)
                    _switch.Interact();
            }
            initialized = true;
            Destroy(gameObject, destroyTime);
        }

        private void FixedUpdate()
        {
            if(!initialized)
                return;
            foreach (var rb in _rigidbody2D)
            {
                rb.velocity = transform.right * velocity;
            }
        }

        private void OnDestroy()
        {
            _rigidbody2D.Clear();
        }
    }
}
