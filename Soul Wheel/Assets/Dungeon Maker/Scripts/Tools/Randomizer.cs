using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Utils = DungeonMaker.Core.GeneratorUtils;

namespace DungeonMaker
{
	[ExecuteInEditMode]
	public class Randomizer : MonoBehaviour
	{
		#region Public Variables
		[Tooltip("Apply the randomizer automatically at the beginning.")]
		public bool auto = true;

		[Tooltip("Destroys the residual elements when finished.")]
		public bool destroy;

		[Tooltip("Set of objects that can be used.")]
		public GameObject[] objects;
		#endregion Public Variables


		#region Private Variables
		private bool applied;
		private GameObject result;
		#endregion Private Variables


		#region Properties
		/// <summary>
		/// Object selected by the randomizer.
		/// </summary>
		public GameObject Result { get { return result; } }
		#endregion Properties;


		#region Unity Methods
		private void OnEnable()
		{
			Generator.OnGeneratorFinish += Init;
		}

		private void OnDisable()
		{
			Generator.OnGeneratorFinish -= Init;
		}
		#endregion Unity Methods


		#region Main Methods
		private void Init(DungeonObject d)
		{
			if (applied) return;

			applied = true;

			if (auto)
			{
				Apply();
			}
		}

		/// <summary>
		/// Selects a random object.
		/// </summary>
		public void Apply()
		{
			if (objects == null || objects.Length == 0) return;

			GameObject obj = objects[Generator.Random.Next(objects.Length)];

			if (obj == null) return;

			if (Utils.IsPrefab(obj))
			{
				result = Instantiate(obj, transform.position, Quaternion.identity, transform);
			}
			else
			{
				result = obj;

				obj.SetActive(true);
			}

			if (destroy)
			{
				foreach (GameObject o in objects)
				{
					if (o != null && o != obj && !Utils.IsPrefab(o))
					{
						if (Application.isPlaying)
						{
							Destroy(o);
						}
						else
						{
#if UNITY_EDITOR
							if (PrefabUtility.IsPartOfAnyPrefab(o))
							{
								PrefabUtility.UnpackPrefabInstance(gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
							}
#endif

							DestroyImmediate(o);
						}
					}
				}

				objects = new GameObject[1];
				objects[0] = obj;
			}
		}
		#endregion Main Methods
	}
}