using UnityEngine;

namespace DungeonMaker.Core
{
    [CreateAssetMenu(fileName = DEFAULT_ROOM_NAME, menuName = "Dungeon Maker/New Room")]
    public class RoomData : ScriptableObject
    {
        #region Const Variables
        public const string DEFAULT_ROOM_NAME = "New Room";
        public static Color DEFAULT_ROOM_COLOR = Color.white;
		#endregion Const Variables


		#region Public Variables
		//public string roomName = DEFAULT_ROOM_NAME;
		public Texture2D icon;
        public Color color = DEFAULT_ROOM_COLOR;
        public RoomSet rooms = new RoomSet();
        #endregion Public Variables


        #region Properties
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        #endregion Properties
    }
}