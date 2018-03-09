using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResourceBalancing.Model
{
    [CreateAssetMenu(menuName = "Resource Balancing/New Round Robin Board Generator", fileName = "New Round Robin Board Generator.asset")]
    public class RoundRobinBoardGenerator : GameBoardGenerator
    {
        public override MapNode[,] Generate()
        {
            var map = new MapNode[_rows, _columns];

            int index = 0;

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    map[i, j] = new MapNode(i, j, GameBoardGenerator.AvailableTileTypes[index]);

                    if (++index >= GameBoardGenerator.AvailableTileTypes.Length)
                        index = 0;
                }
            }

            return map;
        }
    }
}