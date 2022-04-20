using System;

namespace Board.Persistence
{
    public interface IBoardRepository : IDisposable
    {
        void Create(BoardData boardData);
        void Modify(BoardData boardData);
        void Read(BoardData boardData);
        void Delete(BoardData boardData);
    }
}