using System;
using System.Collections.Generic;
using Interaction.Level_Elements;
using Player;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace Interaction.player
{
    public class Airblast : PlayerCreationInteraction
    {

        [SerializeField] private float velocity = 3f;
        [SerializeField] private float destroyTime = 2f;

        private Collider2D[] _cols;
        private List<Rigidbody2D> _rigidbody = new List<Rigidbody2D>();
        private bool _initialized = false;
    
        public override void Interact()
        {
            var collider = GetComponent<Collider2D>();
            _cols = Physics2D.OverlapBoxAll(transform.position, collider.bounds.size, 0);
            foreach (var col in _cols)
            {
                var rb = col.GetComponent<Rigidbody2D>();
                if (rb)
                {
                    _rigidbody.Add(rb);
                }

                var _switch = col.GetComponent<Switch>();
                if(_switch)
                    _switch.Interact();
            }
            _initialized = true;
            Destroy(gameObject, destroyTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var otherRb = other.GetComponent<Rigidbody2D>();
            if (!otherRb) return;
            _initialized = true; //remove line after debugging
            AddObject(otherRb);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var otherRb = other.GetComponent<Rigidbody2D>();
            if (!otherRb) return;
            RemoveObject(otherRb);
        }

        public void AddObject(Rigidbody2D rigidbody2D)
        {
            if (_rigidbody.Contains(rigidbody2D)) return;
            var movement = rigidbody2D.GetComponent<Movement>();
            if (movement) movement.CanMove = false;
            _rigidbody.Add(rigidbody2D);
        }

        public void RemoveObject(Rigidbody2D rigidbody2D)
        {
            if (!_rigidbody.Contains(rigidbody2D)) return;
            var movement = rigidbody2D.GetComponent<Movement>();
            if (movement) movement.CanMove = true;
            _rigidbody.Remove(rigidbody2D);
        }

        private void FixedUpdate()
        {
            if (!_initialized)
                return;
            foreach (var rb in _rigidbody)
            {
                rb.velocity += 5 * Time.fixedDeltaTime * velocity * (Vector2)transform.right;
                //Debug.Log(rb.velocity);
            }
        }

        private void OnDestroy()
        {
            if(_rigidbody.Count > 0) _rigidbody.Clear();
        }
    }
}
