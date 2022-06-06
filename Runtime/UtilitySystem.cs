using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UtilitySystemPackage
{
    [System.Serializable]
    public class UtilitySystem
    {
        [SerializeField] private Dictionary<string, Stat> _inputs = new Dictionary<string, Stat>();

        [SerializeField] private Dictionary<string, Utility> _outputs = new Dictionary<string, Utility>();

        public void Init(UtilitySystemData systemData)
        {
            _inputs = new Dictionary<string, Stat>();
            for (int i = 0; i < systemData.inputs.Count; i++)
            {
                _inputs.Add(systemData.inputs[i],new Stat(systemData.inputs[i], 0f));
            }

            _outputs = new Dictionary<string, Utility>();
            for (int i = 0; i < systemData.utilities.Count; i++)
            {
                _outputs.Add(systemData.utilities[i].Name, systemData.utilities[i]);
            }
        }

        public void UpdateStats(Dictionary<string, float> inputs, bool allowPartialModifications = false)
        {
            foreach (Stat input in _inputs.Values)
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

        public void SetStat(string key, float value)
        {
            if (_inputs.ContainsKey(key))
            {
                _inputs[key].Value = value;
            }
            else
            {
                Debug.LogWarning("Couldn't find input " + key + " in given inputs");
            }
        }

        public void Update()
        {
            foreach (var output in _outputs.Values)
            {
                output.EvaluateUtility(new List<Stat>(_inputs.Values));
            }
        }

        public Utility GetHighestUtility()
        {
            if (_outputs.Count == 0) return null;

            Utility max = null;

            foreach (var utility in _outputs.Values)
            {
                if (max == null)
                    max = utility;
                else if (utility.Value > max.Value)
                    max = utility;
            }

            return max;
        }

        public Utility GetUtility(string key)
        {
            if (_outputs.ContainsKey(key))
                return _outputs[key];
            else
            {
                Debug.LogWarning($"UtilitySystem does not contain the utility {key}");
                return null;
            }
        }

        public Dictionary<string, Utility> GetUtilities()
        {
            return _outputs;
        }

        public Dictionary<string, float> GetUtilitiesAsFloat()
        {
            Dictionary<string, float> utilities = new Dictionary<string, float>();
            foreach (KeyValuePair<string,Utility> kvp in _outputs)
            {
                utilities.Add(kvp.Key, kvp.Value.Value);
            }

            return utilities;
        }

        public List<Utility> GetUtilitiesSorted()
        {
            List<Utility> retVal = new List<Utility>(_outputs.Values);
            retVal.Sort((lhs, rhs) => rhs.Value.CompareTo(lhs.Value));
            return retVal;
        }

        public Dictionary<string, Stat> GetInputs()
        {
            return _inputs;
        }
        
        public Stat GetStat(string key)
        {
            if (_inputs.ContainsKey(key))
                return _inputs[key];
            else
            {
                Debug.LogWarning($"UtilitySystem does not contain the stat {key}");
                return null;
            }
        }

    }
}