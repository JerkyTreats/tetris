using Board.Persistence;
using ProtoBuf;
using UnityEngine;

namespace Tetris
{
    public struct TetrisClassicData
    {
        [ProtoMember(1)] public Vector3Int SpawnPosition { get; set; }
        [ProtoMember(2)] public float StepDelay { get; set; }
        [ProtoMember(3)] public float LockDelay { get; set; }
        
        [ProtoMember(4)] public BoardData BoardData { get; set; }

        public TetrisClassicData(Vector3Int spawnPosition, float stepDelay, float lockDelay, BoardData boardData)
        {
            SpawnPosition = spawnPosition;
            StepDelay = stepDelay;
            LockDelay = lockDelay;
            BoardData = boardData;
        }
    }
}