using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

namespace GGL
{
    [CustomEditor(typeof(GizmosGL), true)]
    public class GizmosGLEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            this.DrawDefaultInspectorWithoutScriptField();
        }
    }

    public static class DefaultInspector_EditorExtension
    {
        public static bool DrawDefaultInspectorWithoutScriptField(this Editor Inspector)
        {
            EditorGUI.BeginChangeCheck();

            Inspector.serializedObject.Update();

            SerializedProperty Iterator = Inspector.serializedObject.GetIterator();

            Iterator.NextVisible(true);

            while (Iterator.NextVisible(false))
            {
                EditorGUILayout.PropertyField(Iterator, true);
            }

            Inspector.serializedObject.ApplyModifiedProperties();

            return (EditorGUI.EndChangeCheck());
        }
    }
    [InitializeOnLoad]
    public class ExecutionOrderManager : Editor
    {
        static ExecutionOrderManager()
        {
            // Get the name of the script we want to change it's execution order
            string scriptName = typeof(GizmosGL).Name;

            int maxExecutionOrder = GetMaxExecutionOrder(scriptName);
            int executionOrder = maxExecutionOrder - 100;

            // Iterate through all scripts (Might be a better way to do this?)
            foreach (MonoScript monoScript in MonoImporter.GetAllRuntimeMonoScripts())
            {
                // If found our script
                if (monoScript.name == scriptName)
                {
                    // And it's not at the execution time we want already
                    // (Without this we will get stuck in an infinite loop)
                    if (MonoImporter.GetExecutionOrder(monoScript) != executionOrder)
                    {
                        MonoImporter.SetExecutionOrder(monoScript, executionOrder);
                    }
                    break;
                }
            }
        }

        static int GetMaxExecutionOrder(string excludeScript)
        {
            int maxExecutionOrder = int.MinValue;
            MonoScript[] monos = MonoImporter.GetAllRuntimeMonoScripts();
            foreach (MonoScript mono in monos)
            {
                int executionOrder = MonoImporter.GetExecutionOrder(mono);
                if (mono.name != excludeScript && maxExecutionOrder < executionOrder)
                {
                    maxExecutionOrder = executionOrder;
                }
            }
            return maxExecutionOrder;
        }
    }
}