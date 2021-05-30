using UnityEngine;

namespace DungeonMaker.Demo
{
	[ExecuteInEditMode]
	public class RandomRotation : MonoBehaviour
	{
		public float range;

		private void OnEnable()
		{
			Generator.OnGeneratorFinish += Init;
		}

		private void OnDisable()
		{
			Generator.OnGeneratorFinish -= Init;
		}

		private void Init(DungeonObject d)
		{
			transform.rotation = Quaternion.Euler(Vector3.up * Random.Range(-range, range));
		}
	}
}