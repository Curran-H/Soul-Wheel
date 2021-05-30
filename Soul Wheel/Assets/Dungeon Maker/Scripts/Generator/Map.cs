using System.Collections.Generic;
using C = UnityEngine.Vector2Int;
using D = DungeonMaker.Core.DungeonData;
using N = DungeonMaker.Core.RoomNode;

namespace DungeonMaker.Core
{
	public class Map
	{
		#region Public Variables
		public D dungeon;
		public N[,] matrix;
		public bool completed;
		#endregion Public Variables


		#region Private Variables
		private List<N> nodes;
		private List<C> collapsed;
		private C[] directions = { C.left, C.up, C.right, C.down };
		#endregion Private Variables


		#region Properties
		private N FirstNode
		{
			get
			{
				N result = null;
				int max = 0;
				int count;

				foreach (N n in nodes)
				{
					count = n.connections.Count;

					if (count >= max)
					{
						result = n;
						max = count;
					}
				}

				return result;
			}
		}
		private bool IsCompleted
		{
			get
			{
				for (int y = 0; y < matrix.GetLength(0); y++)
				{
					for (int x = 0; x < matrix.GetLength(1); x++)
					{
						C c = new C(x, y);

						if (!IsNull(c) && !Match(c, Get(c)))
							return false;
					}
				}

				return true;
			}
		}
		#endregion Properties


		#region Constructor
		public Map(D d)
		{
			if (d == null) return;

			dungeon = d;
			nodes = GeneratorUtils.Copy(d.nodes);

			int size = nodes.Count;
			matrix = new N[size * 2, size * 2];
			collapsed = new List<C>();

			Collapse(new C(size, size), FirstNode);

			int it = 0;
			do
			{
				Process(collapsed, Propagate);
				it++;

			} while (nodes.Count > 0 && it < size * 2);

			if (completed = IsCompleted)
			{
				matrix = Compressor.Compress(matrix);
			}
		}
		#endregion Constructor


		#region Main Methods
		private void Propagate(C c)
		{
			N n = Get(c);

			if (n == null) return;

			foreach (int id in n.connections)
			{
				Calculate(c, dungeon.GetNode(id));
			}
		}

		private void Calculate(C c, N n)
		{
			if (!nodes.Contains(n)) return;

			C c2 = c;
			int accuracy = 0;

			GeneratorUtils.Shuffle(directions);
			foreach (C dir in directions)
			{
				C p = c + dir;

				if (InBounds(p) && IsNull(p))
				{
					int a = Accuracy(p, n);

					if (a > accuracy)
					{
						accuracy = a;
						c2 = p;
					}
				}
			}

			if (c2 != c) Collapse(c2, n);
		}
		#endregion Main Methods


		#region Utility Methods
		private N Get(C c)
		{
			return matrix[c.y, c.x];
		}
		private void Set(C c, N n)
		{
			matrix[c.y, c.x] = n;
		}
		private void Collapse(C c, N n)
		{
			Set(c, n);
			collapsed.Add(c);
			nodes.Remove(n);
		}
		private void Process(List<C> cs, CoordinatesDelegate function)
		{
			for (int i = 0; i < cs.Count; i++)
				function(cs[i]);
		}
		private int Accuracy(C c, N n)
		{
			int result = 0;

			foreach (C dir in directions)
			{
				C p = c + dir;

				if (InBounds(p) && IsCollapsed(p))
				{
					if (n.connections.Contains(Get(p).nodeID))
						result++;
				}
			}

			return result;
		}
		private bool IsNull(C c)
		{
			return matrix[c.y, c.x] == null;
		}
		private bool IsCollapsed(C c)
		{
			return collapsed.Contains(c);
		}
		private bool InBounds(C c)
		{
			return GeneratorUtils.InBounds(ref matrix, c);
		}
		private bool Match(C c, N n)
		{
			int count = 0;

			foreach (C dir in directions)
			{
				C p = c + dir;

				if (InBounds(p))
				{
					N n2 = Get(p);

					if (n2 != null)
					{
						foreach (int id in n.connections)
						{
							if (n2.nodeID == id)
								count++;
						}
					}
				}
			}

			return count == n.connections.Count;
		}
		#endregion Utility Methods
	}
}