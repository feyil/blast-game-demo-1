using _game.Scripts.Components.Grid.Data;
using _game.Scripts.Components.Grid.Objects;
using _game.Scripts.Core.Ui;
using _game.Scripts.Ui.Controllers;
using _game.Scripts.Utility;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _game.Scripts.Core
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField] private GridConfigScriptableObject m_gridConfigSo;

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
        
        private void StartGame()
        {
            var gameUiController = UiManager.Get<GameUiController>();
            gameUiController.Show(m_gridConfigSo.GridConfig, GridObjectSpawner.Instance);
        }

        [Button]
        public void RestartGame()
        {
            var gameUiController = UiManager.Get<GameUiController>();
            var gridManager = gameUiController.GetGridManager();

            gridManager.CleanUp();

            StartGame();
        }
    }
}