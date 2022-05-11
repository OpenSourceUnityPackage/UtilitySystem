using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UtilitySystem.Runtime
{
    [System.Serializable]
    public class UtilitySystem
    {
        [SerializeField] private List<Stat> _inputs = new List<Stat>();

        [SerializeField] private List<Utility> _outputs = new List<Utility>();

        public void Init(UtilitySystemData systemData)
        {
            _inputs = new List<Stat>();
            for (int i = 0; i < systemData.inputs.Count; i++)
            {
                _inputs.Add(new Stat(systemData.inputs[i], 0f));
            }

            _outputs = systemData.utilities;
        }

        public void UpdateStats(Dictionary<string, float> inputs, bool allowPartialModifications = false)
        {
            foreach (Stat input in _inputs)
            {
                if (inputs.ContainsKey(input.Name))
                {
                    input.Value = inputs[input.Name];
                }
                else if (!allowPartialModifications)
                {
                    Debug.LogWarning("Couldn't find input " + input.Name + " in given inputs");
                }
            }
        }

        public void Update()
        {
            foreach (var output in _outputs)
            {
                output.EvaluateUtility(_inputs);
            }
        }

        public Utility GetHighestUtility()
        {
            if (_outputs.Count == 0) return null;

            Utility max = _outputs[0];
            for (int i = 1; i < _outputs.Count; i++)
            {
                if (_outputs[i].Value > max.Value)
                    max = _outputs[i];
            }

            return max;
        }

        public List<Utility> GetUtilities()
        {
            List<Utility> retVal = new List<Utility>(_outputs);
            return retVal;
        }

        public List<Utility> GetUtilitiesSorted()
        {
            List<Utility> retVal = new List<Utility>(_outputs);
            retVal.Sort((lhs, rhs) => rhs.Value.CompareTo(lhs.Value));
            return retVal;
        }

        public List<Stat> GetInputs()
        {
            return _inputs;
        }
    }
}