using System.Collections.Generic;
using _game.Scripts.Components.Grid;
using _game.Scripts.Components.Grid.Data;
using _game.Scripts.Components.Grid.Objects;
using _game.Scripts.Components.Grid.Objects.Data;
using _game.Scripts.Components.InventorySystem;
using _game.Scripts.Components.TaskSystem;
using _game.Scripts.Components.TaskSystem.Data;
using _game.Scripts.Core.Ui;
using _game.Scripts.Ui.Controllers;
using _game.Scripts.Utility;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _game.Scripts.Core
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField] private int m_startProducerCount;
        [SerializeField] private GridSaveManager m_saveManager;
        [SerializeField] private TaskManager m_taskManager;
        [SerializeField] private InventoryManager m_inventoryManager;
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

            // m_saveManager.Initialize(gridManager, "grid_json_data");
            // m_taskManager.Initialize(gridManager);
            // m_inventoryManager.Initialize();

            // var gameEventManager = GameEventManager.Instance;
            // gameEventManager.OnGridObjectAdded -= m_taskManager.OnGridObjectAdded;
            // gameEventManager.OnGridObjectAdded += m_taskManager.OnGridObjectAdded;

            // gameEventManager.OnGridObjectRemoved -= m_taskManager.OnGridObjectRemoved;
            // gameEventManager.OnGridObjectRemoved += m_taskManager.OnGridObjectRemoved;

            // gameEventManager.OnInventoryDrop -= m_inventoryManager.AddItem;
            // gameEventManager.OnInventoryDrop += m_inventoryManager.AddItem;

            // m_taskManager.OnRefresh -= OnRefreshTaskView;
            // m_taskManager.OnRefresh += OnRefreshTaskView;

            // var isLoaded = m_saveManager.LoadGrid();
            // if (!isLoaded)
            // {
            //
            // }
            InitializeGameGrid(gridManager);
        }

        private void OnRefreshTaskView(List<GridTask> taskList)
        {
            var taskViewController = UiManager.Get<GameUiController>().GetTaskViewController();
            taskViewController.Render(taskList);
        }

        private void InitializeGameGrid(GridManager gridManager)
        {
            var dimensions = gridManager.GetDimensions();
            
            for (var y = 0; y < dimensions.y; y++)
            {
                for (var x = 0; x < dimensions.x; x++)
                {
                    GridObjectSpawner.Instance.SpawnApplianceGridObject(gridManager, x, y,
                        ApplianceGridObjectData.GetRandomData());
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