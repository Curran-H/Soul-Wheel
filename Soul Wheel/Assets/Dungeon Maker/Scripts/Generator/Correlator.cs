using System.Threading;
using Math = UnityEngine.Mathf;
using D = DungeonMaker.Core.DungeonData;
using M = DungeonMaker.Core.Map;

namespace DungeonMaker.Core
{
	public class Correlator
	{
		#region Private Variables
		private Thread thread;
		private Generator generator;
		private D dungeon;
		private M map;
		private CorrelatorState state;
		private bool multithreading;
		#endregion Private Variables


		#region Properties
		public CorrelatorState State { get { return state; } }
		#endregion Properties


		#region Main Methods
		public void Run(D dungeon, bool multithreading, Generator generator = null)
		{
			if (!ChangeState(CorrelatorState.RUNNING))
				return;

			this.dungeon = dungeon;
			this.generator = generator;
			this.multithreading = multithreading;

			if (multithreading)
			{
				thread = new Thread(Correlate);
				thread.Start();
			}
			else
			{
				Correlate();
			}
		}

		public void Abort()
		{
			if (!ChangeState(CorrelatorState.DONE))
				return;

			if (thread != null && multithreading)
			{
				thread.Abort();
			}
		}

		public M Extract()
		{
			if (!ChangeState(CorrelatorState.STOPPED))
				return null;

			return map;
		}

		private void Correlate()
		{
			int it = 0;
			int max = (int)Math.Pow(dungeon.nodes.Count, 2);

			do
			{
				if (generator != null)
					generator.SetSeed();

				map = new M(dungeon);
				it++;

			} while (!map.completed && it < max);

			Abort();
		}
		#endregion Main Methods


		#region Utility Methods
		private bool ChangeState(CorrelatorState s)
		{
			if (s == state) 
				return false;

			state = s;

			return true;
		}
		#endregion Utility Methods
	}
}