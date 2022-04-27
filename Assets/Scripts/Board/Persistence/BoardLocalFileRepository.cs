using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Common;
using Persistence;

namespace Board.Persistence
{
    public class BoardLocalFileRepository : ScriptableObject, IBoardRepository
    {
        private LocalFileDataContext _dataContext;

        public void Awake()
        {
            _dataContext = new LocalFileDataContext();
        }

        public void Create(BoardData boardData)
        {
            _dataContext.Save(boardData, boardData.userSavedBoardName);
        }

        public void Modify(BoardData boardData)
        {
            throw new NotImplementedException();
        }

        public BoardData Read(string itemToRetrieve)
        {
            var boardData = _dataContext.Load<BoardData>(itemToRetrieve);
            
            // Protobuf deserializes empty collection to null, doesn't appear to use the constructor
            // Catch and inject fix here for lack of a better idea
            boardData.tiles ??= new List<BoardTileData>();
            
            return boardData;
        }

        public List<string> GetSavedFiles()
        {
            // var paths = Directory.GetFiles(_dataContext.SaveDir);
            var paths = new DirectoryInfo(_dataContext.saveDir).GetFiles()
                .OrderByDescending(f => f.LastWriteTime)
                .ToList();
            var list = new List<string>();
            foreach (var path in paths)
            {
                var fileName =  Path.GetFileName(path.FullName);
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