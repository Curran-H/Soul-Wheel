using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DungeonMaker.Core
{
    public class RoomNode : NodeBase
    {
        #region Const Variables
        public const int MAX_CONNECTIONS = 4;
        private const int ICON_SIZE = 80;
        private const int BUTTON_SIZE = 20;
        #endregion Const Variables


        #region Public Variables
        public RoomData room;
        public List<int> connections = new List<int>();
        [HideInInspector]
        public List<int> initialConnections = new List<int>();
        [HideInInspector]
        public Rect buttonRect;
        #endregion Public Variables


        #region Constructors
        public RoomNode() { }
        #endregion Constructors


        #region Main Methods
        public override void InitNode()
        {
            base.InitNode();

            nodeRect = new Rect(10f, 10f, ICON_SIZE, ICON_SIZE);
        }

        public override void UpdateNode(Event e, Rect viewRect)
        {
            base.UpdateNode(e, viewRect);
        }

#if UNITY_EDITOR
        public override void UpdateNodeGUI(Event e, Rect viewRect, GUISkin viewSkin)
        {
            base.UpdateNodeGUI(e, viewRect, viewSkin);

            nodeRect.width = (ICON_SIZE) * 0.5f;
            nodeRect.height = (ICON_SIZE) * 0.5f;

            buttonRect = nodeRect;
            buttonRect.x -= (BUTTON_SIZE / 2);
            buttonRect.y -= (BUTTON_SIZE / 2);
            buttonRect.width += (BUTTON_SIZE);
            buttonRect.height += (BUTTON_SIZE);

            if (isSelected)
            {
                GUI.DrawTexture(buttonRect, viewSkin.GetStyle("DM_Button").active.background);
            }
            else if (nodeRect.Contains(e.mousePosition))
            {
                GUI.DrawTexture(buttonRect, viewSkin.GetStyle("DM_Button").hover.background);
            }
            else
            {
                GUI.DrawTexture(buttonRect, viewSkin.GetStyle("DM_Button").normal.background);
            }

			//room.icon.ApplyColor(room.color);
			GUI.DrawTexture(nodeRect, room.icon);
			//room.icon.ApplyColor(Color.white);

			EditorUtility.SetDirty(this);
        }

        public override void DrawNodeProperties()
        {
            base.DrawNodeProperties();
        }
#endif
        #endregion Main Methods


        #region Utility Methods
        public void Connect(int connectionID)
        {
            if (!connections.Contains(connectionID) 
                && connections.Count < MAX_CONNECTIONS)
            {
                connections.Add(connectionID);
            }
        }
        public void Disconnect(int connectionID)
        {
            if (connections.Contains(connectionID))
            {
                connections.Remove(connectionID);
            }
        }
        #endregion Utility Methods
    }
}