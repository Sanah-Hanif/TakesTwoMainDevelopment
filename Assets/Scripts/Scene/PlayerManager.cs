using UnityEngine;

namespace Scene
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private GameObject harmony;
        [SerializeField] private GameObject chaos;

        public GameObject Harmony
        {
            get => harmony;
            private set => harmony = value;
        }

        public GameObject Chaos
        {
            get => chaos;
            private set => chaos = value;
        }
    }
}
