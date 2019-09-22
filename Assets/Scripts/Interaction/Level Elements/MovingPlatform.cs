using System;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using Interaction.player;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interaction.Level_Elements
{
    public class MovingPlatform : MonoBehaviour
    {
        
        [SerializeField] private List<Transform> positions = new List<Transform>();
        [SerializeField] private float moveDuration;

        private int _position = 0;
        private Vector3 _initialPosition;
        private List<GameObject> _objectsOnPlatform = new List<GameObject>();
        private BoxCollider2D _collider;

        private Tween _transformTween;

        private Sequence _movePositions;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _collider = GetComponentInChildren<BoxCollider2D>();
            _rb = GetComponent<Rigidbody2D>();
            _initialPosition = transform.position;
            SetupTween();
        }

        private void SetupTween()
        {
            _movePositions = DOTween.Sequence();
            _movePositions.SetLoops(-1);
            foreach (var position in positions)
            {
                _movePositions.Append(transform.DOMove(position.position, moveDuration));
            }
            _movePositions.Append(transform.DOMove(_initialPosition, moveDuration));
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if(other.enabled)
                CheckForValidObjects(other.gameObject);
        }

        private void CheckForValidObjects(GameObject obj)
        {
            if (obj.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
            {
                var movement = obj.gameObject.GetComponent<Movement>();
                movement.OnJump += OnRemove;
                movement.OnMove += OnRemove;
                movement.OnStop += OnObjectStop;
            }
            else if (obj.gameObject.tag.Equals("Block"))
                obj.gameObject.GetComponent<BlockV2>().OnRecreated += OnRemove;
            else
                return;
            AddToList(obj);
        }

        private void OnObjectStop(GameObject objStopped)
        {
            var movement = objStopped.GetComponent<Movement>();
            var cols = Physics2D.OverlapBoxAll(_collider.transform.position + new Vector3(0, _collider.bounds.size.y, 0),
                _collider.bounds.size,
                0);
            if (cols.All(col => col.gameObject != objStopped))
            {
                movement.OnStop -= OnObjectStop; //LINQ statement that checks if any of the objects in the list gotten from the overlapbox
                return;
            }

            movement.OnJump += OnRemove;
            movement.OnMove += OnRemove;
            AddToList(objStopped);
        }

        private void OnRemove(GameObject objToRemove)
        {
            if(!_objectsOnPlatform.Contains(objToRemove)) return;
            
            //unassigning the movement events
            var movement = objToRemove.GetComponent<Movement>();
            if (movement)
            {
                movement.OnJump -= OnRemove;
                movement.OnMove -= OnRemove;
            }

            //unassigning the block event
            var block = objToRemove.GetComponent<BlockV2>();
            if (block)
            {
                block.OnRecreated -= OnRemove;
            }
            objToRemove.transform.SetParent(null);
            _objectsOnPlatform.Remove(objToRemove);
        }

        private void AddToList(GameObject objToAdd)
        {
            if (_objectsOnPlatform.Contains(objToAdd)) return;
            objToAdd.transform.SetParent(transform);
            _objectsOnPlatform.Add(objToAdd);
        }
    }
}
