using UnityEngine;
using UnityEditor;

namespace DungeonMaker.Editor
{
    public class HelpWindow : EditorWindow
    {
		private static EditorWindow window;
        private const string WINDOW_NAME = "Help";
        private const float LOGO_SIZE = 80f;

		public Texture2D logo;
        private Rect logoRect;

		public static void OpenWindow()
        {
            window = GetWindow<HelpWindow>();
            window.minSize = new Vector2(704f, 224f);
            window.titleContent = new GUIContent(WINDOW_NAME);
        }

		private void OnGUI()
		{
            logoRect.width = LOGO_SIZE;
            logoRect.height = LOGO_SIZE;
            logoRect.x = Screen.width / 2 - logoRect.width / 2;
            logoRect.y = logoRect.height - logo.height / 3;

            GUILayout.Space(logo.height - 24f);

            GUI.DrawTexture(logoRect, logo);
            GUILayout.Label("If you need help, I recommend that you read the Dungeon Maker documentation.\n" +
                "You can also see how the demo projects are configured.\n" +
                "In addition, there is a small video tutorial that you can find on the store's page.\n\n" +
                "If necessary, you can contact the creator of the product by sending an email to the address at the end of the documentation.");
		}
	}
}