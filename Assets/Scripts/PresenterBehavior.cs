using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ResourceBalancing
{
    public class PresenterBehavior : MonoBehaviour
    {
        [SerializeField]
        private BoardManager boardManager;

        [SerializeField]
        private AudioManager audioManager;

        private ModelInterface model;
        private GameBoard board;
        private PlayerMode playerMode = PlayerMode.Mining;  // TODO: change this to None when UI allows for mode selection
        private BuildOption buildOption;
        private bool cellsCreated = false;

        #region MonoBehavior events

        void Start()
        {
            model = ModelInterface.Instance;
            NewGame();
        }

        #endregion

        #region Model-Related Calls

        public void NewGame()
        {
            board = model.NewGame();

            if (cellsCreated)
            {
                boardManager.NewBoard(board);
            }
            else
            {
                boardManager.CreateCells(board);
                cellsCreated = true;
            }

            
        }

        public void CellClicked(int row, int column)
        {
            RequestStatus status;

            TileType selectedTile = board[row, column]; // this is the cell's contents before the build/mine action

            switch (playerMode)
            {
                case PlayerMode.Mining:
                    if ((status = model.MineCell(row, column)) == RequestStatus.Success)
                    {
                        // Success: cell was mined, play a mining sound effect
                        audioManager.PlayAudio(playerMode, buildOption, selectedTile);
                    }
                    else
                    {
                        // potentially play an "error" sound to provide feedback that this cell isn't mineable
                        audioManager.PlayAudio(AudioManager.AudioEvent.ButtonClick);
                    }
                    break;

                case PlayerMode.Building:
                    if ((status = model.Build(row, column, buildOption)) == RequestStatus.Success)
                    {
                        TileType newTile = board[row, column]; // this is what was just built

                        // Success: cell was built, play a building sound effect
                        audioManager.PlayAudio(playerMode, buildOption);
                    }
                    else
                    {
                        // potentially play an "error" sound to cue the player that this cell isn't buildable
                        audioManager.PlayAudio(AudioManager.AudioEvent.ButtonClick);
                    }
                    break;

                default:
                    // Player may not have specified any action yet
                    break;
            }
        }

        #endregion

        #region Dashboard Interface

        public PlayerMode GetPlayerMode()
        {
            return playerMode;
        }

        // To be called by the dashboard when mine icon is selected
        public void SetMineMode()
        {
            playerMode = PlayerMode.Mining;
        }

        public void SetBuildCityMode()
        {
            BuildModeSelected(BuildOption.City);
        }

        public void SetBuildPowerPlantMode()
        {
            BuildModeSelected(BuildOption.PowerPlant);
        }
        
        public void SetPlantTreesMode()
        {
            BuildModeSelected(BuildOption.Trees);
        }
        
        #endregion

        private void BuildModeSelected(BuildOption chosenBuildOption)
        {
            playerMode = PlayerMode.Building;
            buildOption = chosenBuildOption;
        }

    }
}
