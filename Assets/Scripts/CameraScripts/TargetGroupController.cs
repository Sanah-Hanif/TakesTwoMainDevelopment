using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace CameraScripts
{
    public class TargetGroupController : MonoBehaviour
    {
        [SerializeField] private float startWeight = 2.5f, startRadius = 2.5f, endWeight = 5f, endRadius = 5f, lerpDuration = 2f;
        private CinemachineTargetGroup _target;
        private Dictionary<GameObject, IEnumerator> _routines = new Dictionary<GameObject, IEnumerator>();

        private void Awake()
        {
            _target = GetComponent<CinemachineTargetGroup>();
        }

        public void AddObjectToTargetGroup(GameObject objToAdd)
        {
            var routine = IncreaseWeight(objToAdd);
            if(_routines.ContainsKey(objToAdd)) return;
            _routines.Add(objToAdd, routine);
            StartCoroutine(routine);
        }

        public void RemoveObjectFromTargetGroup(GameObject objToRemove)
        {
            
        }

        private IEnumerator IncreaseWeight(GameObject objToIncrease)
        {
            
            var currentWeight = startWeight;
            var currentWeightLerp = 0f;
            var currentRadius = startRadius;
            var currentRadiusLerp = 0f;
            _target.AddMember(objToIncrease.transform, currentWeight, currentRadius);
            var member = _target.FindMember(objToIncrease.transform);
            
            var endFrame = new WaitForEndOfFrame();
            
            yield return endFrame;
            
            while (currentRadiusLerp < 1 && currentWeightLerp < 1)
            {
                //currentRadius += Time.deltaTime * (endRadius - startRadius) * (1f/lerpDuration);
                currentRadiusLerp += Time.deltaTime * (endRadius - startRadius) * (1f/lerpDuration);
                //currentWeight += Time.deltaTime * (endWeight - startWeight) * (1f/lerpDuration);
                currentWeightLerp += Time.deltaTime * (endWeight - startWeight) * (1f/lerpDuration);
                
                
                _target.m_Targets[member].radius = Mathf.Lerp(startRadius, endRadius, currentRadiusLerp);
                _target.m_Targets[member].weight = Mathf.Lerp(startWeight, endWeight, currentWeightLerp);
                
                yield return endFrame;
            }

            currentRadiusLerp = 1;
            currentWeightLerp = 1;

            yield return new WaitForSeconds(1f);
            
            while (currentWeightLerp > 0 && currentRadiusLerp > 0)
            {
                currentRadiusLerp -= Time.deltaTime * (endRadius - startRadius) * (1f/lerpDuration);
                currentWeightLerp -= Time.deltaTime * (endWeight - startWeight) * (1f/lerpDuration);
                
                
                _target.m_Targets[member].radius = Mathf.Lerp(startRadius, endRadius, currentRadiusLerp);
                _target.m_Targets[member].weight = Mathf.Lerp(startWeight, endWeight, currentWeightLerp);
                
                yield return endFrame;
            }
            
            _target.RemoveMember(objToIncrease.transform);
            _routines.Remove(objToIncrease);

        }

        /*private IEnumerator DecreaseWeight(GameObject objToDecrease)
        {
            
        }*/
    }
}
