using System.Collections.Generic;
using _game.Scripts.Components.Grid.Data;
using _game.Scripts.Components.Grid.Objects;
using _game.Scripts.Components.Grid.Objects.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _game.Scripts.Components.Grid
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private RectTransform m_container;
        [SerializeField] private RectTransform m_objectContainer;
        [SerializeField] private GridCell m_gridCellPrefab;

        private Dictionary<string, GridCell> _currentGrid;
        private GridConfig _gridConfig;

        [Button]
        public void SpawnGrid(GridConfig gridConfig, GridObjectSpawner gridObjectSpawner)
        {
            _gridConfig = gridConfig;

            CleanUp();
            SpawnGrid(m_container);

            gridObjectSpawner.Initialize(_gridConfig);

            InitializeGameGrid();
        }

        private void SpawnGrid(RectTransform contentArea)
        {
            _currentGrid = new Dictionary<string, GridCell>();

            var size = m_gridCellPrefab.GetSize();
            var cellWidth = size.x + _gridConfig.Spacing.x;
            var cellHeight = size.y + _gridConfig.Spacing.y;

            var rowCount = _gridConfig.NumberOfRows;
            var columnCount = _gridConfig.NumberOfColumns;

            for (var currentRow = 0; currentRow < rowCount; currentRow++)
            {
                for (var currentColumn = 0; currentColumn < columnCount; currentColumn++)
                {
                    var gridCell = Instantiate(m_gridCellPrefab, contentArea.transform);

                    var cord = new Vector2Int(currentRow, currentColumn);
                    var localPosition = new Vector2(currentColumn * cellWidth,
                        -currentRow * cellHeight);

                    gridCell.Initialize(this, cord, localPosition);

                    var index = gridCell.GetIndex();
                    _currentGrid.Add(index, gridCell);
                }
            }
        }

        private void InitializeGameGrid()
        {
            var rowCount = _gridConfig.NumberOfRows;
            var columnCount = _gridConfig.NumberOfColumns;

            for (var y = 0; y < columnCount; y++)
            {
                for (var x = 0; x < rowCount; x++)
                {
                    GridObjectSpawner.Instance.SpawnBoxGridObject(this, x, y);
                }
            }
        }

        public Vector2Int GetDimensions()
        {
            return new Vector2Int(_gridConfig.NumberOfRows, _gridConfig.NumberOfColumns);
        }

        public RectTransform GetObjectContainer()
        {
            return m_objectContainer;
        }

        [Button]
        public GridCell GetCell(int x, int y)
        {
            var index = GridCell.GetIndex(x, y);
            _currentGrid.TryGetValue(index, out var cell);
            return cell;
        }

        [Button]
        public void Shuffle()
        {
            var rowsCount = _gridConfig.NumberOfRows;
            var columnCount = _gridConfig.NumberOfColumns;

            for (var i = 0; i < rowsCount; i++)
            {
                for (var j = 0; j < columnCount; j++)
                {
                    int randomRow = Random.Range(0, rowsCount);
                    int randomColumn = Random.Range(0, columnCount);

                    var tempCell = GetCell(i, j);
                    var tempObject = tempCell.GetGridObject();

                    var randomCell = GetCell(randomRow, randomColumn);
                    var randomObject = randomCell.GetGridObject();

                    tempCell.SetGridObject(randomObject);
                    randomCell.SetGridObject(tempObject);
                }
            }
        }

        [Button]
        public void CleanUp()
        {
            if (_currentGrid == null) return;
            foreach (var value in _currentGrid.Values)
            {
                var gridObject = value.GetGridObject();
                if (gridObject != null)
                {
                    gridObject.Destroy();
                }

                Destroy(value.gameObject);
            }

            _currentGrid = null;
        }

        [Button]
        public bool IsDeadlock()
        {
            foreach (var gridCell in _currentGrid.Values)
            {
                var gridObject = gridCell.GetGridObject();
                if (gridObject == null) continue;
                if (gridObject.IsBlastable()) return false;
            }

            return true;
        }

        public Dictionary<string, GridCell>.ValueCollection GetAllCells()
        {
            return _currentGrid.Values;
        }

        public int GetBlastableGroupCount()
        {
            return _gridConfig.BlastableGroupCount;
        }

        public void CheckAndHandleDeadlock()
        {
            //TODO What to do if the deadlock not broken?
            var isDeadlock = IsDeadlock();
            if (isDeadlock)
            {
                Shuffle();
            }
        }
    }
}