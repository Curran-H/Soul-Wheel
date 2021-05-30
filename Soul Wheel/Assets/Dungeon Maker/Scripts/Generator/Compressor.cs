using N = DungeonMaker.Core.RoomNode;

namespace DungeonMaker.Core
{
	public class Compressor
	{
		#region Private Variables
		private static int minY, minX, maxY, maxX;
		#endregion Private Variables


		#region Main Methods
		public static N[,] Compress(N[,] m)
		{
			Init(m);
			Transpose(ref m);

			return Compact(ref m);
		}

		private static void Init(N[,] m)
		{
			minY = int.MaxValue;
			minX = int.MaxValue;
			maxY = 0;
			maxX = 0;

			for (int y = 0; y < m.GetLength(0); y++)
			{
				for (int x = 0; x < m.GetLength(1); x++)
				{
					if (y < minY && m[y, x] != null)
						minY = y;

					if (y > maxY && m[y, x] != null)
						maxY = y;

					if (x < minX && m[y, x] != null)
						minX = x;

					if (x > maxX && m[y, x] != null)
						maxX = x;
				}
			}

			maxY -= minY - 1;
			maxX -= minX - 1;
		}

		private static void Transpose(ref N[,] m)
		{
			for (int y = 0; y < m.GetLength(0); y++)
			{
				for (int x = 0; x < m.GetLength(1); x++)
				{
					int newY = y - minY;
					int newX = x - minX;

					if (newY < 0) newY = 0;
					if (newX < 0) newX = 0;

					if (newY != y && newX != x)
					{
						m[newY, newX] = m[y, x];
						m[y, x] = null;
					}
				}
			}
		}

		private static N[,] Compact(ref N[,] m)
		{
			N[,] result = new N[maxY, maxX];

			for (int y = 0; y < result.GetLength(0); y++)
			{
				for (int x = 0; x < result.GetLength(1); x++)
				{
					result[y, x] =
						m[y, x] != null
						? m[y, x]
						: null;
				}
			}

			return result;
		}
		#endregion Main Methods
	}
}