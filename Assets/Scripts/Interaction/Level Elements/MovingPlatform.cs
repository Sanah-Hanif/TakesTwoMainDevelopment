using System;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using Interaction.player;
using Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interaction.Level_Elements
{
    [ExecuteInEditMode]
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

        
        
        #if UNITY_EDITOR

        private List<Vector3> _positionsToMove = new List<Vector3>();
        private int _index = 0;
        
        private void OnValidate()
        {
            EditorApplication.playModeStateChanged += StateChange;
            _index = 0;
            _collider = GetComponentInChildren<BoxCollider2D>();
            _rb = GetComponent<Rigidbody2D>();
            SetPosition();
        }

        public void SetPosition()
        {
            _positionsToMove.Clear();
            _index = 0;
            _initialPosition = transform.position;
            _positionsToMove.Add(_initialPosition);
            foreach (var pos in positions)
            {
                _positionsToMove.Add(pos.position);
            }
        }

        public void NextPosition()
        {
            _index++;
            _index %= _positionsToMove.Count;
            transform.position = _positionsToMove[_index];
        }

        void StateChange(PlayModeStateChange mode)
        {
            if(mode == PlayModeStateChange.ExitingEditMode)
                ResetPosition();
        }

        public void ResetPosition()
        {
            _index = 0;
            transform.position = _positionsToMove[0];
        }

        #endif

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
            
            var dot = Vector2.Dot(other.GetContact(0).normal, Vector2.down);
            Debug.Log(dot);
            if(other.enabled && dot > 0.9f)
                CheckForValidObjects(other.gameObject);
        }

        private void CheckForValidObjects(GameObject obj)
        {
            bool addPlayer = true;
            if (obj.gameObject.CompareTag("Player"))
            {
                var movement = obj.gameObject.GetComponent<Movement>();
                movement.OnJump += OnRemove;
                movement.OnMove += OnRemove;
                movement.OnStop += OnObjectStop;
                addPlayer = !movement.IsMoving;
            }
            else if (obj.gameObject.CompareTag("Block"))
                obj.gameObject.GetComponent<BlockV2>().OnRecreated += OnRemove;
            else
                return;
            if(addPlayer)
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

        private void OnTriggerExit2D(Collider2D other)
        {
            OnRemove(other.gameObject);
        }
    }
}
