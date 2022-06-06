using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UtilitySystemPackage;

public class UtilitySystems : MonoBehaviour
{
    [SerializeField] public UtilitySystem logicUtilitySystem;
    public UtilitySystemData logicData;
    [SerializeField] public UtilitySystem personalityUtilitySystem;
    public UtilitySystemData personalityData;

    // Start is called before the first frame update
    void Start()
    {
        logicUtilitySystem.Init(logicData);
        personalityUtilitySystem.Init(personalityData);
    }

    // Update is called once per frame
    void Update()
    {
        logicUtilitySystem.Update();
        var utilities = logicUtilitySystem.GetUtilitiesAsFloat();

        personalityUtilitySystem.UpdateStats(utilities, true);
        personalityUtilitySystem.Update();
    }
#if UNITY_EDITOR
    private void OnGUI()
    {
        GUILayout.BeginVertical();
        DisplayUtilitySystem(logicUtilitySystem, "logic");
        DisplayUtilitySystem(personalityUtilitySystem, "personality");
        GUILayout.EndVertical();
    }

    private void DisplayUtilitySystem(UtilitySystem system, string name)
    {
        GUILayout.Label(name);
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical("box");
        foreach (var stat in system.GetInputs().Values)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(stat.Name);
            stat.Value = GUILayout.HorizontalSlider(stat.Value, 0f, 1f, GUILayout.Width(100f));
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();

        GUILayout.BeginVertical("box");
        foreach (var utility in system.GetUtilities().Values)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(utility.Name);
            GUILayout.HorizontalSlider(utility.Value, 0f, 1f, GUILayout.Width(100f));
            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }
#endif
}