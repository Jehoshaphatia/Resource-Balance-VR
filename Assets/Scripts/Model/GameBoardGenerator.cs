using System;
using UnityEngine;
using ResourceBalancing.Model;

namespace ResourceBalancing.Model
{
    public abstract class GameBoardGenerator : ScriptableObject
    {
        [Header("Game Board Settings")]

        [Tooltip("Number of rows")]
        [Range(2, 10)]
        [SerializeField]
        protected int _rows = 6;

        [Tooltip("Number of columns")]
        [Range(4, 20)]
        [SerializeField]
        protected int _columns = 12;

        public static readonly TileType[] AvailableTileTypes = { TileType.City, TileType.Depleted,
                TileType.Dirt, TileType.Mountain, TileType.PollutedWater,
                TileType.PowerPlant, TileType.Trees, TileType.Water };

        public virtual MapNode[,] Generate()
        {
            throw new NotImplementedException();
        }

    }
}
