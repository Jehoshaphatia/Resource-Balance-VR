using UnityEngine;
using System.Collections.Generic;
using System;

namespace ResourceBalancing.Model
{
    [System.Serializable]
    public class OperationsBoard : IMapSource
    {
        public MapNode[,] map = null;

        public OperationsBoard(GameBoardGenerator boardGenerator)
        {
            map = boardGenerator.Generate();
        }

        public GameBoard ToGameBoard()
        {
            return new GameBoard(this);
        }

        #region IMapSource members

        public TileType this [int row, int col]
        {
            get { return map[row, col].TileValue; }
        }

        public int Rows { get { return map.GetLength(0); } }

        public int Columns { get { return map.GetLength(1); } }

        #endregion
    }
}
