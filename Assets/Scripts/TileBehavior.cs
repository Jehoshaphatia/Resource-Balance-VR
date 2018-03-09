using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ResourceBalancing
{
    public class TileBehavior : MonoBehaviour, IPointerClickHandler
    {
        private CellBehavior parentCell;

        public void InitializeTile(CellBehavior parentCell)
        {
            this.parentCell = parentCell;
        }

        public void OnPointerClick(PointerEventData eventdata)
        {
            parentCell.ReceivedPointerClick();
        }

    }
}
