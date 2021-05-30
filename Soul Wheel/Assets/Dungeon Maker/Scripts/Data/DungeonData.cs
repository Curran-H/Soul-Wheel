using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DungeonMaker.Core
{
    [CreateAssetMenu(fileName = DEFAULT_DUNGEON_NAME, menuName = "Dungeon Maker/New Dungeon")]
    public class DungeonData : ScriptableObject
    {
        #region Const Variables
        private const string DEFAULT_DUNGEON_NAME = "New Dungeon";
		#endregion Const Variables


		#region Public Variables
		//public string dungeonName = DEFAULT_DUNGEON_NAME;
		public List<RoomNode> nodes = new List<RoomNode>();
        public List<RoomNode> selectedNodes = new List<RoomNode>();
        public RoomNode currentNode;
        public RoomNode connectionNode;

        public bool wantMove;
        public bool wantConnect;
        public bool wantDisconnect;
        public bool wantSelect;
        //public bool wantMultiselect;
        public bool showProperties;

        //public Vector3 multiselectorOrigin;
		#endregion Public Variables


		#region Properties
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
		#endregion Properties


		#region Unity Methods
		private void OnEnable()
        {
            currentNode = null;
            connectionNode = null;

            wantSelect = false;
            //wantMultiselect = false;
            wantConnect = false;
            wantDisconnect = false;
            showProperties = false;

            if (nodes == null)
            {
                nodes = new List<RoomNode>();
            }

            if (selectedNodes == null)
            {
                selectedNodes = new List<RoomNode>();
            }

            DeselectAllNodes();
        }
		#endregion Unity Methods


		#region Main Methods
		public void InitGraph()
        {
            if (nodes.Count > 0)
            {
                foreach (RoomNode node in nodes)
                {
                    if (node != null)
                    {
                        node.InitNode();
                    }
                }
            }
        }

#if UNITY_EDITOR
        public void UpdateGraphGUI(Event e, Rect viewRect, GUISkin viewSkin)
        {
            if (nodes.Count > 0)
            {
                UpdateConnections();

                ProcessEvents(e, viewRect);

                UpdateNodes(e, viewRect, viewSkin);

                currentNode = GetCurrentNode(e);
            }

            if (connectionNode != null)
            {
                if (wantConnect)
                {
                    DrawConnectionToMouse(e.mousePosition, Color.green);
                }

                if (wantDisconnect)
                {
                    DrawConnectionToMouse(e.mousePosition, Color.red);
                }
            }

   //         if (wantMultiselect)
   //         {
			//	UpdateMultiselector(e);
			//}

            if (e.type == EventType.Layout)
            {
                showProperties = selectedNodes.Count == 1 ? true : false;
            }

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        private void UpdateConnections()
        {
            foreach (RoomNode node in nodes)
            {
                foreach (int connectionID in node.connections)
                {
                    Vector3 p1 = node.nodeRect.position;
                    Vector3 p2 = GetNode(connectionID).nodeRect.position;
                    Vector3 offset = new Vector3(node.nodeRect.width / 2, node.nodeRect.height / 2);

                    DrawConnection(p1 + offset, p2 + offset, Color.white);
                }
            }
        }

        private void UpdateNodes(Event e, Rect viewRect, GUISkin viewSkin)
        {
            foreach (RoomNode node in nodes)
            {
                if (node != null)
                {
                    node.UpdateNodeGUI(e, viewRect, viewSkin);
                }
            }
        }

        //private void UpdateMultiselector(Event e)
        //{
        //    Rect multiselectorRect = RectFromDragPoints(multiselectorOrigin, e.mousePosition);

        //    Texture2D multiselectorTex = new Texture2D(1, 1);
        //    multiselectorTex.SetPixel(0, 0, EditorColors.MULTISELECTOR_COLOR);
        //    multiselectorTex.Apply();


        //    GUI.DrawTexture(multiselectorRect, multiselectorTex);


        //    foreach (RoomNode node in nodes)
        //    {
        //        if (node != null)
        //        {
        //            Rect nodeRect = node.nodeRect;
        //            Vector2 bl = new Vector2(nodeRect.x, nodeRect.y);
        //            Vector2 br = new Vector2(nodeRect.x + nodeRect.width, nodeRect.y);
        //            Vector2 tl = new Vector2(nodeRect.x, nodeRect.y + nodeRect.height);
        //            Vector2 tr = new Vector2(nodeRect.x + nodeRect.width, nodeRect.y + nodeRect.height);

        //            if (multiselectorRect.Contains(bl)
        //                || multiselectorRect.Contains(br)
        //                || multiselectorRect.Contains(tl)
        //                || multiselectorRect.Contains(tr))
        //            {
        //                SelectNode(node);
        //            }
        //            else
        //            {
        //                DeselectNode(node);
        //            }
        //        }
        //    }
        //}

        private RoomNode GetCurrentNode(Event e)
        {
            RoomNode current = null;
            bool overNode = false;

            foreach (RoomNode node in nodes)
            {
                if (node != null)
                {
                    if (node.nodeRect.Contains(e.mousePosition))
                    {
                        current = node;
                        overNode = true;
                        break;
                    }
                }
            }

            if (!overNode)
            {
                current = null;
            }

            return current;
        }
#endif

        private void ProcessEvents(Event e, Rect viewRect)
        {
            if (viewRect.Contains(e.mousePosition))
            {
                // LEFT CLICK
                if (e.button == 0)
                {
                    if (e.type == EventType.MouseDown)
                    {
                        if (wantSelect)
                        {
                            return;
                        }

                        if (wantConnect)
                        {
                            ConnectTwoNodes(connectionNode, currentNode);
                        }
                        else if (wantDisconnect)
                        {
                            DisconnectTwoNodes(connectionNode, currentNode);
                        }
                        else
                        {
                            if (currentNode != null)
                            {
                                if (!currentNode.isSelected)
                                {
                                    DeselectAllNodes();
                                    SelectNode(currentNode);
                                }
                            }
                            else
                            {
                                if (!wantMove)
                                {
                                    DeselectAllNodes();
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion Main Methods


        #region Utility Methods
        public void DrawConnection(Vector3 p1, Vector3 p2, Color col)
        {
#if UNITY_EDITOR
            Handles.BeginGUI();

            Handles.color = col;
            Handles.DrawAAPolyLine(p1, p2);

            Handles.EndGUI();
#endif
        }

        public void DrawConnectionToMouse(Vector2 mousePosition, Color col)
        {
            Vector3 p1 = new Vector3(connectionNode.nodeRect.x + connectionNode.nodeRect.width / 2, connectionNode.nodeRect.y + connectionNode.nodeRect.height / 2, 0f);
            Vector3 p2 = new Vector3(mousePosition.x, mousePosition.y, 0f);

            DrawConnection(p1, p2, col);
        }

        public void ConnectTwoNodes(RoomNode nodeA, RoomNode nodeB)
        {
            if (nodeA && nodeB && CanConnect)
            {
                nodeA.Connect(nodeB.nodeID);
                nodeB.Connect(nodeA.nodeID);
            }

            connectionNode = null;
            wantConnect = false;
        }

        public void DisconnectTwoNodes(RoomNode nodeA, RoomNode nodeB)
        {
            if (nodeA && nodeB)
            {
                nodeA.Disconnect(nodeB.nodeID);
                nodeB.Disconnect(nodeA.nodeID);
            }

            connectionNode = null;
            wantDisconnect = false;
        }

        public void SelectNode(RoomNode node)
        {
            if (node != null)
            {
                if (!selectedNodes.Contains(node))
                {
                    selectedNodes.Add(node);
                }

                node.isSelected = true;
            }
        }

        public void DeselectNode(RoomNode node)
        {
            if (node != null)
            {
                if (selectedNodes.Contains(node))
                {
                    selectedNodes.Remove(node);
                }

                node.isSelected = false;
            }
        }

        public void DeselectAllNodes()
        {
            foreach (RoomNode node in nodes)
            {
                if (node != null)
                {
                    DeselectNode(node);
                }
            }
        }

        public RoomNode GetNode(int ID)
        {
            foreach (RoomNode node in nodes)
            {
                if (node != null)
                {
                    if (node.nodeID == ID)
                    {
                        return node;
                    }
                }
            }

            return null;
        }

        //private Rect RectFromDragPoints(Vector2 p1, Vector2 p2)
        //{
        //    Vector2 dif = p1 - p2;
        //    Rect rect = new Rect();

        //    if (dif.x < 0)
        //        rect.x = p1.x;
        //    else
        //        rect.x = p2.x;
        //    if (dif.y < 0)
        //        rect.y = p1.y;
        //    else
        //        rect.y = p2.y;

        //    rect.width = Mathf.Abs(dif.x);
        //    rect.height = Mathf.Abs(dif.y);

        //    return rect;
        //}

        private bool CanConnect
        {
            get
            {
                if (currentNode != null)
                {
                    return currentNode != connectionNode
                            && !currentNode.connections.Contains(connectionNode.nodeID)
                            && currentNode.connections.Count < RoomNode.MAX_CONNECTIONS
                            && connectionNode.connections.Count < RoomNode.MAX_CONNECTIONS;
                }

                return false;
            }
        }
        #endregion Utility Methods
    }
}