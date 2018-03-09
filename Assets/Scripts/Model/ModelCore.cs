using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResourceBalancing;

namespace ResourceBalancing.Model
{
    public class ModelCore : ScriptableObject
    {
        private OperationsBoard opsBoard;
        private GameBoard gameBoard;
        private DashboardData dashData;
        private TileDictionary tileDictionary;
        private GameBoardGenerator boardGenerator;
        private TileDefinition cityDef;
        private TileDefinition powerDef;
        private float pollutionChance = 0.2f;
        private const int PollutionNeighborDegree = 1;
        private const int ProsperityNeighborDegree = 2;


        public void Initialize(TileDictionary tileDict, GameBoardGenerator boardGen, DashboardData dashboardData, float pollutionChance)
        {
            tileDictionary = tileDict;
            boardGenerator = boardGen;
            dashData = dashboardData;
            this.pollutionChance = pollutionChance;
            cityDef = tileDictionary.TileDef[TileType.City];
            powerDef = tileDictionary.TileDef[TileType.PowerPlant];
        }

        public GameBoard BeginGame()
        {
            opsBoard = new OperationsBoard(boardGenerator);
            gameBoard = opsBoard.ToGameBoard();

            dashData.Reset();
            UpdateBuildFlags();
            GameEventManager.TriggerEvent(ModelEventType.DashboardDataChanged, 0, 0);

            return gameBoard;
        }

        public RequestStatus MineCell(int row, int col)
        {
            TileType target = opsBoard.map[row, col].TileValue;
            if (!(target == TileType.Mountain || target == TileType.Trees || target == TileType.City || target == TileType.PowerPlant))
            {
                Debug.Log(string.Format("Cannot mine on a {0} cell", target));
                return RequestStatus.InvalidMiningTarget;
            }

            TileDefinition tileData = tileDictionary.TileDef[target];

            if (target == TileType.Mountain || target == TileType.Trees)
            {
                dashData.oreTotal += tileData.oreYield;
                dashData.woodTotal += tileData.woodYield;
            }
            else
            {
                dashData.oreTotal += tileData.oreCost;
                dashData.woodTotal += tileData.woodCost;
                dashData.goldTotal += tileData.goldCost;
            }

            UpdateBuildFlags();

            TileType newTile = TileType.Dirt;
            switch (target)
            {
                case TileType.Trees:
                    newTile = TileType.Dirt;
                    break;

                case TileType.Mountain:
                    newTile = TileType.Depleted;
                    break;

                case TileType.City:
                case TileType.PowerPlant:
                    newTile = (opsBoard.map[row, col].BuiltOnDepleted) ? TileType.Depleted : TileType.Dirt;
                    break;

                default:
                    throw new System.Exception("Unexpected TileType " + target);
            }

            opsBoard.map[row, col].TileValue = newTile;
            gameBoard.UpdateElement(row, col, opsBoard);

            GameEventManager.TriggerEvent(ModelEventType.CellStateChanged, row, col);
            GameEventManager.TriggerEvent(ModelEventType.DashboardDataChanged, row, col);

            return RequestStatus.Success;
        }

        public RequestStatus Build(int row, int col, BuildOption buildOption)
        {
            TileType currentTile = opsBoard.map[row, col].TileValue;
            if (!((currentTile == TileType.Dirt) || (buildOption != BuildOption.Trees && currentTile == TileType.Depleted)))
            {
                Debug.Log("Invalid build target: " + string.Format("{0},{1}", row, col));
                return RequestStatus.InvalidBuildTarget;
            }

            bool canBuild = false;

            switch (buildOption)
            {
                case BuildOption.City:
                    canBuild = dashData.canBuildCity;
                    break;

                case BuildOption.PowerPlant:
                    canBuild = dashData.canBuildPowerPlant;
                    break;

                case BuildOption.Trees:
                    canBuild = dashData.canPlantTrees;
                    break;

                default:
                    throw new System.Exception("Unexpected build option " + buildOption);

            }

            if (!canBuild)
            {
                Debug.Log("Insufficient resources to build " + buildOption);
                return RequestStatus.InsufficientResources;
            }

            TileType newTile = ConvertToTileType(buildOption);
            TileDefinition newTileData = tileDictionary.TileDef[newTile];

            dashData.oreTotal -= newTileData.oreCost;
            dashData.woodTotal -= newTileData.woodCost;
            dashData.goldTotal -= newTileData.goldCost;

            UpdateBuildFlags();

            opsBoard.map[row, col].BuiltOnDepleted = (currentTile == TileType.Depleted);
            opsBoard.map[row, col].TileValue = newTile;
            gameBoard.UpdateElement(row, col, opsBoard);

            GameEventManager.TriggerEvent(ModelEventType.CellStateChanged, row, col);
            GameEventManager.TriggerEvent(ModelEventType.DashboardDataChanged, row, col);

            return RequestStatus.Success;
        }

        public void BoardUpdate()
        {
            dashData.prosperityTotal = 0;

            for (int i = 0; i < opsBoard.Rows; i++)
                for (int j = 0; j < opsBoard.Columns; j++)
                    if (opsBoard.map[i, j].TileValue == TileType.PowerPlant)
                    {
                        foreach (var index in PolluteAround(i, j))
                        {
                            gameBoard.UpdateElement(index.row, index.col, opsBoard);
                            GameEventManager.TriggerEvent(ModelEventType.CellStateChanged, index.row, index.col);
                        }
                    }

            for (int i = 0; i < opsBoard.Rows; i++)
                for (int j = 0; j < opsBoard.Columns; j++)
                    if (opsBoard.map[i, j].TileValue == TileType.City)
                    {
                        dashData.goldTotal += cityDef.goldProduction;
                        dashData.prosperityTotal += CalcProsperity(i, j);
                    }

            UpdateBuildFlags();
            GameEventManager.TriggerEvent(ModelEventType.DashboardDataChanged, 0, 0);
        }

        private int CalcProsperity(int row, int col)
        {
            int prosperity = cityDef.prosperityContribution;

            var list = opsBoard.map[row, col].Neighbors(opsBoard.Rows, opsBoard.Columns, ProsperityNeighborDegree);
            var counts = new Dictionary<TileType, int>();

            foreach (TileType tile in GameBoardGenerator.AvailableTileTypes)
                counts.Add(tile, 0);
            foreach (var index in list)
                counts[opsBoard.map[index.row, index.col].TileValue]++;

            if (counts[TileType.Trees] < 2 || counts[TileType.Water] < 2)
                prosperity = 0;

            prosperity *= (int)Mathf.Pow(powerDef.prosperityMultiplier, counts[TileType.PowerPlant]);

            return prosperity;
        }

        private List<Index2D> PolluteAround(int row, int col)
        {
            var list = new List<Index2D>();
            MapNode node = opsBoard.map[row, col];

            foreach (Index2D neighbor in node.Neighbors(opsBoard.Rows, opsBoard.Columns, PollutionNeighborDegree))
            {
                TileType? newTile = null;

                switch (opsBoard.map[neighbor.row, neighbor.col].TileValue)
                {
                    case TileType.Water:
                        newTile = TileType.PollutedWater;
                        break;

                    case TileType.Trees:
                    case TileType.Dirt:
                        newTile = TileType.Depleted;
                        break;

                    default:
                        break;
                }

                if (newTile != null && UnityEngine.Random.value < pollutionChance)
                {
                    opsBoard.map[neighbor.row, neighbor.col].TileValue = newTile.Value;
                    list.Add(neighbor);
                }
            }

            return list;
        }

        private void UpdateBuildFlags()
        {
            dashData.canPlantTrees = CanBuild(TileType.Trees);
            dashData.canBuildCity = CanBuild(TileType.City);
            dashData.canBuildPowerPlant = CanBuild(TileType.PowerPlant);
        }

        private bool CanBuild(TileType tile)
        {
            TileDefinition tileDef = tileDictionary.TileDef[tile];

            bool canBuild = dashData.goldTotal >= tileDef.goldCost &&
                dashData.woodTotal >= tileDef.woodCost &&
                dashData.oreTotal >= tileDef.oreCost;

            return canBuild;
        }

        private TileType ConvertToTileType(BuildOption b)
        {
            switch (b)
            {
                case BuildOption.Trees: return TileType.Trees;
                case BuildOption.City: return TileType.City;
                case BuildOption.PowerPlant: return TileType.PowerPlant;
                default:
                    throw new System.Exception("Unexpected build option " + b);
            }
        }
    }
}