using UnityEngine;
using UnityEditor;
using DungeonMaker.Core;
using UEditor = UnityEditor.Editor;

namespace DungeonMaker.Editor
{
    [CustomEditor(typeof(RoomData))]
    public class RoomInspector : UEditor
    {
        private Rect iconRect;
        private Texture2D iconTex;
        //private Color iconCol;

        private void OnEnable()
        {
            iconRect.x = 10;
            iconRect.y = 12;
            iconRect.width = 40;
            iconRect.height = 40;

            iconTex = serializedObject.FindProperty("icon").objectReferenceValue as Texture2D;
            //iconCol = serializedObject.FindProperty("color").colorValue;
        }

        public override void OnInspectorGUI()
        {
            //iconTex.ApplyColor(iconCol);
            GUI.DrawTexture(iconRect, iconTex);
            //iconTex.ApplyColor(Color.white);

            EditorGUILayout.BeginVertical();

            GUILayout.Space(13);

            EditorGUILayout.BeginHorizontal();

            GUILayout.Space(50);

            if (GUILayout.Button("Open", GUILayout.MinHeight(30)))
            {
                RoomWindow.OpenWindow();

                if (RoomWindow.window != null)
                {
                    RoomWindow.window.Load(serializedObject.targetObject as RoomData);
                }
            }

            GUILayout.Space(10);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }
    }
}