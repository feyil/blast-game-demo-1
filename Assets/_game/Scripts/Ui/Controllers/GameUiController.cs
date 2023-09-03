using _game.Scripts.Components.Grid;
using _game.Scripts.Components.Grid.Data;
using _game.Scripts.Components.Grid.Objects;
using _game.Scripts.Core;
using _game.Scripts.Core.Ui;
using UnityEngine;
using UnityEngine.UI;

namespace _game.Scripts.Ui.Controllers
{
    public class GameUiController : UiController
    {
        [SerializeField] private GridManager m_gridManager;
        [SerializeField] private Button m_restartButton;
        [SerializeField] private Button m_shuffleButton;
        
        public void Show(GridConfig gridConfig, GridObjectSpawner gridObjectSpawner)
        {
            base.Show();
            m_gridManager.SpawnGrid(gridConfig, gridObjectSpawner);
            
            m_restartButton.onClick.RemoveAllListeners();
            m_restartButton.onClick.AddListener(() =>
            {
                GameManager.Instance.RestartGame();
            });
            
            m_shuffleButton.onClick.RemoveAllListeners();
            m_shuffleButton.onClick.AddListener(() =>
            {
                m_gridManager.Shuffle();
            });
        }

        public GridManager GetGridManager()
        {
            return m_gridManager;
        }
    }
}