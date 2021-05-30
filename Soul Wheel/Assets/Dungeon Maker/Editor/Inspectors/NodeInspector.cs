using UnityEngine;
using UnityEditor;
using DungeonMaker.Core;
using UEditor = UnityEditor.Editor;

namespace DungeonMaker.Editor
{
    [CustomEditor(typeof(RoomNode))]
    public class NodeInspector : UEditor
    {
        private void OnEnable()
        {

        }

        public override void OnInspectorGUI()
        {
            RoomNode node = (RoomNode)target;

            GUILayout.Label(string.Format("ROOM\n{0}", node.room.Name));
            GUILayout.Space(10);
            GUILayout.Label(string.Format("ID\n{0}", node.nodeID.ToString()));
            GUILayout.Space(10);
            GUILayout.Label("CONNECTIONS");
			for (int i = 0; i < node.connections.Count; i++)
			{
                GUILayout.Label(node.connections[i].ToString());
			}
        }
    }
}