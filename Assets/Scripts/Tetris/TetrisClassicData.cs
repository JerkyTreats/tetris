using System;
using Board.Persistence;
using GameManagement;
using ProtoBuf;
using UnityEngine;

namespace Tetris
{
    public class TetrisClassicData : IGameControllerData
    {
        [ProtoMember(1)] public Vector3Int SpawnPosition { get; set; }
        [ProtoMember(2)] public float StepDelay { get; set; }
        [ProtoMember(3)] public float LockDelay { get; set; }
        
        [ProtoMember(4)] public BoardData BoardData { get; set; }
        
        [ProtoMember(5)] public string GameObjectName { get; set; }
        [ProtoMember(6)] public GameControllerType GameControllerType { get; set; }

        public Guid Guid { get; set; }


        public TetrisClassicData(Vector3Int spawnPosition, float stepDelay, float lockDelay, BoardData boardData)
        {
            SpawnPosition = spawnPosition;
            StepDelay = stepDelay;
            LockDelay = lockDelay;
            BoardData = boardData;

            GameObjectName = "TetrisClassicController";
            GameControllerType = GameControllerType.TetrisClassic;
        }
    }
}