using System.Collections.Generic;
using UnityEngine;
using C = UnityEngine.Vector2Int;
using N = DungeonMaker.Core.RoomNode;

namespace DungeonMaker.Core
{
	public static class GeneratorUtils
	{
		#region Static Variables
		public static C up = C.up;
		public static C down = C.down;
		public static C left = C.left;
		public static C right = C.right;
		#endregion Static Variables


		#region Utility Methods
		public static void Shuffle<T>(IList<T> t)
		{
			var n = t.Count;
			var rnd = Generator.Random;

			for (int i = n - 1; i > 0; i--)
			{
				var j = rnd.Next(0, i);
				var tmp = t[i];

				t[i] = t[j];
				t[j] = tmp;
			}
		}
		public static bool InBounds<T>(ref T[,] t, C c)
		{
			return c.y >= 0 && c.x >= 0
				&& c.y < t.GetLength(0)
				&& c.x < t.GetLength(1);
		}
		public static bool IsPrefab(GameObject o)
		{
			return o.gameObject.scene.name == null;
		}
		public static List<N> Copy(List<N> ns)
		{
			List<N> result = new List<N>();

			foreach (N n in ns)
			{
				result.Add(n);
			}

			return result;
		}
		public static RoomType GetType(C c, ref N[,] m)
		{
			int id = m[c.y, c.x].nodeID;
			List<C> cs = new List<C>();
			C[] directions =
			{
				up, down, left, right
			};

			foreach (C dir in directions)
			{
				C p = c + dir;

				if (InBounds(ref m, p))
				{
					N n = m[p.y, p.x];

					if (n != null && n.connections.Contains(id) && n.room.Name != "Void")
					{
						cs.Add(dir);
					}
				}
			}

			return GetType(cs);
		}
		private static RoomType GetType(List<C> cs)
		{
			RoomType result = RoomType.NONE;

			if (cs.Count == 4)
			{
				if (cs.Contains(up) && cs.Contains(down) && cs.Contains(left) && cs.Contains(right)) result = RoomType.TBLR;
			}
			else if (cs.Count == 3)
			{
				if (cs.Contains(up) && cs.Contains(down) && cs.Contains(left)) result = RoomType.TBL;
				else if (cs.Contains(up) && cs.Contains(down) && cs.Contains(right)) result = RoomType.TBR;
				else if (cs.Contains(up) && cs.Contains(left) && cs.Contains(right)) result = RoomType.TLR;
				else if (cs.Contains(down) && cs.Contains(left) && cs.Contains(right)) result = RoomType.BLR;
			}
			else if (cs.Count == 2)
			{
				if (cs.Contains(up) && cs.Contains(down)) result = RoomType.TB;
				else if (cs.Contains(left) && cs.Contains(right)) result = RoomType.LR;
				else if (cs.Contains(up) && cs.Contains(left)) result = RoomType.TL;
				else if (cs.Contains(up) && cs.Contains(right)) result = RoomType.TR;
				else if (cs.Contains(down) && cs.Contains(left)) result = RoomType.BL;
				else if (cs.Contains(down) && cs.Contains(right)) result = RoomType.BR;
			}
			else if (cs.Count == 1)
			{
				if (cs.Contains(up)) result = RoomType.T;
				else if (cs.Contains(down)) result = RoomType.B;
				else if (cs.Contains(left)) result = RoomType.L;
				else if (cs.Contains(right)) result = RoomType.R;
			}

			return result;
		}
		public static GameObject RandomObject(List<GameObject> os)
		{
			if (os.Count == 0) return null;

			return os[Generator.Random.Next(os.Count)];
		}
		#endregion Utility Methods
	}
}