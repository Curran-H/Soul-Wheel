using UnityEngine;

namespace DungeonMaker
{
	[ExecuteInEditMode]
	public class Rule : MonoBehaviour
	{
		#region Public Variables
		[Tooltip("Apply the rule automatically at the beginning.")]
		public bool auto = true;

		[Tooltip("Destroys the residual elements when finished.")]
		public bool destroy;

		[Tooltip("Logic that determines the operation of the rule.")]
		public RuleType rule;
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

			room = FindRoom();

			if (auto)
			{
				Apply();
			}
		}

		/// <summary>
		/// Activate or deactivate the object according to the rule and the position of the room in the dungeon.
		/// </summary>
		public void Apply(RoomType type = RoomType.NONE)
		{
			if (type == RoomType.NONE)
			{
				if (room != null)
				{
					type = room.Type;
				}

				if (room == null || type == RoomType.NONE)
					return;
			}

			switch (rule)
			{
				case RuleType.TOP:
					if (type == RoomType.TBLR
						|| type == RoomType.TB
						|| type == RoomType.T
						|| type == RoomType.TL
						|| type == RoomType.TR
						|| type == RoomType.TBL
						|| type == RoomType.TBR
						|| type == RoomType.TLR)
					{
						SetState();
						return;
					}
					break;
				case RuleType.NOT_TOP:
					if (type != RoomType.TBLR
						&& type != RoomType.TB
						&& type != RoomType.T
						&& type != RoomType.TL
						&& type != RoomType.TR
						&& type != RoomType.TBL
						&& type != RoomType.TBR
						&& type != RoomType.TLR)
					{
						SetState();
						return;
					}
					break;
				case RuleType.BOTTOM:
					if (type == RoomType.TBLR
						|| type == RoomType.TB
						|| type == RoomType.B
						|| type == RoomType.BL
						|| type == RoomType.BR
						|| type == RoomType.TBL
						|| type == RoomType.TBR
						|| type == RoomType.BLR)
					{
						SetState();
						return;
					}
					break;
				case RuleType.NOT_BOTTOM:
					if (type != RoomType.TBLR
						&& type != RoomType.TB
						&& type != RoomType.B
						&& type != RoomType.BL
						&& type != RoomType.BR
						&& type != RoomType.TBL
						&& type != RoomType.TBR
						&& type != RoomType.BLR)
					{
						SetState();
						return;
					}
					break;
				case RuleType.LEFT:
					if (type == RoomType.TBLR
						|| type == RoomType.LR
						|| type == RoomType.TBLR
						|| type == RoomType.L
						|| type == RoomType.TL
						|| type == RoomType.BL
						|| type == RoomType.TBL
						|| type == RoomType.TLR
						|| type == RoomType.BLR)
					{
						SetState();
						return;
					}
					break;
				case RuleType.NOT_LEFT:
					if (type != RoomType.TBLR
						&& type != RoomType.LR
						&& type != RoomType.TBLR
						&& type != RoomType.L
						&& type != RoomType.TL
						&& type != RoomType.BL
						&& type != RoomType.TBL
						&& type != RoomType.TLR
						&& type != RoomType.BLR)
					{
						SetState();
						return;
					}
					break;
				case RuleType.RIGHT:
					if (type == RoomType.TBLR
						|| type == RoomType.LR
						|| type == RoomType.TBLR
						|| type == RoomType.R
						|| type == RoomType.TR
						|| type == RoomType.BR
						|| type == RoomType.TBR
						|| type == RoomType.TLR
						|| type == RoomType.BLR)
					{
						SetState();
						return;
					}
					break;
				case RuleType.NOT_RIGHT:
					if (type != RoomType.TBLR
						&& type != RoomType.LR
						&& type != RoomType.TBLR
						&& type != RoomType.R
						&& type != RoomType.TR
						&& type != RoomType.BR
						&& type != RoomType.TBR
						&& type != RoomType.TLR
						&& type != RoomType.BLR)
					{
						SetState();
						return;
					}
					break;
			}

			if (destroy)
			{
				if (Application.isPlaying)
				{
					Destroy(gameObject);
				}
				else
				{
					DestroyImmediate(gameObject);
				}
			}
		}
		#endregion Main Methods


		#region Utility Methods
		private RoomObject FindRoom()
		{
			Transform t = transform;

			while(t.parent != null)
			{
				RoomObject room = t.GetComponent<RoomObject>();

				if (room != null)
				{
					return room;
				}

				t = t.parent;
			}

			return null;
		}
		private void SetState(bool state = true)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform t = transform.GetChild(i);
				t.gameObject.SetActive(state);
			}
		}
		#endregion Utility Methods
	}
}