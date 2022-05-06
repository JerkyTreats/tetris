using System;
using ProtoBuf;
using Tetris;

namespace GameManagement
{
    public class GameControllerData : IGameControllerData
    {
        public string GameObjectName { get; set; }
        public GameControllerType GameControllerType { get; set; }
        public Guid Guid { get; set; }
    }
}