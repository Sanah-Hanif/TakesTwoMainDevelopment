using System.Collections.Generic;
using Interaction.Level_Elements;
using Interaction.Level_Elements.Toggle;
using UnityEngine;

namespace Nodule
{
    public class NoduleController : MonoBehaviour
    {
        private Dictionary<EnvironmentInteraction, GameObject> nodules = new Dictionary<EnvironmentInteraction, GameObject>();

        [SerializeField] protected Transform spawnPosition;
        
        public void ClearNodules()
        {
            nodules.Clear();
            var nods = spawnPosition.childCount;
            GameObject[] children = new GameObject[nods];
            for (int i = 0; i < nods; i++)
            {
                children[i] = spawnPosition.GetChild(i).gameObject;
            }

            for (int i = 0; i < nods; i++)
            {
                DestroyImmediate(children[i]);
            }
        }

        public void AddNodule(EnvironmentInteraction env, GameObject nod)
        {
            nodules.Add(env, nod);
            var pos = spawnPosition.position;
            Vector3 startingPos = pos + (nodules.Count-1) * -0.5f * Vector3.up;
            foreach (var obj in nodules)
            {
                obj.Value.transform.rotation = spawnPosition.rotation;
                obj.Value.transform.parent = spawnPosition;
                obj.Value.transform.position = startingPos;
                startingPos += Vector3.up;
            }
        }
    }
}
