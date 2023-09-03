using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _game.Scripts.Components.Grid
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private RectTransform m_container;
        [SerializeField] private RectTransform m_objectContainer;
        [SerializeField] private Vector2Int m_dimension;
        [SerializeField] private Vector2 m_spacing;
        [SerializeField] private GridCell m_gridCellPrefab;

        private Dictionary<string, GridCell> _currentGrid;

        [Button]
        public void SpawnGrid()
        {
            CleanUp();
            SpawnGrid(m_container);
        }
        
        private void SpawnGrid(RectTransform contentArea)
        {
            _currentGrid = new Dictionary<string, GridCell>();

            var size = m_gridCellPrefab.GetSize();
            var cellWidth = size.x + m_spacing.x;
            var cellHeight = size.y + m_spacing.y;

            var rowCount = m_dimension.x;
            var columnCount = m_dimension.y;

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

        public Vector2Int GetDimensions()
        {
            return m_dimension;
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
        public void CleanUp()
        {
            if (_currentGrid == null) return;
            foreach (var value in _currentGrid.Values)
            {
                if (Application.isPlaying) Destroy(value.gameObject);
                else DestroyImmediate(value.gameObject);
            }

            _currentGrid = null;
        }
        
        public Dictionary<string, GridCell>.ValueCollection  GetAllCells()
        {
            return _currentGrid.Values;
        }
    }
}