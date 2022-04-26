using System;

namespace Board.Persistence
{
    public interface IBoardRepository : IDisposable
    {
        void Create(BoardData boardData);
        void Modify(BoardData boardData);
        BoardData Read(string itemToRetrieve);
        void Delete(BoardData boardData);
    }
}