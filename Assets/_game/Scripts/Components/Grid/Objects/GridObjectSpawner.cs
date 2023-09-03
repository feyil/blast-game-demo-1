using System.Collections;
using System.Collections.Generic;
using _game.Scripts.Components.Grid.Objects.Data;
using _game.Scripts.Components.Grid.Objects.View;
using _game.Scripts.Utility;
using UnityEngine;

namespace _game.Scripts.Components.Grid.Objects
{
    public class GridObjectSpawner : MonoSingleton<GridObjectSpawner>
    {
        [SerializeField] private ApplianceGridObjectView m_applianceGridObjectView;
        [SerializeField] private ProducerGridObjectView m_producerGridObjectViewPrefab;
        private Dictionary<string, int> _dict;

        private void Awake()
        {
            _dict = new Dictionary<string, int>();
            StartCoroutine(Test());
        }

        private IEnumerator Test()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                _dict.Clear();
            }
        }


        public ApplianceGridObject SpawnApplianceGridObject(GridManager gridManager, int x, int y,
            ApplianceGridObjectData data)
        {
            var gridCell = gridManager.GetCell(x, y);
            if (gridCell.IsFilled()) return null;

            var key = $"{x}_{y}";
            if (_dict.ContainsKey(key))
            {
                _dict[key] += 1;
            }
            else
            {
                _dict.Add(key, 0);
            }
            
            var applianceGridObject = new ApplianceGridObject(gridManager, gridCell, m_applianceGridObjectView, data, _dict[key]);
            gridCell.SetGridObject(applianceGridObject);
            
            return applianceGridObject;
        }

        public ProducerGridObject SpawnProducerGridObject(GridManager gridManager, int x, int y,
            ProducerGridObjectData data)
        {
            // var gridCell = gridManager.GetCell(x, y);
            // if (gridCell.IsFilled()) return null;
            //
            // var producerGridObject =
            //     new ProducerGridObject(gridManager, gridCell, m_producerGridObjectViewPrefab, data);
            // gridCell.SetGridObject(producerGridObject);
            //
            // GameEventManager.Instance.TriggerOnGridObjectAdded(producerGridObject);
            return null;
        }
    }
}