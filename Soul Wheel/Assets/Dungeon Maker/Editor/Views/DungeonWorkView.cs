using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using DungeonMaker.Core;

namespace DungeonMaker.Editor
{
    [Serializable]
    public class DungeonWorkView : ViewBase
    {
        #region Const Variables
        public const string ROOMS_FOLDER = "Rooms";
        private const float DEFAULT_ZOOM = 1.60f;
        private const float ZOOM_SPEED = 3f;
        private const float MAX_ZOOM = 2f;
        private const float MIN_ZOOM = 1f;
        private const float SIZE = 750f;
		#endregion Const Variables


		#region Private Variables
		private Vector2 gridOffset;
        private Vector2 mousePos;
        private RoomData copyRoom;
        private float zoom = DEFAULT_ZOOM;
		private Vector2 zoomCoordsOrigin = Vector2.zero;
        public Vector3 multiselectorOrigin;
        private Rect zoomArea;
        private RoomNode newNode;
        //private bool wantMultiselect;
        private bool inContext;
        #endregion Private Variables


        #region Constructors
        public DungeonWorkView() : base("Work View") { }
        #endregion Constructors


        #region Main Methods
        public override void UpdateView(Rect editorRect, Rect percentageRect, Event e, DungeonData dungeon, DungeonWindow window)
        {
            base.UpdateView(editorRect, percentageRect, e, dungeon, window);

            mousePos = e.mousePosition;

            #region Zoom Area
            zoomArea = new Rect(viewArea.x - SIZE - zoomCoordsOrigin.x, viewArea.y - SIZE - zoomCoordsOrigin.y, viewArea.width + SIZE * 10, viewArea.height + SIZE * 10);
            EditorZoomArea.Begin(zoom, viewArea);

            GUI.Box(viewArea, "", skin.GetStyle("ViewBG"));
            EditorUtils.DrawGrid(zoomArea, gridOffset, 20, EditorColors.GRID_COLOR);
            EditorUtils.DrawGrid(zoomArea, gridOffset, 80, EditorColors.GRID_COLOR);
            EditorUtils.DrawGrid(zoomArea, gridOffset, 80, EditorColors.GRID_COLOR);

            GUILayout.BeginArea(zoomArea);
            if (dungeon != null)
            {
                if (newNode != null)
				{
                    newNode.nodeRect.x = e.mousePosition.x;
                    newNode.nodeRect.y = e.mousePosition.y;

                    newNode = null;
				}

                dungeon.UpdateGraphGUI(e, zoomArea, skin);
            }
            GUILayout.EndArea();

            EditorZoomArea.End();
			#endregion Zoom Area

			ProcessEvents(e);

            //if (wantMultiselect)
            //{
            //    UpdateMultiselector(e);
            //}
        }

        protected override void ProcessEvents(Event e)
        {
            base.ProcessEvents(e);

            if (viewArea.Contains(mousePos))
            {
                #region Left Click
                if (e.button == 0 && dungeon != null)
                {
                    if (e.type == EventType.MouseDown)
                    {
                        if (!dungeon.currentNode)
                        {
                            //wantMultiselect = true;
                            multiselectorOrigin = mousePos;
                        }
                        else
                        {
                            if (dungeon.wantSelect)
                            {
                                if (!dungeon.currentNode.isSelected)
                                {
                                    dungeon.SelectNode(dungeon.currentNode);
                                }
                                else
                                {
                                    dungeon.DeselectNode(dungeon.currentNode);
                                }
                            }
                            else
                            {
                                dungeon.wantMove = true;
                            }
                        }

                        inContext = false;
                    }

                    if (e.type == EventType.MouseDrag)
                    {
                        if (dungeon.selectedNodes.Count == 0 && !inContext)
                        {
                            Move(e);
                        }

                        if (dungeon.wantMove)
                        {
                            foreach (RoomNode node in dungeon.selectedNodes)
                            {
                                if (node != null)
                                {
                                    node.nodeRect.x += e.delta.x / zoom;
                                    node.nodeRect.y += e.delta.y / zoom;
                                }
                            }
                        }
                    }

                    if (e.type == EventType.MouseUp)
                    {
                        //wantMultiselect = false;
                        dungeon.wantMove = false;
                    }
                }
                #endregion Left Click


                #region Right Click
                if (e.button == 1 && dungeon != null)
                {
                    if (e.type == EventType.MouseDown)
                    {
                        if (!dungeon.currentNode)
                        {
                            ProcessContextMenu(e, MenuType.WORK);
                        }
                        else
                        {
                            ProcessContextMenu(e, MenuType.NODE);
                        }

                        dungeon.connectionNode = null;
                        dungeon.wantConnect = false;
                        dungeon.wantDisconnect = false;
                    }
                }
                #endregion Right Click


                #region Mid Click
                if (e.button == 2)
                {
                    if (e.type == EventType.MouseDown)
                    {
                        foreach (RoomNode node in dungeon.nodes)
                        {
                            dungeon.SelectNode(node);
                        }
                    }

                    if (e.type == EventType.MouseDrag)
                    {
                        //Move(e);
                    }

                    if (e.type == EventType.MouseUp)
                    {
                    }
                }
                #endregion Mid Click


                #region Scroll Wheel
                if (e.type == EventType.ScrollWheel)
                {
                    Vector2 screenCoordsMousePos = e.mousePosition;
                    Vector2 delta = e.delta;
                    Vector2 zoomCoordsMousePos = ConvertScreenCoordsToZoomCoords(screenCoordsMousePos);
                    float zoomDelta = -delta.y / 150.0f;
                    float oldZoom = zoom;
                    zoom += zoomDelta * ZOOM_SPEED;
                    zoom = Mathf.Clamp(zoom, MIN_ZOOM, MAX_ZOOM);
                    zoomCoordsOrigin += (zoomCoordsMousePos - zoomCoordsOrigin) - (oldZoom / zoom) * (zoomCoordsMousePos - zoomCoordsOrigin);

                    e.Use();
                }
				#endregion Scrool Wheel
			}
		}
        #endregion Main Methods


        #region Utility Methods
        private void Move(Event e)
		{
            gridOffset.x += e.delta.x / zoom;
            gridOffset.y += e.delta.y / zoom;

            if (dungeon != null)
            {
                foreach (RoomNode node in dungeon.nodes)
                {
                    if (node != null)
                    {
                        node.nodeRect.x += e.delta.x / zoom;
                        node.nodeRect.y += e.delta.y / zoom;
                    }
                }
            }
        }

        private void ProcessContextMenu(Event e, MenuType menuType)
        {
            GenericMenu menu = new GenericMenu();
            inContext = true;

            switch (menuType)
            {
                default:
                case MenuType.NONE:
                    break;

                case MenuType.WORK:
                    if (dungeon != null)
                    {
                        menu.AddItem(new GUIContent("New Room"), false, ContextCallBack, ActionType.NEW_ROOM);
                        RoomsContextMenu(menu);
                        if (copyRoom != null)
                        {
                            menu.AddItem(new GUIContent("Paste"), false, ContextCallBack, ActionType.PASTE_ROOM);
                            menu.AddSeparator("");
                        }
                        menu.AddItem(new GUIContent("Reload"), false, ContextCallBack, ActionType.RELOAD_GRAPH);
                        menu.AddItem(new GUIContent("Empty"), false, ContextCallBack, ActionType.EMPTY_GRAPH);
                        menu.AddSeparator("");
                        menu.AddItem(new GUIContent("Disconnect All"), false, ContextCallBack, ActionType.DISCONNECT_ALL);
                    }
                    break;

                case MenuType.NODE:
                    if (dungeon != null)
                    {
                        menu.AddItem(new GUIContent("Edit"), false, ContextCallBack, ActionType.EDIT_NODE);
                        menu.AddSeparator("");
                        menu.AddItem(new GUIContent("Connect"), false, ContextCallBack, ActionType.CONNECT_NODE);
                        menu.AddItem(new GUIContent("Disconnect"), false, ContextCallBack, ActionType.DISCONNECT_NODE);
                        menu.AddSeparator("");
                        menu.AddItem(new GUIContent("Copy"), false, ContextCallBack, ActionType.COPY_ROOM);
                        menu.AddItem(new GUIContent("Remove"), false, ContextCallBack, ActionType.REMOVE_NODE);
                        menu.AddSeparator("");
                        menu.AddItem(new GUIContent("Isolate"), false, ContextCallBack, ActionType.ISOLATE_NODE);
                    }
                    break;

                case MenuType.CONNECTION:
                    break;
            }

            menu.ShowAsContext();
            e.Use();
        }

        private void ContextCallBack(object actionType)
        {
            switch (actionType)
            {
                #region None
                default:
                case ActionType.NONE:
                    break;
                #endregion None


                #region Reload Graph
                case ActionType.RELOAD_GRAPH:
                    window.Reload();
                    break;
                #endregion Reload Graph


                #region New Room
                case ActionType.NEW_ROOM:
                    RoomWindow.OpenWindow();
                    RoomWindow.window.Load();
                    break;
                #endregion New Room


                #region Copy Room
                case ActionType.COPY_ROOM:
                    copyRoom = dungeon.currentNode.room;
                    break;
                #endregion Copy Room


                #region Paste Room
                case ActionType.PASTE_ROOM:
                    if (copyRoom != null)
                    {
                        CreateNode(copyRoom);
                    }
                    break;
                #endregion Paste Room


                #region Edit Node
                case ActionType.EDIT_NODE:
                    if (dungeon != null)
                    {
                        if (dungeon.currentNode != null)
                        {
                            if (dungeon.currentNode.room != null)
                            {
                                RoomWindow.OpenWindow();
                                RoomWindow.window.Load(dungeon.currentNode.room);
                            }
                        }
                    }
                    break;
                #endregion Edit Node


                #region Connect Node
                case ActionType.CONNECT_NODE:
                    dungeon.wantConnect = true;
                    dungeon.connectionNode = dungeon.currentNode;
                    break;
                #endregion Connect Node


                #region Disconnect Node
                case ActionType.DISCONNECT_NODE:
                    dungeon.wantDisconnect = true;
                    dungeon.connectionNode = dungeon.currentNode;
                    break;
                #endregion Disconnect Node


                #region Isolate Node
                case ActionType.ISOLATE_NODE:
                    if (dungeon != null)
                    {
                        if (dungeon.currentNode != null)
                        {
                            foreach (int connectionID in dungeon.currentNode.connections)
                            {
                                dungeon.GetNode(connectionID).Disconnect(dungeon.currentNode.nodeID);
                            }

                            dungeon.currentNode.connections.Clear();
                        }
                    }
                    break;
                #endregion Isolate Node


                #region Remove Node
                case ActionType.REMOVE_NODE:
                    if (dungeon != null)
                    {
                        if (dungeon.currentNode != null)
                        {
                            foreach (RoomNode node in dungeon.nodes)
                            {
                                if (node.connections.Contains(dungeon.currentNode.nodeID))
                                {
                                    node.Disconnect(dungeon.currentNode.nodeID);
                                }
                            }

                            dungeon.DeselectNode(dungeon.currentNode);
                            dungeon.nodes.Remove(dungeon.currentNode);
                            dungeon.currentNode = null;

                            if (window != null)
                            {
                                window.trash.Add(dungeon.currentNode);
                            }
                        }
                    }
                    break;
                #endregion Remove Node


                #region Empty Graph
                case ActionType.EMPTY_GRAPH:
                    if (dungeon != null)
                    {
                        if (window != null)
                        {
                            foreach (RoomNode node in dungeon.nodes)
                            {
                                window.trash.Add(node);
                            }

                            dungeon.nodes.Clear();
                        }
                    }
                    break;
                #endregion Empty Graph


                #region Disconnect All
                case ActionType.DISCONNECT_ALL:
                    if (dungeon != null)
                    {
                        foreach (RoomNode node in dungeon.nodes)
                        {
                            node.connections.Clear();
                        }
                    }
                    break;
                    #endregion Disconnect All
            }
        }

        private void RoomsContextMenu(GenericMenu menu)
        {
            List<RoomData> rooms = EditorUtils.GetRoomsFromTheDatabase();

            foreach (RoomData room in rooms)
            {
                if (room != null)
                {
                    string path = GetRoomPath(room);

                    menu.AddItem(new GUIContent(path + room.Name), false, CreateNode, room);
                }
            }

            menu.AddSeparator("");
        }

        private string GetRoomPath(RoomData room)
        {
            string path;
            string absolutePath = AssetDatabase.GetAssetPath(room);

            path = absolutePath.Substring(absolutePath.IndexOf(ROOMS_FOLDER));
            path = path.Substring(0, path.LastIndexOf('/'));
            //path = path.Replace(ROOMS_FOLDER, "Add");
            path = EditorUtils.ReplaceFirstOccurrance(path, ROOMS_FOLDER, "Add");

            return path + '/';
        }

        private void CreateNode(object room)
        {
            if (room != null)
            {
                newNode = EditorUtils.CreateNode(dungeon, room as RoomData, mousePos);
            }
        }

        private Vector2 ConvertScreenCoordsToZoomCoords(Vector2 screenCoords)
        {
            return (screenCoords - viewArea.TopLeft()) / zoom + zoomCoordsOrigin;
        }

        private void UpdateMultiselector(Event e)
        {
            Rect multiselectorRect = RectFromDragPoints(multiselectorOrigin, e.mousePosition);

            Texture2D multiselectorTex = new Texture2D(1, 1);
            multiselectorTex.SetPixel(0, 0, EditorColors.MULTISELECTOR_COLOR);
            multiselectorTex.Apply();

            GUI.DrawTexture(multiselectorRect, multiselectorTex);


            foreach (RoomNode node in dungeon.nodes)
            {
                if (node != null)
                {
                    Rect nodeRect = node.nodeRect;
                    Vector2 bl = new Vector2(nodeRect.x, nodeRect.y);
                    Vector2 br = new Vector2(nodeRect.x + nodeRect.width, nodeRect.y);
                    Vector2 tl = new Vector2(nodeRect.x, nodeRect.y + nodeRect.height);
                    Vector2 tr = new Vector2(nodeRect.x + nodeRect.width, nodeRect.y + nodeRect.height);

                    if (multiselectorRect.Contains(bl)
                        || multiselectorRect.Contains(br)
                        || multiselectorRect.Contains(tl)
                        || multiselectorRect.Contains(tr))
                    {
                        dungeon.SelectNode(node);
                    }
                    else
                    {
                        dungeon.DeselectNode(node);
                    }
                }
            }
        }
        private Rect RectFromDragPoints(Vector2 p1, Vector2 p2)
        {
            Vector2 dif = p1 - p2;
            Rect rect = new Rect();

            if (dif.x < 0)
                rect.x = p1.x;
            else
                rect.x = p2.x;
            if (dif.y < 0)
                rect.y = p1.y;
            else
                rect.y = p2.y;

            rect.width = Mathf.Abs(dif.x);
            rect.height = Mathf.Abs(dif.y);

            return rect;
        }

        #endregion Utility Methods
    }
}