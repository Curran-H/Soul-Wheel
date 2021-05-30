using System.Collections.Generic;
using GameObject = UnityEngine.GameObject;

namespace DungeonMaker
{
	[System.Serializable]
    public class RoomSet
    {
		#region Public Variables
		public List<GameObject> DEFAULT	= new List<GameObject>();
		public List<GameObject> TBLR	= new List<GameObject>();
        public List<GameObject> TB		= new List<GameObject>();
        public List<GameObject> LR		= new List<GameObject>();
        public List<GameObject> T		= new List<GameObject>();
        public List<GameObject> B		= new List<GameObject>();
        public List<GameObject> L		= new List<GameObject>();
        public List<GameObject> R		= new List<GameObject>();
        public List<GameObject> TL		= new List<GameObject>();
        public List<GameObject> TR		= new List<GameObject>();
        public List<GameObject> BL		= new List<GameObject>();
        public List<GameObject> BR		= new List<GameObject>();
        public List<GameObject> TBL		= new List<GameObject>();
        public List<GameObject> TBR		= new List<GameObject>();
        public List<GameObject> TLR		= new List<GameObject>();
        public List<GameObject> BLR		= new List<GameObject>();
		#endregion Public Variables


		#region Utility Methods
		public List<GameObject> GetSet(RoomType t)
		{
			switch (t)
			{
				default:
				case RoomType.NONE: return DEFAULT;
				case RoomType.TBLR:	return TBLR;
				case RoomType.TB:	return TB;
				case RoomType.LR:	return LR;
				case RoomType.T:	return T;
				case RoomType.B:	return B;
				case RoomType.L:	return L;
				case RoomType.R:	return R;
				case RoomType.TL:	return TL;
				case RoomType.TR:	return TR;
				case RoomType.BL:	return BL;
				case RoomType.BR:	return BR;
				case RoomType.TBL:	return TBL;
				case RoomType.TBR:	return TBR;
				case RoomType.TLR:	return TLR;
				case RoomType.BLR:	return BLR;
			}
		}
		#endregion Utility Methods
	}
}