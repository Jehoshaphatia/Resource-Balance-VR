using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using ResourceBalancing.Model;

namespace ResourceBalancing
{
    public class ModelInterface : MonoBehaviour
    {
        [SerializeField] [Range(1, 60)] private int updatesPerMinute = 4;
        [SerializeField] [Range(0f, 1f)] private float pollutionChance = 0.5f;
        [SerializeField] private GameBoardGenerator gameBoardGenerator;
        [SerializeField] private ModelCore modelCore;
        [SerializeField] private TileDictionary tileDictionary;
        [SerializeField] private DashboardData dashboardData;

        private float nextBeat;
        private float beatInterval;
        private bool modelInitialized = false;

        private static ModelInterface retainedInstance;

        public static ModelInterface Instance
        {
            get
            {
                if (!retainedInstance)
                {
                    retainedInstance = FindObjectOfType<ModelInterface>();

                    if (!retainedInstance)
                    {
                        Debug.LogError("There must be one active ModelInterface script on a GameObject in the scene.");
                    }
                }

                return retainedInstance;
            }
        }

    
        public void Start()
        {
            float beatsPerSecond = updatesPerMinute / 60f;
            beatInterval = 1.0f / beatsPerSecond;
            nextBeat = Time.time + beatInterval;
        }

        void Update()
        {
            if (Time.time > nextBeat)
            {
                nextBeat = Time.time + beatInterval;
                modelCore.BoardUpdate();
            }
        }

        public GameBoard NewGame()
        {
            if (!modelInitialized)
            {
                modelCore.Initialize(tileDictionary, gameBoardGenerator, dashboardData, pollutionChance);
                modelInitialized = true;
            }
            
            return modelCore.BeginGame();
        }

        public RequestStatus MineCell(int row, int column)
        {
            return modelCore.MineCell(row, column);
        }

        public RequestStatus Build(int row, int col, BuildOption buildOption)
        {
            return modelCore.Build(row, col, buildOption);
        }

    }
}
