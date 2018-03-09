using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;

namespace ResourceBalancing.Model
{
    [CreateAssetMenu(menuName = "Resource Balancing/New Proportional Board Generator", fileName = "New Proportional Board Generator.asset")]
    public class ProportionalBoardGenerator : GameBoardGenerator
    {
        [Header("Proportions")]
        [SerializeField]
        [Range(0,1)]
        private float waterToLand = 0.15f;

        [SerializeField]
        [Range(0,1)]
        private float treesToMountains = 0.75f;

        [SerializeField]
        [Range(0, 1)]
        private float dirtToMineableLand = 0.4f;

        public override MapNode[,] Generate()
        {
            var map = new MapNode[_rows, _columns];

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    TileType tile = ResourceBalancing.TileType.Water;

                    bool land = Random.value > waterToLand;

                    if (land)
                    {
                        if (Random.value <= dirtToMineableLand)
                            tile = ResourceBalancing.TileType.Dirt;
                        else
                            tile = (Random.value <= treesToMountains) ? 
                                ResourceBalancing.TileType.Trees :
                                ResourceBalancing.TileType.Mountain;
                    }

                    map[i, j] = new MapNode(i, j, tile);
                }
            }

            return map;
        }

    }
}