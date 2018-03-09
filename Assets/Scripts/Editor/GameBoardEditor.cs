using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ResourceBalancing.Model;

namespace ResourceBalancing
{
    [CustomEditor(typeof(GameBoardGenerator))]
    public class GameBoardEditor : Editor
    {
        private GameBoardGenerator gameBoardGenerator;

        private void OnEnable()
        {
            gameBoardGenerator = (GameBoardGenerator) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorUtility.SetDirty(gameBoardGenerator);
        }
    }
}
