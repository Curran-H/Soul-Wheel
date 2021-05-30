using System.Collections.Generic;
using UnityEngine;
using DungeonMaker.Core;

namespace DungeonMaker
{
	public class DungeonObject : MonoBehaviour
	{
		#region Private Variables
		private DungeonData dungeon;
		private RoomObject[,] map;
		private List<RoomObject> rooms;
		private int seed;
		#endregion Private Variables


		#region Properties
		/// <summary>
		/// Name of the DungeonData used to create this dungeon.
		/// </summary>
		public string Name { get { return dungeon.Name; } }

		/// <summary>
		/// It contains all the rooms in the form of a matrix.
		/// </summary>
		public RoomObject[,] Map { get { return map; } }

		/// <summary>
		/// It contains all the rooms in the form of a list.
		/// </summary>
		public List<RoomObject> Rooms { get { return rooms; } }

		/// <summary>
		/// Seed that has been used for the generation of this dungeon.
		/// </summary>
		public int Seed { get { return seed; } }

		/// <summary>
		/// Width of the dungeon map.
		/// </summary>
		public int Width { get { return map.GetLength(1); } }

		/// <summary>
		/// Height of the dungeon map.
		/// </summary>
		public int Height { get { return map.GetLength(0); } }
		#endregion Properties


		#region Constructor
		public void Init(DungeonData d, RoomObject[,] m, int s)
		{
			dungeon = d;
			map = m;
			seed = s;

			SetRooms();
		}
		#endregion Constructor


		#region Main Methods
		private void SetRooms()
		{
			rooms = new List<RoomObject>();

			for (int y = 0; y < Height; y++)
				for (int x = 0; x < Width; x++)
					if (!IsNull(x, y))
						rooms.Add(Get(x, y));
		}
		#endregion Main Methods


		#region Utility Methods
		/// <summary>
		/// Returns information about the room of the specified position.
		/// </summary>
		public RoomObject Get(int x, int y)
		{
			return map[y, x];
		}

		/// <summary>
		/// Returns room information for the specified identifier.
		/// </summary>
		public RoomObject Get(int id)
		{
			foreach (RoomObject r in rooms)
				if (r.ID == id)
					return r;

			return null;
		}

		/// <summary>
		/// Returns true if no room is found in the specified position.
		/// </summary>
		public bool IsNull(int x, int y)
		{
			return map[y, x] == null;
		}

		/// <summary>
		/// Returns true if the specified position is within the boundaries of the dungeon map.
		/// </summary>
		public bool InBounds(int x, int y)
		{
			return x >= 0 && y >= 0
				&& x < Width && y < Height;
		}

		/// <summary>
		/// Scroll the width and height of the dungeon map and apply an action to each position.
		/// </summary>
		public void Process(XYDelegate function)
		{
			for (int y = 0; y < Height; y++)
				for (int x = 0; x < Width; x++)
					function(x, y);
		}

		/// <summary>
		/// Prints a graphic representation of the dungeon map on the console.
		/// </summary>
		public void Print(string full = "X", string empty = " . ")
		{
			string result = string.Format("{0}\n", Name);

			for (int y = Height - 1; y >= 0; y--)
			{
				for (int x = 0; x < Width; x++)
				{
					result += string.Format("{0} ", IsNull(x, y) ? empty : full);
				}

				result.Remove(result.Length - 1);
				result += '\n';
			}

			Debug.Log(result);
		}
		#endregion Utility Methods
	}
}