using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using UnityEditor;
using UnityEngine;
using UtilitySystemPackage;

namespace UtilitySystem.Editor
{
    [CustomEditor(typeof(UtilitySystemData))]
    public class UtilitySystemDataEditor : UnityEditor.Editor
    {
        private static UtilitySystemData _utilitySystemData;
        private static SerializedProperty InputsProperty;
        private static SerializedProperty OutputsProperty;

        private void OnEnable()
        {
            _utilitySystemData = target as UtilitySystemData;
            InputsProperty = serializedObject.FindProperty("inputs");
            OutputsProperty = serializedObject.FindProperty("utilities");
        }

        public override void OnInspectorGUI()
        {
            // base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(InputsProperty);

            ShowOutputs();

            serializedObject.ApplyModifiedProperties();
        }

        private void ShowOutputs()
        {
            EditorGUILayout.BeginHorizontal();
            OutputsProperty.isExpanded =
                EditorGUILayout.Foldout(OutputsProperty.isExpanded, OutputsProperty.displayName);
            
            // TODO: Use default constructor
            if (FitButton("Add Utility"))
            {
                OutputsProperty.InsertArrayElementAtIndex(OutputsProperty.arraySize);
                OutputsProperty.GetArrayElementAtIndex(OutputsProperty.arraySize - 1).isExpanded = true;
            }

            EditorGUILayout.EndHorizontal();
            if (OutputsProperty.isExpanded)
            {
                EditorGUI.indentLevel += 1;
                for (int i = 0; i < OutputsProperty.arraySize; i++)
                {
                    SerializedProperty outputProperty = OutputsProperty.GetArrayElementAtIndex(i);
                    SerializedProperty statProperties = outputProperty.FindPropertyRelative("statImportances");

                    EditorGUILayout.BeginHorizontal();

                    outputProperty.isExpanded =
                        EditorGUILayout.Foldout(outputProperty.isExpanded, outputProperty.displayName);
                    if (FitButton("Remove Utility"))
                    {
                        OutputsProperty.DeleteArrayElementAtIndex(i);
                        break;
                    }

                    // TODO: Use default constructor
                    if (statProperties.arraySize < InputsProperty.arraySize && FitButton("Add Stat Importance") )
                    {
                        outputProperty.isExpanded = true;
                        statProperties.InsertArrayElementAtIndex(statProperties.arraySize);
                        statProperties.isExpanded = true;
                        statProperties.GetArrayElementAtIndex(statProperties.arraySize - 1).isExpanded = true;
                    }

                    EditorGUILayout.EndHorizontal();

                    if (outputProperty.isExpanded)
                    {
                        EditorGUI.indentLevel += 1;

                        EditorGUILayout.PropertyField(outputProperty.FindPropertyRelative("Name"));

                        EditorGUILayout.BeginHorizontal();

                        statProperties.isExpanded =
                            EditorGUILayout.Foldout(statProperties.isExpanded, statProperties.displayName);

                        EditorGUILayout.EndHorizontal();

                        if (statProperties.isExpanded)
                        {
                            EditorGUI.indentLevel += 1;

                            for (int j = 0; j < statProperties.arraySize; j++)
                            {
                                SerializedProperty statProperty = statProperties.GetArrayElementAtIndex(j);

                                EditorGUILayout.BeginHorizontal();
                                
                                statProperty.isExpanded =
                                    EditorGUILayout.Foldout(statProperty.isExpanded, statProperty.displayName);

                                if (FitButton("Remove Stat Importance"))
                                {
                                    statProperties.DeleteArrayElementAtIndex(j);
                                    break;
                                }

                                EditorGUILayout.EndHorizontal();

                                if (statProperty.isExpanded)
                                {
                                    SerializedProperty statName = statProperty.FindPropertyRelative("name");

                                    List<string> selectable = _utilitySystemData.inputs.Where(s =>
                                    {
                                        for (int k = 0; k < statProperties.arraySize; k++)
                                        {
                                            if (statProperties.GetArrayElementAtIndex(k)
                                                    .FindPropertyRelative("name").stringValue == s && k != j)
                                            {
                                                return false;
                                            }
                                        }

                                        return true;
                                    }).ToList();

                                    int current = selectable.FindIndex(value =>
                                        statName.stringValue == value);
                                    
                                    int selected = EditorGUILayout.Popup(current >= 0 ? current : 0, selectable.ToArray());

                                    if (current != selected)
                                    {
                                        statProperty.FindPropertyRelative("name").stringValue =
                                            selected >= 0 ? selectable[selected] : "";
                                    }

                                    EditorGUILayout.PropertyField(statProperty.FindPropertyRelative("curve"),
                                        new GUIContent("Importance Curve"));

                                    SerializedProperty statWeight = statProperty.FindPropertyRelative("weight");

                                    statWeight.intValue = EditorGUILayout.IntSlider(statWeight.displayName, statWeight.intValue, 0, 10);
                                }
                            }

                            EditorGUI.indentLevel -= 1;
                        }

                        EditorGUI.indentLevel -= 1;
                    }
                }

                EditorGUI.indentLevel -= 1;
            }
        }

        private static Vector2 ComputeTextWidth(String text)
        {
            return GUI.skin.button.CalcSize(new GUIContent(text));
        }

        private static bool FitButton(string text)
        {
            return GUILayout.Button(text, EditorStyles.miniButton, GUILayout.Width(ComputeTextWidth(text).x));
        }
    }
}