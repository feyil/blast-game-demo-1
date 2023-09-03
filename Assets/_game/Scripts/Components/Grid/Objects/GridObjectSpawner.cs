using System.Collections;
using System.Collections.Generic;
using _game.Scripts.Components.Grid.Data;
using _game.Scripts.Components.Grid.Objects.Data;
using _game.Scripts.Components.Grid.Objects.View;
using _game.Scripts.Utility;
using UnityEngine;

namespace _game.Scripts.Components.Grid.Objects
{
    public class GridObjectSpawner : MonoSingleton<GridObjectSpawner>
    {
        [SerializeField] private BoxGridObjectView m_boxGridObjectView;

        private Dictionary<string, int> _offsetTrackerDict;
        private WaitForEndOfFrame _waitForEndOfFrame;

        private GridConfig _gridConfig;

        public void Initialize(GridConfig gridConfig)
        {
            _gridConfig = gridConfig;
            _offsetTrackerDict = new Dictionary<string, int>();

            _waitForEndOfFrame = new WaitForEndOfFrame();
            StartCoroutine(OffsetCleanUpCoroutine());
        }

        private IEnumerator OffsetCleanUpCoroutine()
        {
            while (true)
            {
                yield return _waitForEndOfFrame;
                if (_offsetTrackerDict.Count != 0)
                {
                    _offsetTrackerDict.Clear();
                }
            }
        }

        public BoxGridObject SpawnBoxGridObject(GridManager gridManager, int x, int y)
        {
            var gridCell = gridManager.GetCell(x, y);
            if (gridCell.IsFilled()) return null;

            var offset = TrackOffset(x, y);

            var data = BoxGridObjectData.GetRandomData(_gridConfig);
            var boxGridObject =
                new BoxGridObject(gridManager, gridCell, m_boxGridObjectView, data, offset);
            gridCell.SetGridObject(boxGridObject);

            return boxGridObject;
        }

        private int TrackOffset(int x, int y)
        {
            var key = $"{x}_{y}";
            if (_offsetTrackerDict.ContainsKey(key))
            {
                _offsetTrackerDict[key] += 1;
            }
            else
            {
                _offsetTrackerDict.Add(key, 0);
            }

            return _offsetTrackerDict[key];
        }
    }
}