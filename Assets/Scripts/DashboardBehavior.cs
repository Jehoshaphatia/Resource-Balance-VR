using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ResourceBalancing
{
    public class DashboardBehavior : MonoBehaviour
    {
		public DashboardData dashboardData;

 		// Subscribe to dashboard data changed events published by the model
        private void OnEnable()
        {
            GameEventManager.StartListening(ModelEventType.DashboardDataChanged, DashboardDataChanged);
        }

        private void OnDisable()
        {
            GameEventManager.StopListening(ModelEventType.DashboardDataChanged, DashboardDataChanged);
        }

        void Start()
        {
            DashboardDataChanged(0,0);
        }

		// Actual dashboard wouldn't be a single text string, but this is just for demonstration
		public void DashboardDataChanged(int ignore1, int ignore2)
		{
			Text textComponent = GetComponentInChildren<Text>();

			textComponent.text = string.Format("Wood: {0}  Ore: {1}  Gold: {2}  Prosperity: {3}", 
				dashboardData.woodTotal, dashboardData.oreTotal, dashboardData.goldTotal, dashboardData.prosperityTotal);
            
            UpdateButtons();
		}

        private void UpdateButtons()
        {
            GameObject buildCitySelector = GameObject.FindGameObjectWithTag("Build City Selector");
            if (buildCitySelector != null)
            {
                var button = buildCitySelector.GetComponent<Button>();
                button.interactable = dashboardData.canBuildCity;
            }

            GameObject buildPowerPlantSelector = GameObject.FindGameObjectWithTag("Build Power Plant Selector");
            if (buildPowerPlantSelector != null)
            {
                var button = buildPowerPlantSelector.GetComponent<Button>();
                button.interactable = dashboardData.canBuildPowerPlant;
            }

            GameObject plantTreesSelector = GameObject.FindGameObjectWithTag("Plant Trees Selector");
            if (plantTreesSelector != null)
            {
                var button = plantTreesSelector.GetComponent<Button>();
                button.interactable = dashboardData.canPlantTrees;
            }
        }
    }
}
