using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace UtilitySystem
{

    public class Desire : MonoBehaviour
    {
        [System.Serializable]
        struct StatImportance
        {
            public string statName;

            [SerializeField]
            public AnimationCurve curve; // must return a value between 0 and 1.
        }

        [SerializeField]
        float defaultImportance = 0f;

        [SerializeField]
        public int priority = 0;

        [SerializeField]
        List<StatImportance> statsImportance = new List<StatImportance>();

        [SerializeField]
        UnityEvent<Desire> onStart;
        [SerializeField]
        UnityEvent<Desire> onEnd;

        [SerializeField]
        float lastEvaluation = 0f;


        // STATS
        public float TotalDurationDoingDesire
        {
            get;
            private set;
        }



        public float EvaluateUtility(Blackboard bb)
        {
            float totalImportance = defaultImportance;

            foreach (StatImportance statImportance in statsImportance)
            {
                Stat stat = bb.stats.Find((Stat s) => { return s.name == statImportance.statName; });
                if (stat != null)
                    totalImportance += statImportance.curve.Evaluate(stat.Ratio);
                else
                {
                    Debug.LogWarning("Stat not found : " + statImportance.statName);
                }
            }

            lastEvaluation = Mathf.Clamp(totalImportance, 0, 1);

            return lastEvaluation;
        }

        public virtual void StartDesire()
        {
            onStart?.Invoke(this);
        }

        public virtual void UpdateDesire()
        {
            TotalDurationDoingDesire += Time.deltaTime;
        }

        public virtual void EndDesire()
        {
            onEnd?.Invoke(this);
        }
    }
}