using _game.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _game.Scripts.Components.Grid.Data
{
    [CreateAssetMenu(fileName = "GridConfigScriptableObject", menuName = "ScriptableObjects/GridConfigScriptableObject",
        order = 1)]
    public class GridConfigScriptableObject : ScriptableObject
    {
        public GridConfig GridConfig;

        [Button(ButtonSizes.Large), PropertySpace(30)]
        public void Editor_RestartGame()
        {
            GameManager.Instance.RestartGame();
        }
    }
}