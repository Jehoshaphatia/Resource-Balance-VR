using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResourceBalancing.Model;

namespace ResourceBalancing
{
    [System.Serializable]
    public class GameBoard
    {
        protected TileType[,] map;

        public GameBoard(TileType[,] map)
        {
            this.map = map;
        }

        public GameBoard(IMapSource source)
        {
            map = new TileType[source.Rows, source.Columns];
            for (int i=0; i< source.Rows; i++)
                for (int j=0; j< source.Columns; j++)
                    map[i,j] = source[i,j];
        }
        
        public TileType this [int row, int col]
        {
            get { return map[row, col]; }
        }

        public int Rows
        {
            get { return map.GetLength(0); }
        }

        public int Columns
        {
            get { return map.GetLength(1); }
        }

        public void UpdateElement(int row, int col, IMapSource source)
        {
            map[row, col] = source[row, col];
        }

        public IEnumerable<TileType> AsIEnumerable()
        {
            return map.OfType<TileType>();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < Rows; i++)
            {
                if (i > 0)
                    sb.Append("', ");
                
                for (int j = 0; j < Columns; j++)
                {
                    TileType t = map[i, j];
                    sb.Append(t.ToString());
                    sb.Append(' ');
                }
            }

            return sb.ToString();
        }
    }
}
