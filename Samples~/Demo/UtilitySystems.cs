using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UtilitySystem.Runtime;

public class UtilitySystems : MonoBehaviour
{
    [SerializeField] public UtilitySystem.Runtime.UtilitySystem logicUtilitySystem;
    public UtilitySystemData logicData;
    [SerializeField] public UtilitySystem.Runtime.UtilitySystem personalityUtilitySystem;
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
        var utilities = logicUtilitySystem.GetUtilities();
        
        var dict = new Dictionary<string, float>();
        foreach (var utility in utilities)
        {
            dict[utility.Name] = utility.Value;
        }
        
        personalityUtilitySystem.UpdateStats(dict, true);
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

    private void DisplayUtilitySystem(UtilitySystem.Runtime.UtilitySystem system, string name)
    {
        GUILayout.Label(name);
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical("box");
        foreach (var stat in system.GetInputs())
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(stat.Name);
            stat.Value = GUILayout.HorizontalSlider(stat.Value, 0f, 1f, GUILayout.Width(100f));
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();

        GUILayout.BeginVertical("box");
        foreach (var utility in system.GetUtilities())
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