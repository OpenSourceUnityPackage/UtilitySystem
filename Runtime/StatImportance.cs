using UnityEngine;

namespace UtilitySystem.Runtime
{
    [System.Serializable]
    public class StatImportance
    {
        public string name;
        [SerializeField] public AnimationCurve curve;
        public int weight = 1;
    }
}