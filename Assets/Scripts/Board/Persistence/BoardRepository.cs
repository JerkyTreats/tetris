using System;
using UnityEngine;
using Common;
using Persistence;

namespace Board.Persistence
{
    public class BoardRepository : ScriptableObject, IBoardRepository
    {
        private LocalFileDataContext _dataContext;

        public void Awake()
        {
            _dataContext = new LocalFileDataContext();
        }

        public void Create(BoardData boardData)
        {
            Debug.Log(Application.persistentDataPath); 
            _dataContext.Save<BoardData>(boardData);
        }

        public void Modify(BoardData boardData)
        {
            throw new NotImplementedException();
        }

        public void Read(BoardData boardData)
        {
            throw new NotImplementedException();
        }

        public BoardData Read()
        {
            return _dataContext.Load<BoardData>();
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