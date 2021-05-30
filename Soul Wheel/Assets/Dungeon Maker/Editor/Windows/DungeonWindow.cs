using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using DungeonMaker.Core;

namespace DungeonMaker.Editor
{
    public class DungeonWindow : EditorWindow
    {
        #region Const Variables
        public const string DUNGEON_SKIN_PATH = "Skins/DungeonEditorSkin";
        private const float VIEW_PERCENTAGE = 0.75f;
        #endregion Const Variables


        #region Public Variables
        public static DungeonWindow window;

        public DungeonWorkView workView;
        public DungeonPropertyView propertyView;
        public float viewPercentage = VIEW_PERCENTAGE;

        public DungeonData dungeon;
        public string assetPath;

        public List<RoomNode> trash = new List<RoomNode>();
        #endregion Public Variables


        #region Private Variables
        private DungeonData loadedDungeon;
        private bool saved;
        #endregion Private Variables


        #region Main Methods
        public static void OpenWindow()
        {
            if (EditorApplication.isPlaying)
            {
                EditorUtility.DisplayDialog("Dungeon Editor", "Edit in play mode is not supported.", "OK");
                return;
            }

            window = (DungeonWindow)EditorWindow.GetWindow<DungeonWindow>();
            window.minSize = new Vector2(828f, 358f);
            window.titleContent = new GUIContent("Dungeon Editor");

            CreateViews();
        }

        private void OnEnable()
        {
            trash.Clear();
        }

        private void OnDestroy()
        {
            if (dungeon != null)
            {
                dungeon.DeselectAllNodes();

                if (!saved)
                {
                    RevertConnections();
                }
            }
        }

        private void OnGUI()
        {
            if (EditorApplication.isPlaying)
            {
                Debug.LogWarning("<b>Dungeon Maker</b>\nDungeon Editor: The Dungeon Editor has been closed because edit in play mode is not supported.");
                Close();
                return;
            }

            if (propertyView == null || workView == null)
            {
                CreateViews();
                return;
            }

            Event e = Event.current;
            ProcessEvents(e);

            workView.UpdateView(position, new Rect(0f, 0f, viewPercentage, 1f), e, dungeon, window);
            propertyView.UpdateView(new Rect(position.width, position.y, position.width, position.height),
                                    new Rect(viewPercentage, 0f, 1f - viewPercentage, 1f), e, dungeon, window);

            Repaint();
        }

        private void ProcessEvents(Event e) { }
        #endregion Main Methods


        #region Utility Methods
        private static void CreateViews()
        {
            if (window != null)
            {
                window.propertyView = new DungeonPropertyView();
                window.workView = new DungeonWorkView();
            }
            else
            {
                window = GetWindow<DungeonWindow>();
            }
        }

        public void Load(DungeonData dungeonToLoad = null)
        {
            dungeon = (DungeonData)CreateInstance(typeof(DungeonData));
            dungeon.InitGraph();

            if (dungeonToLoad != null)
            {
                loadedDungeon = dungeonToLoad;

                assetPath = AssetDatabase.GetAssetPath(dungeonToLoad.GetInstanceID());
                dungeon.Name = Path.GetFileNameWithoutExtension(assetPath);

                CopyNodes(dungeonToLoad.nodes, dungeon.nodes);

                foreach (RoomNode node in dungeon.nodes)
                {
                    foreach (int connection in node.connections)
                    {
                        node.initialConnections.Add(connection);
                    }
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public void Save()
        {
            if (dungeon != null)
            {
                if (loadedDungeon != null)
                {
                    foreach (RoomNode node in loadedDungeon.nodes)
                    {
                        trash.Add(node);
                    }
                }
                EmptyTrash();

                if (string.IsNullOrEmpty(assetPath))
                {
                    assetPath = EditorUtility.SaveFilePanelInProject("Save Dungeon", dungeon.Name, "asset",
                        "Please choose a directory to save the dungeon");

                    if (!string.IsNullOrEmpty(assetPath))
                    {
                        AssetDatabase.CreateAsset(dungeon, assetPath);

                        UpdateObjects();
                    }
                }
                else
                {
					EditorUtility.CopySerialized(dungeon, AssetDatabase.LoadAssetAtPath(assetPath, typeof(DungeonData)));

					UpdateObjects();

                    AssetDatabase.RenameAsset(assetPath, dungeon.Name);
				}

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Debug.Log("<b>Dungeon Maker</b>\nDungeon Editor: Changes saved successfully in " + dungeon.Name + '.');
                saved = true;

                Load(loadedDungeon);
            }
        }

        public void Reload()
        {
            if (dungeon != null)
            {
                Load(loadedDungeon);
            }
            else
            {
                Debug.LogWarning("<b>Dungeon Maker</b>\nDungeon Editor: " + dungeon.Name + " could not be reloaded.");
            }
        }
        
        private void UpdateObjects()
        {
            foreach (RoomNode node in dungeon.nodes)
            {
                if (node == null) continue;

                if (!AssetDatabase.Contains(node))
                {
                    if (loadedDungeon != null)
                    {
						node.dungeon = loadedDungeon;
					}

                    AssetDatabase.AddObjectToAsset(node, AssetDatabase.LoadAssetAtPath(assetPath, typeof(DungeonData)));
                }
            }

            AssetDatabase.SetMainObject(AssetDatabase.LoadAssetAtPath(assetPath, typeof(DungeonData)), assetPath);
        }

        private void CopyNodes(List<RoomNode> src, List<RoomNode> dst)
        {
            foreach (RoomNode node in src)
            {
                RoomNode copy = (RoomNode)CreateInstance(typeof(RoomNode));

                copy.nodeID = node.nodeID;
                copy.isSelected = false;
                copy.nodeRect = node.nodeRect;
                copy.nodeType = node.nodeType;
                copy.dungeon = dungeon;
                copy.room = node.room;

                foreach (int connection in node.connections)
                {
                    copy.connections.Add(connection);
                }

                dst.Add(copy);
            }
        }

        public void RevertConnections()
        {
            if (dungeon != null)
            {
                foreach (RoomNode node in dungeon.nodes)
                {
                    node.connections.Clear();

                    foreach (int connection in node.initialConnections)
                    {
                        node.connections.Add(connection);
                    }
                }
            }
        }

        private void EmptyTrash()
        {
            foreach (RoomNode node in trash)
            {
                if (node != null)
                {
                    DestroyImmediate(node, true);
                }
            }
        }
		#endregion Utility Methods
	}
}