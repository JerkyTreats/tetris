using Board.Persistence;
using ProtoBuf;

namespace GameManagement
{
    public struct GameControllerData
    {
        [ProtoMember(1)] public BoardData BoardData { get; set; }
        [ProtoMember(2)] public string ControllerName { get; set; }
    }
}