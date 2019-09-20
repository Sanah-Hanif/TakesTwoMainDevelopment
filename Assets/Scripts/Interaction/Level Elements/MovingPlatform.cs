using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Interaction.Level_Elements
{
    public class MovingPlatform : MonoBehaviour
    {
        
        [SerializeField] private List<Transform> positions = new List<Transform>();
        [SerializeField] private float moveDuration;

        private int _position = 0;
        private Vector3 _initialPosition;

        private Tween _transformTween;

        private Sequence _movePositions;

        private void Awake()
        {
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
    }
}
