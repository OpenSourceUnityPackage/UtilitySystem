using UnityEngine;

namespace UtilitySystemPackage
{
    [System.Serializable]
    public class StatImportance
    {
        public string name;
        [SerializeField] public AnimationCurve curve;
        public int weight = 1;
    }
}