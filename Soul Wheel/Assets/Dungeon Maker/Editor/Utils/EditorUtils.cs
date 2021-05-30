using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DungeonMaker.Core;

namespace DungeonMaker.Editor
{
    public static class EditorUtils
    {
        #region Graphs Methods
        public static void CreateNewGraph(string wantedName)
        {
            DungeonData dungeon = (DungeonData)ScriptableObject.CreateInstance<DungeonData>();

            if (dungeon != null)
            {
                dungeon.Name = wantedName;
                dungeon.InitGraph();

                string assetPath = EditorUtility.SaveFilePanelInProject("Save Graph", dungeon.Name, "asset",
                        "Please choose a directory to save the dungeon");

                if (!string.IsNullOrEmpty(assetPath))
                {
                    AssetDatabase.CreateAsset(dungeon, assetPath);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    if (DungeonWindow.window != null)
                    {
                        DungeonWindow.window.Load(dungeon);
                    }
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Dungeon Editor", "Unable to create new dungeon, please see you fiendly programmer!", "OK");
            }
        }

        public static void LoadGraph()
        {
            string assetPath = EditorUtility.OpenFilePanel("Load Graph", Application.dataPath, "asset");

            if (!string.IsNullOrEmpty(assetPath))
            {
                int appPathLen = Application.dataPath.Length;
                string finalPath = assetPath.Substring(appPathLen - 6);
                DungeonData dungeon = (DungeonData)AssetDatabase.LoadAssetAtPath(finalPath, typeof(DungeonData));

                if (dungeon != null)
                {
                    if (DungeonWindow.window != null)
                    {
                        DungeonWindow.window.Load(dungeon);
                    }
                }
                else
                {
                    EditorUtility.DisplayDialog("Dungeon Editor", "Unable to load selected graph!", "OK");
                }
            }
        }

        public static void UnloadGraph()
        {
            if (DungeonWindow.window != null)
            {
                DungeonWindow.window.dungeon = null;
                DungeonWindow.window.RevertConnections();
            }
        }
        #endregion Graphs Methods


        #region Nodes Methods
        public static RoomNode CreateNode(DungeonData dungeon, RoomData room, Vector2 mousePos)
        {
            if (dungeon != null)
            {
                RoomNode node = ScriptableObject.CreateInstance<RoomNode>() as RoomNode;

                if (node != null)
                {
                    node.nodeID = GenerateID(dungeon);
                    node.nodeType = NodeType.ROOM;
                    node.room = room;

                    node.InitNode();
                    node.nodeRect.x = mousePos.x;
                    node.nodeRect.y = mousePos.y;
                    node.dungeon = dungeon;
                    dungeon.nodes.Add(node);

                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    return node;
                }
            }

            return null;
        }
        #endregion Nodes Methods


        #region Utility Methods
        public static void DrawGrid(Rect container, Vector2 offset, float gridSize, Color color)
        {
            float zoomFactor = 1.60f;

            if (Event.current.type != EventType.Repaint)
            {
                return;
            }

            Handles.color = color;

            var drawGridSize = zoomFactor > 0.5f ? gridSize : gridSize * 5;
            var step = drawGridSize * zoomFactor;

            var xDiff = offset.x % step;
            var xStart = container.xMin + xDiff;
            var xEnd = container.xMax;
            for (var i = xStart; i < xEnd; i += step)
            {
                Handles.DrawLine(new Vector3(i, container.yMin, 0), new Vector3(i, container.yMax, 0));
            }

            var yDiff = offset.y % step;
            var yStart = container.yMin + yDiff;
            var yEnd = container.yMax;
            for (var i = yStart; i < yEnd; i += step)
            {
                Handles.DrawLine(new Vector3(0, i, 0), new Vector3(container.xMax, i, 0));
            }

            Handles.color = Color.white;
        }

        public static List<RoomData> GetRoomsFromTheDatabase()
        {
            List<RoomData> lst = new List<RoomData>();
            Object[] rooms = Resources.LoadAll(DungeonWorkView.ROOMS_FOLDER, typeof(RoomData));

            foreach (RoomData room in rooms)
            {
                lst.Add(room);
            }

            return lst;
        }

        private static int GenerateID(DungeonData dungeon)
        {
            int ID;
            bool isValid;

            do
            {
                isValid = true;
                ID = Random.Range(11111, 99999);

                foreach (RoomNode node in dungeon.nodes)
                {
                    if (node != null)
                    {
                        if (node.nodeID == ID)
                        {
                            isValid = false;
                            break;
                        }
                    }
                }

            } while (!isValid);

            return ID;
        }

        public static string ReplaceFirstOccurrance(string original, string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(original)) return string.Empty;
            if (string.IsNullOrEmpty(oldValue)) return original;
            if (string.IsNullOrEmpty(newValue)) newValue = string.Empty;

            int loc = original.IndexOf(oldValue);

            return loc == -1 ? original : original.Remove(loc, oldValue.Length).Insert(loc, newValue);
        }
        #endregion Utility Methods
    }
}