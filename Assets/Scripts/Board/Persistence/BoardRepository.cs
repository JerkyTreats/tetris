using System;
using UnityEngine;
using Common;

namespace Board.Persistence
{
    public class BoardRepository : ScriptableObject, IBoardRepository
    {
        private PersistentDataManager _dataContext;

        public void Awake()
        {
            _dataContext = new PersistentDataManager();
        }

        public void Create(BoardData boardData)
        {
            Debug.Log(Application.persistentDataPath); 
            _dataContext.SaveBoard(boardData);
        }

        public void Modify(BoardData boardData)
        {
            throw new NotImplementedException();
        }

        public void Read(BoardData boardData)
        {
            throw new NotImplementedException();
        }

        public void Delete(BoardData boardData)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}