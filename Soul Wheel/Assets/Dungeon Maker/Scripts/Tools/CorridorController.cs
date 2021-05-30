using UnityEngine;

namespace DungeonMaker.Demo
{
	[ExecuteInEditMode]
    public class CorridorController : MonoBehaviour
    {
		#region Public Variables
		[Tooltip("Apply the controller automatically at the beginning.")]
		public bool auto = true;

		[Tooltip("Simple corridor shape.")]
		public CorridorType type;
		#endregion Public Variables


		#region Private Variables
		private RoomObject room;
		private bool applied;
		#endregion Private Variables


		#region Properties
		/// <summary>
		/// Room to which this rule belongs.
		/// </summary>
		public RoomObject Room { get { return room; } }
		#endregion Properties


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

			room = GetComponent<RoomObject>();

			if (auto)
			{
				Apply();
			}
		}

		/// <summary>
		/// Rotate the corridor to match the surrounding rooms.
		/// </summary>
		public void Apply()
		{
			switch (type)
			{
				case CorridorType.I:
					if (room.Type == RoomType.LR) Rotate(new Vector3(0f, 90f, 0f));
					break;
				case CorridorType.L:
					if (room.Type == RoomType.TL) Rotate(new Vector3(0f, -90f, 0f));
					if (room.Type == RoomType.BL) Rotate(new Vector3(0f, 180f, 0f));
					if (room.Type == RoomType.BR) Rotate(new Vector3(0f, 90f, 0f));
					break;
				case CorridorType.T:
					if (room.Type == RoomType.TBL) Rotate(new Vector3(0f, 90f, 0f));
					if (room.Type == RoomType.TBR) Rotate(new Vector3(0f, -90f, 0f));
					if (room.Type == RoomType.TLR) Rotate(new Vector3(0f, 180f, 0f));
					break;
				case CorridorType.X:
					break;
			}
		}
		#endregion Main Methods


		#region Utility Methods
		private void Rotate(Vector3 rotation)
		{
			transform.rotation = Quaternion.Euler(rotation);
		}
		#endregion Utility Methods
	}
}