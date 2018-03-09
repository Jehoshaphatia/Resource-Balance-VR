using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;

namespace ResourceBalancing.Model
{
    [CreateAssetMenu(menuName = "Resource Balancing/New Random Board Generator", fileName = "New Random Board Generator.asset")]
    public class RandomBoardGenerator : GameBoardGenerator
    {
        public override MapNode[,] Generate()
        {
            var map = new MapNode[_rows, _columns];

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    map[i, j] = new MapNode(i, j, GameBoardGenerator.AvailableTileTypes[Random.Range(0, GameBoardGenerator.AvailableTileTypes.Length)]);
                }
            }

            return map;
        }

    }
}