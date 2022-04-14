using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;


public class Action : MonoBehaviour
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
    UnityEvent<Action> onStart;
    [SerializeField]
    UnityEvent<Action> onEnd;

    [SerializeField]
    float lastEvaluation = 0f;


    // STATS
    public float TotalDurationDoingAction
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

    public virtual void StartAction()
    {
        onStart?.Invoke(this);
    }

    public virtual void UpdateAction()
    {
        TotalDurationDoingAction += Time.deltaTime;
    }

    public virtual void EndAction()
    {
        onEnd?.Invoke(this);
    }
}
