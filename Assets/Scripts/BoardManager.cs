using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResourceBalancing
{
    public class BoardManager : MonoBehaviour
    {
        public GameObject treesTile;
        public GameObject mountainTile;
        public GameObject waterTile;
        public GameObject pollutedWaterTile;
        public GameObject depletedTile;
        public GameObject dirtTile;
        public GameObject cityTile;
        public GameObject powerPlantTile;
        public GameObject cellPrefab;

        public float cellWidth = 1f; // Unity units
        public float cellDepth = 1f;
        public float boardHeight = 0; // board position on Y axis
        public bool rotateTilesRandomly = true;

        private Dictionary<TileType, GameObject> linkedTile;

        private PresenterBehavior presenter;

        private CellBehavior[,] boardCell;

        private GameBoard gameBoard;


        // Subscribe to cell state change events published by the model
        private void OnEnable()
        {
            GameEventManager.StartListening(ModelEventType.CellStateChanged, CellStateChanged);
        }

        private void OnDisable()
        {
            GameEventManager.StopListening(ModelEventType.CellStateChanged, CellStateChanged);
        }

        private void Start()
        {
            presenter = FindObjectOfType<PresenterBehavior>();
            if (presenter == null)
                throw new System.Exception("BoardManager: cannot locate presenter");

            linkedTile = new Dictionary<TileType, GameObject>();

            if (dirtTile != null) linkedTile.Add(TileType.Dirt, dirtTile);
            if (mountainTile != null) linkedTile.Add(TileType.Mountain, mountainTile);
            if (treesTile != null) linkedTile.Add(TileType.Trees, treesTile);
            if (waterTile != null) linkedTile.Add(TileType.Water, waterTile);
            if (pollutedWaterTile != null) linkedTile.Add(TileType.PollutedWater, pollutedWaterTile);
            if (depletedTile != null) linkedTile.Add(TileType.Depleted, depletedTile);
            if (powerPlantTile != null) linkedTile.Add(TileType.PowerPlant, powerPlantTile);
            if (cityTile != null) linkedTile.Add(TileType.City, cityTile);
        }

        public void CreateCells(GameBoard board)
        {
            GameObject tile;

            gameBoard = board;
            boardCell = new CellBehavior[board.Rows, board.Columns];

            // Note: currently handles centering a horizontal board at (0, boardHeight, 0) 

            float xOffset = (board.Columns * cellWidth / 2f - cellWidth / 2f) * -1f;
            float zOffset = (board.Rows * cellDepth / 2f - cellDepth / 2f) * -1f;

            for (int i = 0; i < board.Rows; i++)
            {
                for (int j = 0; j < board.Columns; j++)
                {
                    tile = linkedTile[board[i, j]];

                    Vector3 position = new Vector3(j * cellWidth + xOffset, boardHeight, (board.Rows - 1 - i) * cellDepth + zOffset);

                    GameObject newCell = Instantiate(cellPrefab, position, Quaternion.identity);
                    newCell.transform.parent = this.transform;

                    boardCell[i,j] = newCell.GetComponent<CellBehavior>();
                    boardCell[i,j].InitializeCell(i, j, tile, this);
                }
            }
        }

        // Clicks on board cells are sent to the presenter
        public void CellClicked(int row, int column)
        {
            presenter.CellClicked(row, column);
        }


        // If a cell's state has been changed by the model, update the displayed tile  
        public void CellStateChanged(int row, int column)
        {
            //Get new tile type and tell the cell to update

            TileType t = gameBoard[row, column];

            GameObject tilePrefab = linkedTile[t];
            boardCell[row, column].ChangeTile(tilePrefab);
        }

        public void NewBoard(GameBoard newBoard)
        {
            gameBoard = newBoard;
            
            for (int i=0; i < gameBoard.Rows; i++)
                for (int j=0; j < gameBoard.Columns; j++)
                    CellStateChanged(i, j);
        }
    }
}
