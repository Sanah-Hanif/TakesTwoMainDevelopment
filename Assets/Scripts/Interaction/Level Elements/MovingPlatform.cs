using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Interaction.Level_Elements
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovingPlatform : MonoBehaviour
    {
        
        [SerializeField] private List<Transform> positions = new List<Transform>();
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float moveDuration;

        private int _position = 0;
        private int _totalPostions;
        private Vector3 _initialPosition;

        private Tween _transformTween;

        private Sequence _movePositions;

        private void Awake()
        {
            _initialPosition = transform.position;
            _totalPostions = positions.Count;
            SetupTween();
        }

        private void SetupTween()
        {
            _movePositions = DOTween.Sequence();
            _movePositions.SetLoops(-1);
            foreach (var position in positions)
            {
                _movePositions.Append(rb.DOMove(position.position, moveDuration));
            }
            _movePositions.Append(rb.DOMove(_initialPosition, moveDuration));
        }
    }
}
