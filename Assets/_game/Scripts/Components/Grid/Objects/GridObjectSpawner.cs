using System.Collections;
using System.Collections.Generic;
using _game.Scripts.Components.Grid.Objects.Data;
using _game.Scripts.Components.Grid.Objects.View;
using _game.Scripts.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace _game.Scripts.Components.Grid.Objects
{
    public class GridObjectSpawner : MonoSingleton<GridObjectSpawner>
    {
        [SerializeField] private BoxGridObjectView mBoxGridObjectView;
        
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


        public BoxGridObject SpawnApplianceGridObject(GridManager gridManager, int x, int y,
            BoxGridObjectData data)
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
            
            var applianceGridObject = new BoxGridObject(gridManager, gridCell, mBoxGridObjectView, data, _dict[key]);
            gridCell.SetGridObject(applianceGridObject);
            
            return applianceGridObject;
        }
    }
}