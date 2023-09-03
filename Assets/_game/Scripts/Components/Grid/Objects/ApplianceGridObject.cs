using System.Collections.Generic;
using _game.Scripts.Components.Grid.Objects.Data;
using _game.Scripts.Components.Grid.Objects.View;
using UnityEngine;

namespace _game.Scripts.Components.Grid.Objects
{
    public class ApplianceGridObject : CommonGridObject
    {
        private readonly ApplianceGridObjectData _data;
        private readonly ApplianceGridObjectView _viewSpecific;

        public ApplianceGridObject(GridManager gridManager, GridCell gridCell, ApplianceGridObjectView viewPrefab,
            ApplianceGridObjectData data, int offset) : base(gridManager, gridCell, viewPrefab.gameObject, offset)
        {
            _data = data;
            _viewSpecific = _view.GetComponent<ApplianceGridObjectView>();
            Refresh();
        }

        private void Refresh()
        {
            _viewSpecific.Render(_data);
        }

        public override IItemData GetData()
        {
            return _data;
        }

        public override bool CanMerge(IGridObject gridObject)
        {
            if (gridObject is not ApplianceGridObject applianceGridObject) return false;

            return applianceGridObject._data.Number == _data.Number;
        }

        public override bool Merge(IGridObject gridObject)
        {
            var applianceGridObject = gridObject as ApplianceGridObject;
            if (applianceGridObject == null) return false;

            var gridObjectNumber = applianceGridObject._data.Number;
            if (gridObjectNumber == ApplianceGridObjectData.MAX_VALUE) return false;

            var cord = _gridCell.GetCord();
            gridObject.Destroy();
            Destroy();

            GridObjectSpawner.Instance.SpawnApplianceGridObject(_gridManager, cord.x, cord.y,
                new ApplianceGridObjectData()
                {
                    Number = _data.Number * 2
                });

            return true;
        }

        public override void OnInteract()
        {
            var affectedCellList = new HashSet<ApplianceGridObject>();
            CheckNeighbouringCells(affectedCellList);
            if (affectedCellList.Count < 2) return;

            foreach (var affectedCell in affectedCellList)
            {
                affectedCell.Destroy();
                
                var upCell = affectedCell._gridCell.GetUpCell();
                if (upCell == null)
                {
                    var cord = affectedCell._gridCell.GetCord();
                    GridObjectSpawner.Instance.SpawnApplianceGridObject(_gridManager, cord.x, cord.y,
                        ApplianceGridObjectData.GetRandomData());
                    continue;
                }
                upCell.Fall(affectedCell._gridCell);
            }
        }

        private void CheckNeighbouringCells(HashSet<ApplianceGridObject> affectedCellList)
        {
            var cord = _gridCell.GetCord();
            for (var i = -1; i <= 1; i += 2)
            {
                var gridCell = _gridManager.GetCell(cord.x, cord.y + i);
                if (gridCell == null) continue;

                var gridObject = gridCell.GetGridObject();
                if (gridObject == null) continue;

                var applianceGridObject = gridObject as ApplianceGridObject;
                if (applianceGridObject == null) continue;
                if (!applianceGridObject.IsDestroyed() && applianceGridObject.GetNumber() == GetNumber())
                {
                    if(affectedCellList.Contains(applianceGridObject)) continue;
                    affectedCellList.Add(applianceGridObject);
                    applianceGridObject.CheckNeighbouringCells(affectedCellList);
                }
            }
            
            
            for (var i = -1; i <= 1; i += 2)
            {
                var gridCell = _gridManager.GetCell(cord.x + i, cord.y);
                if (gridCell == null) continue;

                var gridObject = gridCell.GetGridObject();
                if (gridObject == null) continue;

                var applianceGridObject = gridObject as ApplianceGridObject;
                if (applianceGridObject == null) continue;
                if (!applianceGridObject.IsDestroyed() && applianceGridObject.GetNumber() == GetNumber())
                {
                    if(affectedCellList.Contains(applianceGridObject)) continue;
                    affectedCellList.Add(applianceGridObject);
                    applianceGridObject.CheckNeighbouringCells(affectedCellList);
                }
            }
        }

        public int GetNumber()
        {
            return _data.Number;
        }

        public void RenderTaskTargetView()
        {
            _data.Color = Color.green;
            _viewSpecific.Render(_data);
        }
    }
}