using _game.Scripts.Components.Grid;
using _game.Scripts.Components.Grid.Objects;
using _game.Scripts.Components.Grid.Objects.Data;
using _game.Scripts.Core.Ui;
using _game.Scripts.Ui.Controllers;
using _game.Scripts.Utility;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _game.Scripts.Core
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField] private Color m_twoColor;
        [SerializeField] private Color m_fourColor;
        [SerializeField] private Color m_eightColor;
        [SerializeField] private Color m_sixteenColor;
        [SerializeField] private Color m_thirtTwoColor;
        [SerializeField] private Color m_sixyFourColor;

        private void Awake()
        {
            InitializeAwake();
        }

        private void Start()
        {
            InitializeStart();
        }

        private void InitializeAwake()
        {
            UiManager.Instance.Initialize();
        }

        private void InitializeStart()
        {
            StartGame();
        }

        [Button]
        private void StartGame()
        {
            var gameUiController = UiManager.Get<GameUiController>();
            gameUiController.Show();

            var gridManager = gameUiController.GetGridManager();
            InitializeGameGrid(gridManager);
        }
        
        private void InitializeGameGrid(GridManager gridManager)
        {
            var dimensions = gridManager.GetDimensions();
            
            for (var y = 0; y < dimensions.y; y++)
            {
                for (var x = 0; x < dimensions.x; x++)
                {
                    GridObjectSpawner.Instance.SpawnApplianceGridObject(gridManager, x, y,
                        BoxGridObjectData.GetRandomData());
                }
            }
        }

        public Color GetColor(int number)
        {
            switch (number)
            {
                case 2:
                    return m_twoColor;
                case 4:
                    return m_fourColor;
                case 8:
                    return m_eightColor;
                case 16:
                    return m_sixteenColor;
                case 32:
                    return m_thirtTwoColor;
                case 64:
                    return m_sixyFourColor;
            }

            return Color.black;
        }
    }
}