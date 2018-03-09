using System;
using System.Collections.Generic;
using UnityEngine;

namespace ResourceBalancing.Model
{
    public static class Util 
    {
        public static List<Index2D> GetNeighborIndices(int cellRow, int cellColumn, int distance, int numRows, int numCols)
        {
            int startRow = Mathf.Clamp(cellRow-distance, 0, cellRow);
            int endRow = Mathf.Clamp(cellRow+distance, cellRow, numRows-1);

            int startCol = Mathf.Clamp(cellColumn-distance, 0, cellColumn);
            int endCol = Mathf.Clamp(cellColumn+distance, cellColumn, numCols-1);

            var list = new List<Index2D>();

            for (int i=startRow; i <= endRow; i++)
                for (int j=startCol; j <= endCol; j++)
                    if (!(i==cellRow && j==cellColumn))
                        list.Add(new Index2D(i,j));

            return list;
        } 
    }
}