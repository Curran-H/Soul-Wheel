using Coordinates = UnityEngine.Vector2Int;

namespace DungeonMaker
{
	public delegate void VoidDelegate();
	public delegate void XYDelegate(int x, int y);
	public delegate void DungeonObjectDelegate(DungeonObject d);
}

namespace DungeonMaker.Core
{
	public delegate void CoordinatesDelegate(Coordinates c);
	public delegate void RoomNodeDelegate(RoomNode n);
	public delegate void MapDelegate(Map m);
}