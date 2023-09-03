using _game.Scripts.Components.Grid;
using _game.Scripts.Components.Grid.Data;
using _game.Scripts.Components.Grid.Objects;
using _game.Scripts.Core.Ui;
using UnityEngine;

namespace _game.Scripts.Ui.Controllers
{
    public class GameUiController : UiController
    {
        [SerializeField] private GridManager m_gridManager;
        
        public void Show(GridConfig gridConfig, GridObjectSpawner gridObjectSpawner)
        {
            base.Show();
            m_gridManager.SpawnGrid(gridConfig, gridObjectSpawner);
        }

        public GridManager GetGridManager()
        {
            return m_gridManager;
        }
    }
}