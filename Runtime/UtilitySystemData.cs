using System.Collections.Generic;
using UnityEngine;

namespace UtilitySystemPackage
{
    [CreateAssetMenu(fileName = "UtilitySystemData", menuName = "UtilitySystem/UtilitySystemData", order = 0)]
    public class UtilitySystemData : ScriptableObject
    {
        public List<Utility> utilities;
        public List<string> inputs;
    }
}