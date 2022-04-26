using System;
using System.Collections.Generic;
using System.IO;
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
            _dataContext.Save<BoardData>(boardData);
        }

        public void Modify(BoardData boardData)
        {
            throw new NotImplementedException();
        }

        public BoardData Read(string itemToRetrieve)
        {
            return _dataContext.Load<BoardData>(itemToRetrieve);
        }

        public List<string> GetSavedFiles()
        {
            var paths = Directory.GetFiles(_dataContext.SaveDir);
            var list = new List<string>();
            foreach (var path in paths)
            {
                var fileName =  Path.GetFileName(path);
                if (fileName.Contains(LocalFileDataContext.SaveFileName))
                    list.Add(fileName);
            }

            return list;
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