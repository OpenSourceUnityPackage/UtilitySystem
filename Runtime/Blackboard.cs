using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilitySystem
{
    public class Blackboard : MonoBehaviour
    {
        [SerializeField]
        public List<Stat> stats = new List<Stat>();
    }
}