using UnityEngine;

namespace DungeonMaker.Core
{
    public class NodeBase : ScriptableObject
    {
        #region Public Variables
        [HideInInspector]
        public bool isSelected = false;
        public int nodeID;
        public NodeType nodeType;
        [HideInInspector]
        public Rect nodeRect;
        public DungeonData dungeon;
        #endregion Public Variables


        #region Protected Variables
        protected GUISkin skin;
        #endregion Protected Variables


        #region Main Methods
        public virtual void InitNode() { }

        public virtual void UpdateNode(Event e, Rect viewRect)
        {
            ProcessEvent(e, viewRect);
        }

#if UNITY_EDITOR
        public virtual void UpdateNodeGUI(Event e, Rect viewRect, GUISkin viewSkin)
        {
            ProcessEvent(e, viewRect);
        }

        public virtual void DrawNodeProperties() { }
#endif

        protected virtual void ProcessEvent(Event e, Rect viewRect) { }
        #endregion Main Methods
    }
}