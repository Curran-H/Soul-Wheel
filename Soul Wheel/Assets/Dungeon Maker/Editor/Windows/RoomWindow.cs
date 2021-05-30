using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using DungeonMaker.Core;

namespace DungeonMaker.Editor
{
    public class RoomWindow : BaseRoomWindow
    {
        #region Private Variables
        private string assetPath;
        private bool ok;

        private Rect headerIconRect;
        private Rect headerTitleRect;

        private Rect menuRect;

        private Rect roomIconRect;
        private Rect roomsRect;
        private Rect endRect;

        private Vector2 scroll;
        private bool[] foldouts;
        private int pieceY;
		#endregion Private Variables


		#region Base Methods
		protected override void OnEnable()
		{
			base.OnEnable();
            foldouts = new bool[16];
        }

        protected override void DrawLayouts()
        {
            base.DrawLayouts();

            float headerIconSize = 40f;
            headerIconRect = NewRect(6f, 6f, headerIconSize, headerIconSize);
            headerTitleRect = NewRect(headerRect.width / 2f, 0f, 0f, headerRect.height);

            float menuOffset = 25f;
            menuRect = NewRect(0f, windowRect.height - headerRect.height * 2 - menuOffset, bodyRect.width, headerRect.height + menuOffset);

            roomIconRect = NewRect(0f, 0f, bodyRect.width, 35f);
            roomsRect = NewRect(0f, roomIconRect.y + roomIconRect.height + 2f, bodyRect.width, bodyRect.height - menuRect.height);

            float endHeight = 40f;
            endRect = NewRect(0f, bodyRect.height - endHeight, bodyRect.width, endHeight);
        }
        protected override void DrawHeader()
        {
            GUILayout.BeginArea(headerRect);
                if (room == null)
                {
                    GUI.color = ROOM_EDITOR_TITLE_COLOR;
                    GUI.Label(headerTitleRect, ROOM_EDITOR_TITLE, skin.GetStyle("Header"));
                    GUI.color = Color.white;
                }
                else
                {
                    GUI.Label(headerTitleRect, room.Name, skin.GetStyle("Header"));

                    if (room.icon != null && room.icon.isReadable)
                        GUI.DrawTexture(headerIconRect, room.icon);
                }
            GUILayout.EndArea();
        }
        protected override void DrawBody()
        {
            GUILayout.BeginArea(bodyRect);
                if (room == null) DrawMenu();
                else DrawRoom();
            GUILayout.EndArea();
        }
        #endregion Base Methods


        #region Main Methods
        private void DrawMenu()
        {
            GUILayout.BeginArea(menuRect);
                GUILayout.BeginHorizontal();
                    GUILayout.Space(10);
                    GUILayout.BeginVertical();

                        // CREATE
                        GUI.backgroundColor = EditorColors.COLOR_GREEN;
                        if (GUILayout.Button(new GUIContent("Create"), GUILayout.Height(30)))
                        CreateData(RoomData.DEFAULT_ROOM_NAME);
                        GUI.backgroundColor = Color.white;

                        GUILayout.Space(5);

                        // LOAD
                        GUI.backgroundColor = EditorColors.COLOR_BLUE;
                        if (GUILayout.Button(new GUIContent("Load"), GUILayout.Height(30)))
                        LoadData();
                        GUI.backgroundColor = Color.white;

                    GUILayout.EndVertical();
                    GUILayout.Space(10);
                GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void DrawRoom()
        {
            #region Icon
            GUILayout.BeginArea(roomIconRect);
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                    GUILayout.Space(10);

                    GUILayout.Label(new GUIContent("Icon", "Represents the room in the node editor."), skin.GetStyle("Body"));
                    GUILayout.Space(56);

                    GUILayout.BeginVertical();
                        GUILayout.Space(5);
                        room.icon = EditorGUILayout.ObjectField(room.icon, typeof(Texture2D), false) as Texture2D;
                    GUILayout.EndVertical();

                    GUILayout.Space(10);
                GUILayout.EndHorizontal();
            GUILayout.EndArea();
            #endregion Icon


            #region Rooms
            GUILayout.BeginArea(roomsRect);

                GUILayout.BeginHorizontal();
                    GUILayout.Space(10f);

                    GUILayout.Label(new GUIContent("Rooms", "T - Top\nB - Bottom\nL - Left\nR - Right"), skin.GetStyle("Body"));

                    Rect openButtonRect = NewRect(114f, 0f, 50f, 16f);
                    Rect closeButtonRect = openButtonRect;
                    closeButtonRect.x += openButtonRect.width + 11f;
                    Rect resetButtonRect = closeButtonRect;
                    resetButtonRect.x += closeButtonRect.width + 11f;

                    // OPEN
                    GUI.backgroundColor = EditorColors.COLOR_GREEN;
                    if (GUI.Button(openButtonRect, new GUIContent("Open")))
                        for (int i = 0; i < foldouts.Length; i++) foldouts[i] = true;
                    GUI.backgroundColor = Color.white;

                    // CLOSE
                    GUI.backgroundColor = EditorColors.COLOR_BLUE;
                    if (GUI.Button(closeButtonRect, new GUIContent("Close")))
                        for (int i = 0; i < foldouts.Length; i++) foldouts[i] = false;
                    GUI.backgroundColor = Color.white;

                    // RESET
                    GUI.backgroundColor = EditorColors.COLOR_RED;
                    if (GUI.Button(resetButtonRect, new GUIContent("Reset"))) Reset();
                    GUI.backgroundColor = Color.white;
                GUILayout.EndHorizontal();

                var scrollWidth = GUILayout.Width(roomsRect.width + 1f);
                var scrollHeight = GUILayout.Height(roomsRect.height - endRect.height + 10f);
                scroll = EditorGUILayout.BeginScrollView(scroll, scrollWidth, scrollHeight);
                    pieceY = 0;
                    for (int i = 0; i <= 15; i++) DrawPiece(i);
                    GUILayout.Space(888f + (pieceY * 21f) + 55f);
                    GUILayout.Label("");
                EditorGUILayout.EndScrollView();

            GUILayout.EndArea();
            #endregion Rooms


            #region End
            GUILayout.BeginArea(endRect);
                GUILayout.BeginHorizontal();
                    GUILayout.Space(10f);

                    ok = true;
                    if (string.IsNullOrEmpty(room.Name))
                    {
                        ok = false;
                        EditorGUILayout.HelpBox("Invalid name.", MessageType.Warning);
                    }
                    else if (room.icon == null || !room.icon.isReadable)
                    {
                        ok = false;
                        EditorGUILayout.HelpBox("Invalid icon.", MessageType.Warning);
                    }

                    if (ok)
                    {
                        // SAVE
                        GUI.backgroundColor = EditorColors.COLOR_GREEN;
                        if (GUILayout.Button(new GUIContent("Save"), GUILayout.Height(30f))) Save();
                        GUI.backgroundColor = Color.white;

                        GUILayout.Space(10f);

                        // CANCEL
                        GUI.backgroundColor = EditorColors.COLOR_RED;
                        if (GUILayout.Button(new GUIContent("Cancel"), GUILayout.Height(30f), GUILayout.Width(bodyRect.width * 0.3f)))
                            if (window != null) UnloadData();
                        GUI.backgroundColor = Color.white;
                    }

                    GUILayout.Space(10f);
                GUILayout.EndHorizontal();
            GUILayout.EndArea();
            #endregion End
        }

        private void DrawPiece(int ID)
        {
            #region Icon
            string pieceName = GetPieceName(ID);
            float pieceSize = 50f;
            Vector2 piecePos = new Vector2(13f, pieceY * 21f);
            Rect pieceRect = NewRect(piecePos.x, piecePos.y + (ID * (pieceSize + 10f)) + 5f, pieceSize, pieceSize);
            Texture2D pieceTex = Resources.Load<Texture2D>("Pictograms/" + pieceName);
            GUI.DrawTexture(pieceRect, pieceTex);
            #endregion Icon


            #region Property
            SerializedObject data = new SerializedObject(room);
            data.ApplyModifiedProperties();
            data.Update();

            SerializedProperty property = data.FindProperty("rooms").FindPropertyRelative(pieceName);
            if (property.isExpanded) pieceY += property.arraySize;

            Rect propertyRect = NewRect(pieceRect.x + pieceSize + 3f, pieceRect.y + (pieceRect.height / 2f) - 7f, roomsRect.width - 100f, roomsRect.height);
            Rect labelRect = propertyRect;
            labelRect.x = pieceRect.x + pieceRect.width + 20f;
            labelRect.y = pieceRect.y + ((pieceRect.height / 2f) - (propertyRect.height / 2f)) - 2f;
            Rect foldoutRect = propertyRect;
            foldoutRect.height = 10f;
            Rect fieldRect = NewRect(propertyRect.x + 80f, foldoutRect.y + 42f, roomsRect.width - 205f, 16f);
            Rect addButtonRect = NewRect(labelRect.x, fieldRect.y - 21f, 50f, fieldRect.height);
            Rect clearButtonRect = addButtonRect;
            clearButtonRect.x += addButtonRect.width + 10f;
            Rect removeButtonRect = NewRect(fieldRect.x + fieldRect.width + 5f, addButtonRect.y + 21f, addButtonRect.height + 10f, addButtonRect.height);
            Rect indexRect = NewRect(fieldRect.x - 65f, fieldRect.y, 50f, 16f);

            EditorGUI.BeginProperty(propertyRect, GUIContent.none, property);

                GUI.Label(labelRect, new GUIContent(pieceName), skin.GetStyle("Piece"));

                foldouts[ID] = EditorGUI.Foldout(foldoutRect, foldouts[ID], GUIContent.none);
                if (foldouts[ID])
                {
                    List<GameObject>  list = GetPieceList(ID);

                    // ADD
                    GUI.backgroundColor = EditorColors.COLOR_GREEN;
                    if (GUI.Button(addButtonRect, new GUIContent("Add"))) list.Add(null);
                    GUI.backgroundColor = Color.white;

                    // CLEAR
                    GUI.backgroundColor = EditorColors.COLOR_CYAN;
                    if (GUI.Button(clearButtonRect, new GUIContent(" Clear"))) list.Clear();
                    GUI.backgroundColor = Color.white;

                    for (int i = 0; i < list.Count; i++)
                    {
                        // REMOVE
                        GUI.backgroundColor = EditorColors.COLOR_RED;
                        if (GUI.Button(removeButtonRect, new GUIContent("X")))
                        {
                            list.Remove(list[i]);
                            break;
                        }
                        GUI.backgroundColor = Color.white;

                        GUI.Label(indexRect, (i + 1).ToString(), skin.GetStyle("Field"));

                        list[i] = EditorGUI.ObjectField(fieldRect, list[i], typeof(GameObject), false) as GameObject;

                        fieldRect.y += fieldRect.height + 5f;
                        indexRect.y = fieldRect.y;
                        removeButtonRect.y = fieldRect.y;
                    }
                    pieceY += list.Count;
                }

            EditorGUI.EndProperty();
            #endregion Property
        }
        #endregion Main Methods


        #region Utility Methods
        public void Load(RoomData roomData = null)
        {
            room = (RoomData)CreateInstance(typeof(RoomData));

            if (roomData != null)
            {
                assetPath = AssetDatabase.GetAssetPath(roomData.GetInstanceID());

                room.Name = Path.GetFileNameWithoutExtension(assetPath);
                room.icon = roomData.icon;
                room.color = roomData.color;

                CopyList(ref room.rooms.DEFAULT, roomData.rooms.DEFAULT);
                CopyList(ref room.rooms.TBLR, roomData.rooms.TBLR);
                CopyList(ref room.rooms.TB, roomData.rooms.TB);
                CopyList(ref room.rooms.LR, roomData.rooms.LR);
                CopyList(ref room.rooms.T, roomData.rooms.T);
                CopyList(ref room.rooms.B, roomData.rooms.B);
                CopyList(ref room.rooms.L, roomData.rooms.L);
                CopyList(ref room.rooms.R, roomData.rooms.R);
                CopyList(ref room.rooms.TL, roomData.rooms.TL);
                CopyList(ref room.rooms.TR, roomData.rooms.TR);
                CopyList(ref room.rooms.BL, roomData.rooms.BL);
                CopyList(ref room.rooms.BR, roomData.rooms.BR);
                CopyList(ref room.rooms.TBL, roomData.rooms.TBL);
                CopyList(ref room.rooms.TBR, roomData.rooms.TBR);
                CopyList(ref room.rooms.TLR, roomData.rooms.TLR);
                CopyList(ref room.rooms.BLR, roomData.rooms.BLR);
            }
        }
        private void Save()
        {
            if (room != null)
            {
                if (string.IsNullOrEmpty(assetPath))
                {
                    assetPath = EditorUtility.SaveFilePanelInProject("Save Room", room.Name, "asset",
                        "Please choose a directory to save the room");

                    if (!string.IsNullOrEmpty(assetPath))
                    {
                        AssetDatabase.CreateAsset(room, assetPath);
                    }
                }
                else
                {
                    EditorUtility.CopySerialized(room, AssetDatabase.LoadAssetAtPath(assetPath, typeof(RoomData)));

                    AssetDatabase.RenameAsset(assetPath, room.Name);
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Debug.Log("<b>Dungeon Maker</b>\nRoom Editor: Changes saved successfully in " + room.Name + '.');
                Load((RoomData)AssetDatabase.LoadAssetAtPath(assetPath, typeof(RoomData)));
            }
        }
        private void CreateData(string wantedName)
        {
            RoomData ro = CreateInstance<RoomData>() as RoomData;

            if (ro != null)
            {
                ro.Name = wantedName;

                string assetPath = EditorUtility.SaveFilePanelInProject("Save Room", wantedName, "asset",
                            "Please choose a directory to save the room");

                if (string.IsNullOrEmpty(assetPath))
                {
                    return;
                }

                AssetDatabase.CreateAsset(ro, assetPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                if (window != null)
                {
                    Load(ro);
                }
            }
        }
        private void LoadData()
        {
            RoomData ro = null;
            string roomPath = EditorUtility.OpenFilePanel("Load Room", Application.dataPath, "asset");

            if (string.IsNullOrEmpty(roomPath))
            {
                return;
            }

            int appPathLen = Application.dataPath.Length;
            string finalPath = roomPath.Substring(appPathLen - 6);

            ro = (RoomData)AssetDatabase.LoadAssetAtPath(finalPath, typeof(RoomData));

            if (ro != null)
            {
                Load(ro);
            }
            else
            {
                EditorUtility.DisplayDialog("Room Editor", "Unable to load selected room!", "OK");
            }
        }
        private void UnloadData()
        {
            room = null;
        }
        private string GetPieceName(int ID)
        {
            switch (ID)
            {
                case 0: return "DEFAULT";
                case 1: return "TBLR";
                case 2: return "TB";
                case 3: return "LR";
                case 4: return "T";
                case 5: return "B";
                case 6: return "L";
                case 7: return "R";
                case 8: return "TL";
                case 9: return "TR";
                case 10: return "BL";
                case 11: return "BR";
                case 12: return "TBL";
                case 13: return "TBR";
                case 14: return "TLR";
                case 15: return "BLR";
                default: return null;
            }
        }
        private List<GameObject> GetPieceList(int ID)
        {
            if (room == null)
            {
                return null;
            }

            switch (ID)
            {
                case 0: return room.rooms.DEFAULT;
                case 1: return room.rooms.TBLR;
                case 2: return room.rooms.TB;
                case 3: return room.rooms.LR;
                case 4: return room.rooms.T;
                case 5: return room.rooms.B;
                case 6: return room.rooms.L;
                case 7: return room.rooms.R;
                case 8: return room.rooms.TL;
                case 9: return room.rooms.TR;
                case 10: return room.rooms.BL;
                case 11: return room.rooms.BR;
                case 12: return room.rooms.TBL;
                case 13: return room.rooms.TBR;
                case 14: return room.rooms.TLR;
                case 15: return room.rooms.BLR;
                default: return null;
            }
        }
        private void CopyList(ref List<GameObject> list, List<GameObject> src)
        {
            for (int i = 0; i < src.Count; i++)
            {
                list.Add(src[i]);
            }
        }
        private void Reset()
        {
            if (room != null)
            {
                room.rooms.DEFAULT = new List<GameObject>();
                room.rooms.TBLR = new List<GameObject>();
                room.rooms.TB = new List<GameObject>();
                room.rooms.LR = new List<GameObject>();
                room.rooms.T = new List<GameObject>();
                room.rooms.B = new List<GameObject>();
                room.rooms.L = new List<GameObject>();
                room.rooms.R = new List<GameObject>();
                room.rooms.TL = new List<GameObject>();
                room.rooms.TR = new List<GameObject>();
                room.rooms.BL = new List<GameObject>();
                room.rooms.BR = new List<GameObject>();
                room.rooms.TBL = new List<GameObject>();
                room.rooms.TBR = new List<GameObject>();
                room.rooms.TLR = new List<GameObject>();
                room.rooms.BLR = new List<GameObject>();
            }
        }
        #endregion Utility Methods
    }
}