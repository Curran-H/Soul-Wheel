using UnityEngine;

namespace DungeonMaker
{
	public class Framework : MonoBehaviour
	{
		#region Public Variables
		[Tooltip("Show or hide the gizmo.")]
		public bool show = true;

		[Tooltip("Size of the gizmo in width (X), height (Y) and length (Z).")]
		public Vector3 size = Vector3.one;

		[Tooltip("Color of the gizmo.")]
		public Color color = Color.white;
		#endregion Public Variables


		#region Unity Methods
#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			if (!show) return;

			Color c = Gizmos.color;

			Gizmos.color = color;
			Gizmos.DrawWireCube(transform.position, size);
			Gizmos.color = c;
		}
#endif
		#endregion Unity Methods
	}
}