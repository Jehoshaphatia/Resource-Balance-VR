using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResourceBalancing.Model
{
    public class TileDefinition : ScriptableObject
    {
        public string displayName;
        public char code;

        [Header("Building")]
        public int goldCost;
        public int oreCost;
        public int woodCost;

        [Header("Mining")]        
        public int oreYield;
        public int woodYield;

        [Header("Production per Update")]
        public int goldProduction;

        [Header("City Prosperity")]
        public int prosperityContribution;
        public int prosperityMultiplier = 1;

    }
}
