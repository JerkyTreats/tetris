using System;

namespace GameManagement
{
    public interface IGameControllerData
    {
        public string GameObjectName { get; set; }
        public GameControllerType GameControllerType { get; set; }
        public Guid Guid { get; set; }
    }
}