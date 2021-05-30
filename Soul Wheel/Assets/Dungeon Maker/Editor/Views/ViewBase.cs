using System;
using UnityEngine;
using DungeonMaker.Core;

namespace DungeonMaker.Editor
{
    [Serializable]
    public class ViewBase
    {
        #region Public Variables
        public string title;
        public Rect viewArea;
        #endregion Public Variables


        #region Protected Variables
        protected DungeonWindow window;
        protected DungeonData dungeon;
        protected GUISkin skin;
        #endregion Protected Variables


        #region Constructors
        public ViewBase(string title)
        {
            this.title = title;

            GetEditorSkin();
        }
        #endregion Constructors


        #region Main Methods
        public virtual void UpdateView(Rect editorRect, Rect percentageRect, Event e, DungeonData dungeon, DungeonWindow window)
        {
            if (skin == null)
            {
                GetEditorSkin();
                return;
            }

            this.window = window;
            this.dungeon = dungeon;

            if (dungeon != null)
            {
                title = dungeon.Name;
            }
            else
            {
                title = "No Graph";
            }

            viewArea = new Rect(editorRect.x * percentageRect.x,
                                editorRect.y * percentageRect.y,
                                editorRect.width * percentageRect.width,
                                editorRect.height * percentageRect.height);
        }

        protected virtual void ProcessEvents(Event e) { }
        #endregion Main Methods


        #region Utility Methods
        protected virtual void GetEditorSkin()
        {
            skin = (GUISkin)Resources.Load(DungeonWindow.DUNGEON_SKIN_PATH);
        }
        #endregion Utility Methods
    }
}