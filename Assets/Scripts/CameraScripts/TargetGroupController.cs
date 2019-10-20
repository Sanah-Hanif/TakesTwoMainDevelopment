using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using ScriptableObjects.Camera;
using UnityEngine;

namespace CameraScripts
{
    public class TargetGroupController : MonoBehaviour
    {
        [HideInInspector]
        public bool foldout = true;
        
        public TargetSettings settings;
        private CinemachineTargetGroup _target;
        private Dictionary<GameObject, IEnumerator> _routines = new Dictionary<GameObject, IEnumerator>();

        private void Awake()
        {
            _target = GetComponent<CinemachineTargetGroup>();
        }

        private void OnValidate()
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

        private IEnumerator IncreaseWeight(GameObject objToIncrease)
        {
            var currentWeight = settings.startWeight;
            var currentWeightLerp = 0f;
            var currentRadius = settings.startRadius;
            var currentRadiusLerp = 0f;
            _target.AddMember(objToIncrease.transform, currentWeight, currentRadius);
            var member = _target.FindMember(objToIncrease.transform);
            
            var endFrame = new WaitForEndOfFrame();
            
            yield return endFrame;
            
            while (currentRadiusLerp < 1 && currentWeightLerp < 1)
            {
                //currentRadius += Time.deltaTime * (endRadius - startRadius) * (1f/lerpDuration);
                currentRadiusLerp += Time.deltaTime * (1f/settings.lerpDuration);
                //currentWeight += Time.deltaTime * (endWeight - startWeight) * (1f/lerpDuration);
                currentWeightLerp += Time.deltaTime * (1f/settings.lerpDuration);
                
                if(_target.FindMember(objToIncrease.transform) == -1) break;
                _target.m_Targets[member].radius = Mathf.Lerp(settings.startRadius, settings.endRadius, currentRadiusLerp);
                _target.m_Targets[member].weight = Mathf.Lerp(settings.startWeight, settings.endWeight, currentWeightLerp);
                
                yield return endFrame;
            }

            currentRadiusLerp = 1;
            currentWeightLerp = 1;

            yield return new WaitForSeconds(1f);
            
            while (currentWeightLerp > 0 && currentRadiusLerp > 0)
            {
                currentRadiusLerp -= Time.deltaTime * (1f/settings.lerpDuration);
                currentWeightLerp -= Time.deltaTime * (1f/settings.lerpDuration);
                
                
                _target.m_Targets[member].radius = Mathf.Lerp(settings.startRadius, settings.endRadius, currentRadiusLerp);
                _target.m_Targets[member].weight = Mathf.Lerp(settings.startWeight, settings.endWeight, currentWeightLerp);
                
                yield return endFrame;
            }

            if (objToIncrease == null) yield break;
            _target.RemoveMember(objToIncrease.transform);
            _routines.Remove(objToIncrease);
        }
    }
}
