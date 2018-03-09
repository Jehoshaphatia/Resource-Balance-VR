using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResourceBalancing.Model
{
    public class TileDictionary : ScriptableObject
    {
        [SerializeField] private TileDefinition waterTile;
        [SerializeField] private TileDefinition mountainTile;
        [SerializeField] private TileDefinition treesTile;
        [SerializeField] private TileDefinition dirtTile;
        [SerializeField] private TileDefinition pollutedWaterTile;
        [SerializeField] private TileDefinition depletedTile;
        [SerializeField] private TileDefinition powerPlantTile;
        [SerializeField] private TileDefinition cityTile;

        private Dictionary<TileType, TileDefinition> dict;

        public Dictionary<TileType, TileDefinition> TileDef
        {
            get
            {
                if (dict == null)
                {
                    dict = new Dictionary<TileType, TileDefinition>();

                    if (dirtTile != null) dict.Add(TileType.Dirt, dirtTile);
                    if (mountainTile != null) dict.Add(TileType.Mountain, mountainTile);
                    if (treesTile != null) dict.Add(TileType.Trees, treesTile);
                    if (waterTile != null) dict.Add(TileType.Water, waterTile);
                    if (pollutedWaterTile != null) dict.Add(TileType.PollutedWater, pollutedWaterTile);
                    if (depletedTile != null) dict.Add(TileType.Depleted, depletedTile);
                    if (powerPlantTile != null) dict.Add(TileType.PowerPlant, powerPlantTile);
                    if (cityTile != null) dict.Add(TileType.City, cityTile);
                }

                return dict;
            }
        }
    }
}