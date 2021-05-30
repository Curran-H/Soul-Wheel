using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using DungeonMaker.Core;

namespace DungeonMaker.Editor
{
    [Serializable]
    public class DungeonPropertyView : ViewBase
    {
        #region Const Variables
        private const string DUNGEON_EDITOR_TITLE = "DUNGEON  MAKER";
        private static Color DUNGEON_EDITOR_TITLE_COLOR = EditorColors.COLOR_YELLOW;
        private const KeyCode SELECTION_KEY = KeyCode.LeftControl;
        #endregion Const Variables


        #region Private Variables
        private Rect headerArea;
        private Color headerColor;
        private Texture2D headerTex;

        private Rect bodyArea;
        private Color bodyColor;
        private Texture2D bodyTex;

        private Rect optionsArea;
        private Rect nameArea;
        private Rect endArea;

        private Rect logoRect;

        private bool ok;
        #endregion Private Variables


        #region Constructors
        public DungeonPropertyView() : base("Property View") { }
        #endregion Constructors


        #region Main Methods
        public override void UpdateView(Rect editorRect, Rect percentageRect, Event e, DungeonData dungeon, DungeonWindow window)
        {
            base.UpdateView(editorRect, percentageRect, e, dungeon, window);

            DrawLayouts();
            DrawHeader();
            DrawBody();

            ProcessEvents(e);
        }

        private void DrawLayouts()
        {
            headerColor = EditorColors.MAIN1_COLOR;
            headerTex = new Texture2D(1, 1);
            headerTex.SetPixel(0, 0, headerColor);
            headerTex.Apply();

            bodyColor = EditorColors.MAIN2_COLOR;
            bodyTex = new Texture2D(1, 1);
            bodyTex.SetPixel(0, 0, bodyColor);
            bodyTex.Apply();

            headerArea.x = viewArea.x;
            headerArea.y = 0;
            headerArea.width = viewArea.width;
            headerArea.height = 50;

            bodyArea.x = viewArea.x;
            bodyArea.y = headerArea.height;
            bodyArea.width = viewArea.width;
            bodyArea.height = viewArea.height;

            optionsArea.x = 0;
            optionsArea.y = viewArea.height - headerArea.height * 2 - 30;
            optionsArea.width = viewArea.width;
            optionsArea.height = viewArea.height - headerArea.height;

            nameArea.x = 0;
            nameArea.y = 0;
            nameArea.width = viewArea.width;
            nameArea.height = 35;

            endArea.x = 0;
            endArea.y = bodyArea.height - headerArea.height - (ok ? 40 : 50);
            endArea.width = viewArea.width;
            endArea.height = 50;

            logoRect.x = viewArea.width / 2 - 40;
            logoRect.y = 10;
            logoRect.width = 80;
            logoRect.height = 80;

            GUI.DrawTexture(headerArea, headerTex);
            GUI.DrawTexture(bodyArea, bodyTex);
        }

        private void DrawHeader()
        {
            GUILayout.BeginArea(headerArea);

            if (dungeon == null)
            {
                GUI.color = DUNGEON_EDITOR_TITLE_COLOR;
                GUILayout.Label(DUNGEON_EDITOR_TITLE, skin.GetStyle("Header"));
                GUI.color = Color.white;
            }
            else
            {
                GUILayout.Label(dungeon.Name, skin.GetStyle("Header"));
            }

            GUILayout.EndArea();
        }

        private void DrawBody()
        {
            GUILayout.BeginArea(bodyArea);

            if (dungeon == null)
            {
                DrawWindowProperties();
            }
            else
            {
                DrawDungeonProperties();
                DrawNodeProperties();
            }

            GUILayout.EndArea();
        }

        private void DrawWindowProperties()
        {
            GUILayout.BeginArea(optionsArea);

            GUILayout.BeginHorizontal();
            GUILayout.Space(10);

            GUILayout.BeginVertical();

            // CREATE
            GUI.backgroundColor = EditorColors.COLOR_GREEN;
            if (GUILayout.Button(new GUIContent("Create"), GUILayout.Height(30)))
            {
                if (window != null)
                {
                    EditorUtils.CreateNewGraph("New Dungeon");
                }
            }
            GUI.backgroundColor = Color.white;

            GUILayout.Space(5);

            // LOAD
            GUI.backgroundColor = EditorColors.COLOR_BLUE;
            if (GUILayout.Button(new GUIContent("Load"), GUILayout.Height(30)))
            {
                EditorUtils.LoadGraph();
            }
            GUI.backgroundColor = Color.white;

            GUILayout.EndVertical();

            GUILayout.Space(10);
            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        private void DrawDungeonProperties()
        {
            #region Name
            //GUILayout.BeginArea(nameArea);

            //Rect nameLabelRect = new Rect(-viewArea.width / 2 + 40, 0, viewArea.width, 30);
            //GUI.Label(nameLabelRect, new GUIContent("Name", "Dungeon Name"), skin.GetStyle("Body"));

            //GUILayout.Space(10);

            //GUILayout.BeginHorizontal();
            //GUILayout.Space(120);

            //dungeon.dungeonName = GUILayout.TextField(dungeon.dungeonName, 13);

            //GUILayout.Space(12);
            //GUILayout.EndHorizontal();

            //GUILayout.EndArea();
            #endregion Name


            #region End
            GUILayout.BeginArea(endArea);

            GUILayout.BeginHorizontal();
            GUILayout.Space(10);

            ok = true;
            if (string.IsNullOrEmpty(dungeon.Name))
            {
                EditorGUILayout.HelpBox("Invalid name.", MessageType.Warning);
                ok = false;
            }

            if (ok)
            {
                // SAVE
                GUI.backgroundColor = EditorColors.COLOR_GREEN;
                if (GUILayout.Button(new GUIContent("Save"), GUILayout.Height(30)))
                {
                    if (window != null)
                    {
                        window.Save();
                    }
                }
                GUI.backgroundColor = Color.white;

                GUILayout.Space(10);

                // CANCEL
                GUI.backgroundColor = EditorColors.COLOR_RED;
                if (GUILayout.Button(new GUIContent("Cancel"), GUILayout.Height(30), GUILayout.Width(viewArea.width * 0.3f)))
                {
                    if (window != null)
                    {
                        EditorUtils.UnloadGraph();
                    }
                }
                GUI.backgroundColor = Color.white;
            }

            GUILayout.Space(10);
            GUILayout.EndHorizontal();

            GUILayout.EndArea();
            #endregion
        }

        private void DrawNodeProperties()
        {
            if (dungeon.selectedNodes.Count == 1)
            {
                RoomData room = dungeon.selectedNodes[0].room;

                GUI.DrawTexture(logoRect, room.icon);

                logoRect.y += 55;

                GUI.Label(logoRect, room.Name, skin.GetStyle("Header"));
            }
        }

        protected override void ProcessEvents(Event e)
        {
            base.ProcessEvents(e);

            // SELECTION KEY
            if (dungeon != null)
            {
                if (Event.current.keyCode == SELECTION_KEY)
                {
                    dungeon.wantSelect = true;
                }

                if (e.type == EventType.KeyUp)
                {
                    if (Event.current.keyCode == SELECTION_KEY)
                    {
                        dungeon.wantSelect = false;
                    }
                }
            }
        }
        #endregion Main Methods
    }
}