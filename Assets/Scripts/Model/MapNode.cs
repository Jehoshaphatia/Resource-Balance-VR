using System;
using System.Collections.Generic;
using ResourceBalancing;
using UnityEngine;

namespace ResourceBalancing.Model
{
    [System.Serializable]
    public class MapNode
    {
        public TileType TileValue { get; set; }
        public bool BuiltOnDepleted { get; set; }

        private Index2D self;
        private List<Index2D>[] neighbors;
        private const int MaxDegree = 2;

        public MapNode(int row, int col, TileType tile)
        {
            this.self = new Index2D(row, col);
            this.TileValue = tile;
            this.neighbors = new List<Index2D>[MaxDegree];
        }

        public List<Index2D> Neighbors(int arrayRows, int arrayCols, int degree)
        {
            if (degree < 1 || degree > MaxDegree)
                throw new ArgumentException("Invalid degree value");

            if (neighbors[degree-1] == null)
                neighbors[degree-1] = Util.GetNeighborIndices(self.row, self.col, degree, arrayRows, arrayCols);

            return neighbors[degree-1];
        }
    }
}