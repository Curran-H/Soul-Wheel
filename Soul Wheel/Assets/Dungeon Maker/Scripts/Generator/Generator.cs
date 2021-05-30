using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using DungeonMaker.Core;
using Random = System.Random;
using C = UnityEngine.Vector2Int;
using D = DungeonMaker.Core.DungeonData;
using N = DungeonMaker.Core.RoomNode;

namespace DungeonMaker
{
	[ExecuteInEditMode]
	public class Generator : MonoBehaviour
	{
		#region Static Variables
		private static Random random = new Random();
		#endregion Static Variales


		#region Public Variables
		public bool auto = true;
		public bool multithreading = true;
		public bool staticSeed;
		public int seed;
		public ModeType mode;
		public Vector2 spacing;
		[Tooltip("Set of dungeons that can be used.")]
		public List<D> dungeons = new List<D>();
		#endregion Public Variables


		#region Private Variables
		private Correlator correlator;
		private Transform parent;
		#endregion Private Variables


		#region Events
		public static event VoidDelegate OnGeneratorStart;
		public static event DungeonObjectDelegate OnGeneratorFinish;
		#endregion Events


		#region Properties
		public static Random Random
		{
			get { return random; }
		}
		public bool Generating
		{
			get { return correlator != null && correlator.State == CorrelatorState.RUNNING; }
		}
		private D RandomDungeon
		{
			get
			{
				if (dungeons.Count == 0) return null;

				return dungeons[Random.Next(dungeons.Count)];
			}
		}
		#endregion Properties


		#region Unity Methods
		private void OnEnable()
		{
#if UNITY_EDITOR
			EditorApplication.update += Verify;
#endif

			correlator = new Correlator();
		}

		private void OnDisable()
		{
#if UNITY_EDITOR
			EditorApplication.update -= Verify;
#endif

			if (correlator != null)
				correlator.Abort();
		}

		private void OnDestroy()
		{
			if (correlator != null)
				correlator.Abort();
		}

		private void OnApplicationQuit()
		{
			if (correlator != null)
				correlator.Abort();
		}

		private void Start()
		{
			if (auto && Application.isPlaying)
			{
				Generate();
			}
		}

		private void Update()
		{
			Verify();
		}
		#endregion Unity Methods


		#region Main Methods
		public void Generate(D dungeon = null)
		{
			if (correlator.State == CorrelatorState.RUNNING) return;

			SetSeed();

			D d = dungeon != null ? dungeon : RandomDungeon;

			if (d == null)
			{
				Debug.LogError("<b>Dungeon Maker</b>\nGenerator: No valid dungeon found.");
				return;
			}

			if (d.nodes.Count < 2)
			{
				string problem = d.nodes.Count == 0 ? " empty." : " too small.";

				Debug.LogError("<b>Dungeon Maker</b>\nGenerator: (" + d.Name + ") The dungeon is" + problem);
				return;
			}

			correlator.Run(d, multithreading);

			LaunchOnGeneratorStartEvent();
		}

		private void Verify()
		{
			if (correlator != null && correlator.State == CorrelatorState.DONE)
			{
				Map m = correlator.Extract();

				if (m != null && m.completed)
				{
					Destroy();
					Build(m);
				}
				else Debug.LogError("<b>Dungeon Maker</b>\nGenerator: (" + m.dungeon.Name + ") The dungeon could not be generated.");

				LaunchOnGeneratorFinishEvent();
			}
		}

		private void Build(Map map)
		{
			N[,] matrix = map.matrix;
			RoomObject[,] m = new RoomObject[matrix.GetLength(0), matrix.GetLength(1)];

			parent = new GameObject(map.dungeon.Name).transform;

			DungeonObject dungeon = AddDungeonObject(parent.gameObject, map.dungeon, m);

			for (int y = 0; y < matrix.GetLength(0); y++)
			{
				for (int x = 0; x < matrix.GetLength(1); x++)
				{
					N n = matrix[y, x];

					if (n == null) continue;

					C c = new C(x, y);
					RoomType t = GeneratorUtils.GetType(c, ref map.matrix);
					List<GameObject> os = n.room.rooms.GetSet(t);
					GameObject o = GeneratorUtils.RandomObject(os);

					if (o == null)
					{
						os = n.room.rooms.GetSet(RoomType.NONE);
						o = GeneratorUtils.RandomObject(os);
					}

					if (o != null)
					{
						GameObject r = Create(c, n, o);
						RoomObject room = AddRoomObject(r, dungeon, n, t, c);

						m[y, x] = room;
					}
					else
					{
						Debug.LogError("<b>Dungeon Maker</b>\nGenerator: (" + n.room.Name + ") The room format is incorrect.");
						return;
					}
				}
			}

			AddDungeonObject(parent.gameObject, map.dungeon, m);
		}

		public void Destroy(bool editor = false)
		{
			if (parent != null)
			{
				DestroyImmediate(parent.gameObject);
			}
			else if (editor)
			{
				Debug.LogWarning("<b>Dungeon Maker</b>\nGenerator: The reference to the dungeon has been lost.");
			}
		}
		#endregion Main Methods


		#region Utility Methods
		private GameObject Create(C c, N n, GameObject o)
		{
			Vector3 p = mode == ModeType._2D
				? new Vector3(c.x * spacing.x, c.y * spacing.y, 0f)
				: new Vector3(c.x * spacing.x, 0f, c.y * spacing.y);

			GameObject r = Instantiate(o, p, Quaternion.identity);
			r.transform.SetParent(parent);
			r.name = n.room.name.ToString();

			return r;
		}
		private DungeonObject AddDungeonObject(GameObject o, D d, RoomObject[,] m)
		{
			DungeonObject dungeonObject = o.GetComponent<DungeonObject>();

			if (dungeonObject == null)
			{
				dungeonObject = o.AddComponent<DungeonObject>();
			}

			dungeonObject.Init(d, m, seed);

			return dungeonObject;
		}
		private RoomObject AddRoomObject(GameObject o, DungeonObject d, N n, RoomType t, C c)
		{
			RoomObject roomObject = o.GetComponent<RoomObject>();

			if (roomObject == null)
			{
				roomObject = o.AddComponent<RoomObject>();
			}

			roomObject.Init(d, n, t, c.x, c.y);

			return roomObject;
		}
		public void SetSeed()
		{
			if (!staticSeed)
				seed = random.Next(int.MinValue, int.MaxValue);

			random = new Random(seed);
			UnityEngine.Random.InitState(seed);
		}
		private void LaunchOnGeneratorStartEvent()
		{
			if (OnGeneratorStart != null)
			{
				OnGeneratorStart();

#if UNITY_EDITOR
				AssetDatabase.Refresh();
#endif
			}
		}
		private void LaunchOnGeneratorFinishEvent()
		{
			if (OnGeneratorFinish != null)
			{
				DungeonObject dungeonObject = parent == null 
					? null 
					: parent.GetComponent<DungeonObject>();

				OnGeneratorFinish(dungeonObject);

#if UNITY_EDITOR
				AssetDatabase.Refresh();
#endif
			}
		}
		#endregion Utility Methods
	}
}