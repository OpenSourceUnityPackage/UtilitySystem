using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UtilitySystemPackage
{
    [System.Serializable]
    public class Utility
    {
        public string Name;
        
        [SerializeField]
        private float value;

        public float Value
        {
            get { return value; }
            set { this.value = Mathf.Clamp(value, 0, 1); }
        }

        [SerializeField]
        public List<StatImportance> statImportances;

        public float EvaluateUtility(List<Stat> inputs, float defaultImportance = 0f)
        {
            float totalImportance = defaultImportance;

            foreach (StatImportance statImportance in statImportances)
            {
                Stat stat = inputs.Find((Stat s) => s.Name == statImportance.name);
                if (stat != null)
                    totalImportance += statImportance.curve.Evaluate(stat.Value) * statImportance.weight;
                else
                {
                    Debug.LogWarning("Stat not found in inputs : " + statImportance.name);
                }
            }

            Value = totalImportance / statImportances.Sum(importance => importance.weight);
            return Value;
        }
    }
}