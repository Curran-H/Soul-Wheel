using UnityEngine;
using UnityEditor;
using UEditor = UnityEditor.Editor;

namespace DungeonMaker.Editor
{
	[CustomEditor(typeof(Generator))]
	public class GeneratorInspector : UEditor
	{
		private bool auto;
		private bool multithreading;
		private bool staticSeed;
		private bool generating;

		private void OnEnable()
		{
			Generator.OnGeneratorStart += GeneratingOn;
			Generator.OnGeneratorFinish += GeneratingOff;
		}

		private void OnDisable()
		{
			Generator.OnGeneratorStart -= GeneratingOn;
			Generator.OnGeneratorFinish -= GeneratingOff;
		}

		public override void OnInspectorGUI()
		{
			Generator generator = (Generator)target;

			#region Auto
			EditorGUILayout.BeginHorizontal();

			auto = generator.auto;
			auto = EditorGUILayout.Toggle(new GUIContent("Auto", "Generate a dungeon automatically at the beginning."), auto);
			generator.auto = auto;

			EditorGUILayout.EndHorizontal();
			#endregion Auto


			#region Multithreading
			EditorGUILayout.BeginHorizontal();

			multithreading = generator.multithreading;
			multithreading = EditorGUILayout.Toggle(new GUIContent("Multithreading", "Execute the dungeon generation as a sub-process in another thread."), multithreading);
			generator.multithreading = multithreading;

			EditorGUILayout.EndHorizontal();
			#endregion Multithreading


			#region Seed
			EditorGUILayout.BeginHorizontal();

			staticSeed = generator.staticSeed;
			staticSeed = EditorGUILayout.Toggle(new GUIContent("Seed", "Number used to initialize the pseudo-random generator."), staticSeed);
			generator.staticSeed = staticSeed;

			if (staticSeed)
			{
				generator.seed = EditorGUILayout.IntField(generator.seed);
				GUILayout.Space(10);
			}
			else
			{
				GUILayout.Label(generator.seed.ToString());
			}

			EditorGUILayout.EndHorizontal();
			#endregion Seed


			#region Mode
			EditorGUILayout.BeginHorizontal();

			generator.mode = (ModeType)EditorGUILayout.EnumPopup(new GUIContent("Mode", "2D - Generates the dungeon on the X and Y axis.\n3D - Generates the dungeon on the X and Z axes."), generator.mode);

			GUILayout.Space(10);

			EditorGUILayout.EndHorizontal();
			#endregion Mode


			#region Spacing
			EditorGUILayout.BeginHorizontal();

			generator.spacing = EditorGUILayout.Vector2Field(new GUIContent("Spacing", "Size of the rooms in width (X) and length (Y)."), generator.spacing);

			EditorGUILayout.EndHorizontal();
			#endregion Spacing


			#region Dungeons
			EditorGUILayout.BeginHorizontal();

			SerializedProperty dungeons = serializedObject.FindProperty("dungeons");

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(dungeons, true);

			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
			}

			GUILayout.Space(10);

			EditorGUILayout.EndHorizontal();
			#endregion Dungeons


			#region End
			GUILayout.Space(15);

			EditorGUILayout.BeginHorizontal();

			// BUILD
			GUI.backgroundColor = generating ? EditorColors.GRID_COLOR : EditorColors.COLOR_GREEN;
			if (GUILayout.Button("Build", GUILayout.Height(30)))
			{
				generator.Generate();
			}
			GUI.backgroundColor = Color.white;

			//DESTROY
			GUI.backgroundColor = EditorColors.COLOR_RED;
			if (GUILayout.Button("Destroy", GUILayout.Height(30), GUILayout.Width(Screen.width * 0.3f)))
			{
				generator.Destroy(true);
			}
			GUI.backgroundColor = Color.white;

			GUILayout.Space(10);

			EditorGUILayout.EndHorizontal();
			#endregion End
		}

		private void GeneratingOn() { generating = true; }
		private void GeneratingOff(DungeonObject d) { generating = false; }
	}
}