using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UtilitySystemPackage
{
    [System.Serializable]
    public class Stat
    {
        public string Name;

        [SerializeField] 
        [Range(0f, 1f)]
        private float value;

        public float Value
        {
            get { return value; }
            set { this.value = Mathf.Clamp(value, 0, 1); }
        }

        public Stat(string name, float value)
        {
            Name = name;
            Value = value;
        }

        public Stat(Utility utility)
        {
            Name = utility.Name;
            Value = utility.Value;
        }
    };
}