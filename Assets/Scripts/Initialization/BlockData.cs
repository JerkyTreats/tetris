using System;
using Board.Persistence;
using UnityEngine.Tilemaps;

namespace Initialization
{
    [Serializable]
    public struct BlockData
    {
        public Block block;
        public Tile tile;
    }
}