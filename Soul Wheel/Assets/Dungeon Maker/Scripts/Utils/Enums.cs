namespace DungeonMaker
{
    public enum ModeType
    {
        _2D,
        _3D
    }

    public enum RuleType
    {
        TOP,
        BOTTOM,
        LEFT,
        RIGHT,
        NOT_TOP,
        NOT_BOTTOM,
        NOT_LEFT,
        NOT_RIGHT
    }

    public enum RoomType
    {
        NONE,
        TBLR,
        TB,
        LR,
        T,
        B,
        L,
        R,
        TL,
        TR,
        BL,
        BR,
        TBL,
        TBR,
        TLR,
        BLR
    }

    public enum CorridorType
    {
        I,
        L,
        T,
        X
    }
}

namespace DungeonMaker.Core
{
    public enum CorrelatorState
	{
        STOPPED,
        RUNNING,
        DONE
	}

    public enum NodeType
    {
        NONE,
        ROOM
    }
}

namespace DungeonMaker.Editor
{
    public enum MenuType
    {
        NONE,
        WORK,
        NODE,
        CONNECTION
    }

    public enum ActionType
    {
        NONE,
        CREATE_GRAPH,
        LOAD_GRAPH,
        RELOAD_GRAPH,
        UNLOAD_GRAPH,
        NEW_ROOM,
        COPY_ROOM,
        PASTE_ROOM,
        CREATE_NODE,
        EDIT_NODE,
        CONNECT_NODE,
        DISCONNECT_NODE,
        ISOLATE_NODE,
        REMOVE_NODE,
        EMPTY_GRAPH,
        DISCONNECT_ALL
    }
}