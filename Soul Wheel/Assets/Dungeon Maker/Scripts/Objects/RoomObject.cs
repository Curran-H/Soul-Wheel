using System.Collections.Generic;
using UnityEngine;
using DungeonMaker.Core;

namespace DungeonMaker
{
	public class RoomObject : MonoBehaviour
	{
		#region Private Variables
		private RoomData room;
		private DungeonObject dungeon;
		private List<int> connections;
		private int x, y;
		private int id;
		private RoomType type;
		#endregion Private Variables


		#region Properties
		/// <summary>
		/// Dungeon this room belongs to.
		/// </summary>
		public DungeonObject Dungeon { get { return dungeon; } }

		/// <summary>
		/// It contains the identifiers of all the rooms that are connected to this room.
		/// </summary>
		public List<int> Connections { get { return connections; } }

		/// <summary>
		/// Name of the RoomData used to create this room.
		/// </summary>
		public string Name { get { return room.Name; } }

		/// <summary>
		/// X position that the room occupies on the map of the dungeon.
		/// </summary>
		public int X { get { return x; } }

		/// <summary>
		/// Y position that the room occupies on the map of the dungeon.
		/// </summary>
		public int Y { get { return y; } }

		/// <summary>
		/// Room identifier.
		/// </summary>
		public int ID { get { return id; } }

		/// <summary>
		/// Type of shape the room has acquired when it has been generated.
		/// </summary>
		public RoomType Type { get { return type; } }

		public List<GameObject> TBLR { get { return room.rooms.TBLR; } }
		public List<GameObject> TB { get { return room.rooms.TB; } }
		public List<GameObject> LR { get { return room.rooms.LR; } }
		public List<GameObject> T { get { return room.rooms.T; } }
		public List<GameObject> B { get { return room.rooms.B; } }
		public List<GameObject> L { get { return room.rooms.L; } }
		public List<GameObject> R { get { return room.rooms.R; } }
		public List<GameObject> TL { get { return room.rooms.TL; } }
		public List<GameObject> TR { get { return room.rooms.TR; } }
		public List<GameObject> BL { get { return room.rooms.BL; } }
		public List<GameObject> BR { get { return room.rooms.BR; } }
		public List<GameObject> TBL { get { return room.rooms.TBL; } }
		public List<GameObject> TBR { get { return room.rooms.TBR; } }
		public List<GameObject> TLR { get { return room.rooms.TLR; } }
		public List<GameObject> BLR { get { return room.rooms.BLR; } }
		#endregion Properties


		#region Constructor
		public void Init(DungeonObject d, RoomNode n, RoomType t, int x, int y)
		{
			dungeon = d;
			room = n.room;
			id = n.nodeID;
			connections = n.connections;
			type = t;
			this.x = x;
			this.y = y;
		}
		#endregion Constructor


		#region Utility Methods
		/// <summary>
		/// Returns true if the specified room is in the connections of this room.
		/// </summary>
		public bool IsConnectedTo(RoomObject room)
		{
			return connections.Contains(room.ID);
		}

		/// <summary>
		/// Returns true if the specified identifier is found in the connections of this room.
		/// </summary>
		public bool IsConnectedTo(int id)
		{
			return connections.Contains(id);
		}

		/// <summary>
		/// Returns the list of objects corresponding to the specified room type.
		/// </summary>
		public List<GameObject> GetSet(RoomType type)
		{
			return room.rooms.GetSet(type);
		}
		#endregion Utiliy Methods
	}
}