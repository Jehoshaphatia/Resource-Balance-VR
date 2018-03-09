using System;
using UnityEngine;

namespace ResourceBalancing
{
    public class DashboardData : ScriptableObject
    {
        public int woodTotal;
        public int oreTotal;
        public int goldTotal;
        public int prosperityTotal;
        public bool canPlantTrees;
        public bool canBuildCity;
        public bool canBuildPowerPlant;

        public void Reset()
        {
            woodTotal = oreTotal = goldTotal = prosperityTotal = 0;
            canBuildCity = canBuildPowerPlant = false;
        }
    }
}