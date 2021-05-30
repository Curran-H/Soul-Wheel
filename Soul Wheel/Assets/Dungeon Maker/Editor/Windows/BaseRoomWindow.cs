using UnityEngine;
using UnityEditor;
using DungeonMaker.Core;

namespace DungeonMaker.Editor
{
    public class BaseRoomWindow : EditorWindow
    {
        #region Const Variables
        protected const string ROOM_EDITOR_TITLE = "DUNGEON  MAKER";
        protected static Color ROOM_EDITOR_TITLE_COLOR = EditorColors.COLOR_YELLOW;
        protected const string ROOM_SKIN_PATH = "Skins/RoomEditorSkin";
        #endregion Const Variables


        #region Public Variables
        public static RoomWindow window;
        public RoomData room;
        #endregion Public Variables


        #region Protected
        protected GUISkin skin;
        protected Rect windowRect;

        protected Rect headerRect;
        protected Color headerColor;
        protected Texture2D headerTex;

        protected Rect bodyRect;
        protected Color bodyColor;
        protected Texture2D bodyTex;
        #endregion Protected Variables


        #region Editor Methods
        protected virtual void OnEnable()
        {
            skin = Resources.Load<GUISkin>(ROOM_SKIN_PATH);

            headerColor = EditorColors.MAIN1_COLOR;
            headerTex = new Texture2D(1, 1);
            headerTex.SetPixel(0, 0, headerColor);
            headerTex.Apply();

            bodyColor = EditorColors.MAIN2_COLOR;
            bodyTex = new Texture2D(1, 1);
            bodyTex.SetPixel(0, 0, bodyColor);
            bodyTex.Apply();
        }

        protected virtual void OnGUI()
        {
            if (EditorApplication.isPlaying)
            {
                Debug.LogWarning("<b>Dungeon Maker</b>\nRoom Editor: The Room Editor has been closed because edit in play mode is not supported.");
                Close();
                return;
            }

            DrawLayouts();
            DrawHeader();
            DrawBody();
        }
        #endregion Editor Methods


        #region Main Methods
        public static void OpenWindow()
        {
            if (EditorApplication.isPlaying)
            {
                EditorUtility.DisplayDialog("Room Editor", "Edit in play mode is not supported.", "OK");
                return;
            }
            window = GetWindow<RoomWindow>();
            window.minSize = new Vector2(362f, 328f);
            window.titleContent = new GUIContent("Room Editor");
        }

        protected virtual void DrawLayouts()
        {
            windowRect = position;
            headerRect = NewRect(0f, 0f, windowRect.width, 50f);
            bodyRect = NewRect(0f, headerRect.height, windowRect.width, windowRect.height - headerRect.height);
            GUI.DrawTexture(headerRect, headerTex);
            GUI.DrawTexture(bodyRect, bodyTex);
        }

        protected virtual void DrawHeader() { }
        protected virtual void DrawBody() { }
        #endregion Main Methods


        #region Utility Methods
        protected virtual Rect NewRect(float x, float y, float w, float h) => new Rect(new Vector2(x, y), new Vector2(w, h));
        #endregion Utility Methods
    }
}