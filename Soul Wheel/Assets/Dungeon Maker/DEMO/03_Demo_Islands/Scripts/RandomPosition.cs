using UnityEngine;

namespace DungeonMaker.Demo
{
	[ExecuteInEditMode]
    public class RandomPosition : MonoBehaviour
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
			Vector3 pos = transform.position;
			float x = pos.x + Random.Range(0f, range);
			float y = pos.y;
			float z = pos.z + Random.Range(0f, range);

			transform.position = new Vector3(x, y, z);
		}
	}
}