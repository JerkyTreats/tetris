using System;

namespace GameManagement
{
    /// <summary>
    /// Contract defining relationship between GameControllerData and GameManager. Manager initializes Controllers with the data defined in this contract. 
    /// </summary>
    public interface IGameControllerData
    {
        public string GameObjectName { get; set; }
        public GameControllerType GameControllerType { get; set; }
        public Guid Guid { get; set; }
    }
}