using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResourceBalancing
{
    public class CellBehavior : MonoBehaviour
    {
        private int row, column;
        private BoardManager boardManager;
        private GameObject currentTile = null;

        private static Quaternion defaultTileRotation = Quaternion.identity;

        public GameObject InitializeCell(int row, int column, GameObject tilePrefab, BoardManager boardManager)
        {
            this.row = row;
            this.column = column;
            this.boardManager = boardManager;

            currentTile = InstantiateTile(tilePrefab);

            return currentTile;
        }

        public void ChangeTile(GameObject newTilePrefab)
        {
            //NOTE: better to transfer tiles to a pool and avoid destroying/instantiating for every change

            Destroy(currentTile);
            currentTile = InstantiateTile(newTilePrefab);
        }

        private GameObject InstantiateTile(GameObject tilePrefab)
        {
            Quaternion tileRotation = defaultTileRotation;

            if (boardManager.rotateTilesRandomly)
            {
                float yRot = (float) Random.Range(0, 4) * 90f;
                tileRotation = Quaternion.Euler(0, yRot, 0);
            }

            GameObject tile = Instantiate(tilePrefab, transform.position, tileRotation);
            var tileBehavior = tile.GetComponent<TileBehavior>();

            tileBehavior.InitializeTile(this);
            tile.transform.parent = transform;
            return tile;
        }

        public void ReceivedPointerClick()
        {
            boardManager.CellClicked(row, column);
        }

    }
}