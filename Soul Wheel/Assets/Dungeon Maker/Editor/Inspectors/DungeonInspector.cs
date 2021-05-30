using UnityEngine;
using UnityEditor;
using DungeonMaker.Core;
using UEditor = UnityEditor.Editor;

namespace DungeonMaker.Editor
{
    [CustomEditor(typeof(DungeonData))]
    public class DungeonInspector : UEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical();

            GUILayout.Space(13);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Open", GUILayout.MinHeight(30)))
            {
                DungeonWindow.OpenWindow();

                if (DungeonWindow.window != null)
                {
                    DungeonWindow.window.Load(serializedObject.targetObject as DungeonData);
                }
            }

            GUILayout.Space(10);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }
    }
}